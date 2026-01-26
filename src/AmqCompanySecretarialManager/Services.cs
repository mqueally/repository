using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AmqCompanySecretarialManager;

public sealed class RegisterEventService
{
    private readonly SecretarialDbContext _dbContext;

    public RegisterEventService(SecretarialDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RegisterEvent> AppendEventAsync(RegisterEvent registerEvent)
    {
        registerEvent.RecordedAtUtc = DateTime.UtcNow;

        var previousHash = await _dbContext.RegisterEvents
            .Where(e => e.CompanyId == registerEvent.CompanyId)
            .OrderByDescending(e => e.RecordedAtUtc)
            .Select(e => e.ThisHash)
            .FirstOrDefaultAsync();

        registerEvent.PrevHash = previousHash;
        registerEvent.ThisHash = AuditHashService.CalculateHash(registerEvent, previousHash);

        _dbContext.RegisterEvents.Add(registerEvent);
        await _dbContext.SaveChangesAsync();

        return registerEvent;
    }
}

public static class AuditHashService
{
    public static string CalculateHash(RegisterEvent registerEvent, string? previousHash)
    {
        var payload = string.Join('|',
            previousHash ?? string.Empty,
            registerEvent.CompanyId,
            registerEvent.EntityType,
            registerEvent.EntityId,
            registerEvent.EventType,
            registerEvent.PayloadJson,
            registerEvent.OccurredAtUtc.ToString("O"),
            registerEvent.RecordedAtUtc.ToString("O"),
            registerEvent.UserId,
            registerEvent.WorkstationId,
            registerEvent.Reason);

        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes);
    }

    public static async Task<AuditChainResult> VerifyChainAsync(SecretarialDbContext dbContext, Guid companyId)
    {
        var events = await dbContext.RegisterEvents
            .Where(e => e.CompanyId == companyId)
            .OrderBy(e => e.RecordedAtUtc)
            .ToListAsync();

        string? previousHash = null;
        var failures = new List<AuditChainFailure>();

        foreach (var registerEvent in events)
        {
            var expected = CalculateHash(registerEvent, previousHash);
            if (!string.Equals(expected, registerEvent.ThisHash, StringComparison.OrdinalIgnoreCase))
            {
                failures.Add(new AuditChainFailure(registerEvent.Id, expected, registerEvent.ThisHash));
            }

            previousHash = registerEvent.ThisHash;
        }

        return new AuditChainResult(failures.Count == 0, failures);
    }
}

public sealed record AuditChainFailure(Guid EventId, string ExpectedHash, string ActualHash);

public sealed record AuditChainResult(bool IsValid, IReadOnlyCollection<AuditChainFailure> Failures);

