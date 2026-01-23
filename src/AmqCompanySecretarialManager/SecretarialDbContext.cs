using Microsoft.EntityFrameworkCore;

namespace AmqCompanySecretarialManager;

public sealed class SecretarialDbContext : DbContext
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RegisterEvent> RegisterEvents => Set<RegisterEvent>();
    public DbSet<DirectorCurrent> Directors => Set<DirectorCurrent>();
    public DbSet<SecretaryCurrent> Secretaries => Set<SecretaryCurrent>();
    public DbSet<MemberCurrent> Members => Set<MemberCurrent>();
    public DbSet<ShareCertificateCurrent> ShareCertificates => Set<ShareCertificateCurrent>();
    public DbSet<InterestCurrent> Interests => Set<InterestCurrent>();
    public DbSet<RboCurrent> RboEntries => Set<RboCurrent>();
    public DbSet<FilingType> FilingTypes => Set<FilingType>();
    public DbSet<Filing> Filings => Set<Filing>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<MinuteRecord> Minutes => Set<MinuteRecord>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    public SecretarialDbContext(DbContextOptions<SecretarialDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RegisterEvent>()
            .HasIndex(e => new { e.CompanyId, e.RecordedAtUtc });

        modelBuilder.Entity<RegisterEvent>()
            .Property(e => e.PayloadJson)
            .HasColumnType("TEXT");

        modelBuilder.Entity<RegisterEvent>()
            .Property(e => e.Reason)
            .IsRequired();

        modelBuilder.Entity<RegisterEvent>()
            .Property(e => e.ThisHash)
            .IsRequired();

        modelBuilder.Entity<FilingType>()
            .Property(f => f.SchemaJson)
            .HasColumnType("TEXT");

        modelBuilder.Entity<FilingType>()
            .Property(f => f.ChecklistItemsJson)
            .HasColumnType("TEXT");

        var companyA = new Company
        {
            Id = Guid.Parse("b95aa3b3-7694-4b7e-b14f-46ce3fc9a6d4"),
            Name = "AMQ Sample Holdings Limited",
            CroNumber = "123456",
            YearEnd = new DateOnly(2024, 12, 31),
            Ard = new DateOnly(2025, 6, 30),
            RegisteredOffice = "1 Sample Quay, Dublin",
            BusinessAddress = "1 Sample Quay, Dublin",
            Status = "Active"
        };

        var companyB = new Company
        {
            Id = Guid.Parse("c1303587-3b33-4e97-9e65-8164b698d0de"),
            Name = "AMQ Advisory Services Limited",
            CroNumber = "654321",
            YearEnd = new DateOnly(2024, 9, 30),
            Ard = new DateOnly(2025, 3, 31),
            RegisteredOffice = "22 Finance Street, Cork",
            BusinessAddress = "22 Finance Street, Cork",
            Status = "Active"
        };

        var companyC = new Company
        {
            Id = Guid.Parse("a3b4c2e2-56b7-4a94-97cc-1cd8b6ed27ad"),
            Name = "AMQ Trustees Limited",
            CroNumber = "987654",
            YearEnd = new DateOnly(2024, 6, 30),
            Ard = new DateOnly(2024, 12, 31),
            RegisteredOffice = "5 Harbour View, Galway",
            BusinessAddress = "5 Harbour View, Galway",
            Status = "Active"
        };

        modelBuilder.Entity<Company>().HasData(companyA, companyB, companyC);

        var b1Type = new FilingType
        {
            Id = Guid.Parse("a235a7d7-7ff0-43ba-9cfe-9d8b5030d11a"),
            Name = "CRO B1 Annual Return",
            Authority = "CRO",
            ChecklistItemsJson = "[\"Financial statements\",\"Signatures\",\"Directors declaration\"]",
            SchemaJson = "{\"fields\":[{\"key\":\"ard\",\"label\":\"ARD\",\"type\":\"date\"},{\"key\":\"approvalDate\",\"label\":\"Approval Date\",\"type\":\"date\"}]}"
        };

        var b10Type = new FilingType
        {
            Id = Guid.Parse("1691b2ea-9428-4e8a-8d61-4b78af7070a2"),
            Name = "CRO B10 Change in Director/Secretary",
            Authority = "CRO",
            ChecklistItemsJson = "[\"Appointment/resignation confirmations\",\"Identity evidence\"]",
            SchemaJson = "{\"fields\":[{\"key\":\"changeType\",\"label\":\"Change Type\",\"type\":\"text\"},{\"key\":\"effectiveDate\",\"label\":\"Effective Date\",\"type\":\"date\"}]}"
        };

        modelBuilder.Entity<FilingType>().HasData(b1Type, b10Type);
    }
}
