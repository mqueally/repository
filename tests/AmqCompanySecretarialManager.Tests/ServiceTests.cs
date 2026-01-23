using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AmqCompanySecretarialManager.Tests;

public sealed class ServiceTests
{
    [Fact]
    public void RebuildHoldings_ReplaysEventsAsAtDate()
    {
        var service = new MemberRegisterService();
        var now = DateTime.UtcNow;

        var events = new List<RegisterEvent>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                EntityId = Guid.NewGuid(),
                EntityType = "Member",
                EventType = "ALLOTMENT",
                PayloadJson = JsonSerializer.Serialize(new ShareAllotment("Anne Walsh", "Ordinary", 100, now.AddDays(-10))),
                OccurredAtUtc = now.AddDays(-10),
                RecordedAtUtc = now.AddDays(-10),
                UserId = Guid.NewGuid(),
                WorkstationId = "WS-1",
                Reason = "Initial allotment"
            },
            new()
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                EntityId = Guid.NewGuid(),
                EntityType = "Member",
                EventType = "TRANSFER",
                PayloadJson = JsonSerializer.Serialize(new ShareTransfer("Anne Walsh", "John Byrne", "Ordinary", 40, now.AddDays(-2))),
                OccurredAtUtc = now.AddDays(-2),
                RecordedAtUtc = now.AddDays(-2),
                UserId = Guid.NewGuid(),
                WorkstationId = "WS-1",
                Reason = "Transfer"
            }
        };

        var result = service.RebuildHoldings(events, now.AddDays(-1));

        Assert.Collection(result,
            item => Assert.Equal(60, item.SharesHeld),
            item => Assert.Equal(40, item.SharesHeld));
    }

    [Fact]
    public void TransferValidation_ThrowsWhenExceedingHoldings()
    {
        var service = new MemberRegisterService();
        var now = DateTime.UtcNow;

        var events = new List<RegisterEvent>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                EntityId = Guid.NewGuid(),
                EntityType = "Member",
                EventType = "ALLOTMENT",
                PayloadJson = JsonSerializer.Serialize(new ShareAllotment("Anne Walsh", "Ordinary", 10, now.AddDays(-5))),
                OccurredAtUtc = now.AddDays(-5),
                RecordedAtUtc = now.AddDays(-5),
                UserId = Guid.NewGuid(),
                WorkstationId = "WS-1",
                Reason = "Initial allotment"
            },
            new()
            {
                Id = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                EntityId = Guid.NewGuid(),
                EntityType = "Member",
                EventType = "TRANSFER",
                PayloadJson = JsonSerializer.Serialize(new ShareTransfer("Anne Walsh", "John Byrne", "Ordinary", 40, now.AddDays(-2))),
                OccurredAtUtc = now.AddDays(-2),
                RecordedAtUtc = now.AddDays(-2),
                UserId = Guid.NewGuid(),
                WorkstationId = "WS-1",
                Reason = "Transfer"
            }
        };

        Assert.Throws<InvalidOperationException>(() => service.RebuildHoldings(events, now));
    }

    [Fact]
    public async Task AuditHashChainVerification_DetectsTampering()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<SecretarialDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var setupContext = new SecretarialDbContext(options))
        {
            await setupContext.Database.EnsureCreatedAsync();
        }

        var companyId = Guid.NewGuid();
        await using (var context = new SecretarialDbContext(options))
        {
            var eventService = new RegisterEventService(context);

            await eventService.AppendEventAsync(new RegisterEvent
            {
                Id = Guid.NewGuid(),
                CompanyId = companyId,
                EntityType = "Director",
                EntityId = Guid.NewGuid(),
                EventType = "APPOINT",
                PayloadJson = "{}",
                OccurredAtUtc = DateTime.UtcNow.AddDays(-1),
                UserId = Guid.NewGuid(),
                WorkstationId = "WS-1",
                Reason = "Appointment"
            });

            await eventService.AppendEventAsync(new RegisterEvent
            {
                Id = Guid.NewGuid(),
                CompanyId = companyId,
                EntityType = "Director",
                EntityId = Guid.NewGuid(),
                EventType = "RESIGN",
                PayloadJson = "{}",
                OccurredAtUtc = DateTime.UtcNow,
                UserId = Guid.NewGuid(),
                WorkstationId = "WS-1",
                Reason = "Resignation"
            });
        }

        await using (var validationContext = new SecretarialDbContext(options))
        {
            var result = await AuditHashService.VerifyChainAsync(validationContext, companyId);
            Assert.True(result.IsValid);
        }

        await using (var tamperContext = new SecretarialDbContext(options))
        {
            var firstEvent = await tamperContext.RegisterEvents.FirstAsync();
            firstEvent.PayloadJson = "{\"tampered\":true}";
            await tamperContext.SaveChangesAsync();
        }

        await using (var validationContext = new SecretarialDbContext(options))
        {
            var result = await AuditHashService.VerifyChainAsync(validationContext, companyId);
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Failures);
        }
    }

    [Fact]
    public void FilingPackBuilder_IncludesEvidenceAndSections()
    {
        var builder = new FilingPackBuilder();
        var company = new Company { Name = "AMQ Sample Holdings Limited" };
        var filing = new Filing { Authority = "CRO", Period = "2024" };
        var evidence = new List<Document>
        {
            new() { PathOrBlob = "evidence/statement.pdf" },
            new() { PathOrBlob = "evidence/signature.png" }
        };

        var pack = builder.BuildPack(company, filing, evidence);

        Assert.Contains("Cover Sheet", pack.Sections);
        Assert.Equal(2, pack.EvidencePaths.Count);
    }
}