public sealed class MemberRegisterService
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public IReadOnlyList<MemberHoldingSnapshot> RebuildHoldings(IEnumerable<RegisterEvent> events, DateTime asAtUtc)
    {
        var holdings = new Dictionary<string, MemberHoldingSnapshot>(StringComparer.OrdinalIgnoreCase);

        foreach (var registerEvent in events
                     .Where(e => e.OccurredAtUtc <= asAtUtc)
                     .OrderBy(e => e.OccurredAtUtc))
        {
            switch (registerEvent.EventType.ToUpperInvariant())
            {
                case "ALLOTMENT":
                    ApplyAllotment(holdings, JsonSerializer.Deserialize<ShareAllotment>(registerEvent.PayloadJson, _serializerOptions));
                    break;
                case "TRANSFER":
                    ApplyTransfer(holdings, JsonSerializer.Deserialize<ShareTransfer>(registerEvent.PayloadJson, _serializerOptions));
                    break;
                case "CANCELLATION":
                    ApplyCancellation(holdings, JsonSerializer.Deserialize<ShareCancellation>(registerEvent.PayloadJson, _serializerOptions));
                    break;
                default:
                    break;
            }
        }

        return holdings.Values.ToList();
    }

    private static void ApplyAllotment(Dictionary<string, MemberHoldingSnapshot> holdings, ShareAllotment? allotment)
    {
        if (allotment is null)
        {
            return;
        }

        var key = BuildKey(allotment.HolderName, allotment.ShareClass);
        if (!holdings.TryGetValue(key, out var snapshot))
        {
            snapshot = new MemberHoldingSnapshot(allotment.HolderName, allotment.ShareClass, 0);
        }

        snapshot = snapshot with { SharesHeld = snapshot.SharesHeld + allotment.Shares };
        holdings[key] = snapshot;
    }

    private static void ApplyTransfer(Dictionary<string, MemberHoldingSnapshot> holdings, ShareTransfer? transfer)
    {
        if (transfer is null)
        {
            return;
        }

        var fromKey = BuildKey(transfer.FromHolder, transfer.ShareClass);
        var toKey = BuildKey(transfer.ToHolder, transfer.ShareClass);

        if (!holdings.TryGetValue(fromKey, out var fromSnapshot) || fromSnapshot.SharesHeld < transfer.Shares)
        {
            throw new InvalidOperationException("Cannot transfer more shares than held at transfer date.");
        }

        holdings[fromKey] = fromSnapshot with { SharesHeld = fromSnapshot.SharesHeld - transfer.Shares };

        if (!holdings.TryGetValue(toKey, out var toSnapshot))
        {
            toSnapshot = new MemberHoldingSnapshot(transfer.ToHolder, transfer.ShareClass, 0);
        }

        holdings[toKey] = toSnapshot with { SharesHeld = toSnapshot.SharesHeld + transfer.Shares };
    }

    private static void ApplyCancellation(Dictionary<string, MemberHoldingSnapshot> holdings, ShareCancellation? cancellation)
    {
        if (cancellation is null)
        {
            return;
        }

        var key = BuildKey(cancellation.HolderName, cancellation.ShareClass);
        if (!holdings.TryGetValue(key, out var snapshot) || snapshot.SharesHeld < cancellation.Shares)
        {
            throw new InvalidOperationException("Cannot cancel more shares than held at cancellation date.");
        }

        holdings[key] = snapshot with { SharesHeld = snapshot.SharesHeld - cancellation.Shares };
    }

    private static string BuildKey(string holderName, string shareClass) => $"{holderName}|{shareClass}";
}

public sealed record MemberHoldingSnapshot(string HolderName, string ShareClass, int SharesHeld);

public sealed record ShareAllotment(string HolderName, string ShareClass, int Shares, DateTime EffectiveDateUtc);

public sealed record ShareTransfer(string FromHolder, string ToHolder, string ShareClass, int Shares, DateTime EffectiveDateUtc);

public sealed record ShareCancellation(string HolderName, string ShareClass, int Shares, DateTime EffectiveDateUtc);

public sealed class FilingPackBuilder
{
    public FilingPack BuildPack(Company company, Filing filing, IReadOnlyCollection<Document> evidence)
    {
        var items = new List<string>
        {
            "Cover Sheet",
            "Required Data",
            "Checklist",
            "Evidence"
        };

        return new FilingPack(company.Name, filing.Authority, filing.Period, items, evidence.Select(d => d.PathOrBlob).ToList());
    }
}

public sealed record FilingPack(string CompanyName, string Authority, string Period, IReadOnlyCollection<string> Sections, IReadOnlyCollection<string> EvidencePaths);

public sealed class PdfReportService
{
    public byte[] BuildRegisterReport(string title, IReadOnlyCollection<MemberHoldingSnapshot> snapshots)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        using var stream = new MemoryStream();

        QuestPDF.Fluent.Document.Create(container =>
            container.Page(page =>
            {
                page.Margin(30);
                page.Content().Column(column =>
                {
                    column.Item().Text(title).FontSize(18).Bold();
                    foreach (var snapshot in snapshots)
                    {
                        column.Item().Text($"{snapshot.HolderName} - {snapshot.ShareClass}: {snapshot.SharesHeld}");
                    }
                });
            })).GeneratePdf(stream);

        return stream.ToArray();
    }
}
