using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AmqCompanySecretarialManager.Migrations;

[DbContext(typeof(SecretarialDbContext))]
partial class SecretarialDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

        modelBuilder.Entity("AmqCompanySecretarialManager.Company", b =>
        {
            b.Property<Guid>("Id");
            b.Property<DateOnly>("Ard");
            b.Property<string>("BusinessAddress");
            b.Property<string>("CroNumber");
            b.Property<string>("Name");
            b.Property<string>("Notes");
            b.Property<string>("RegisteredOffice");
            b.Property<string>("Status");
            b.Property<DateOnly>("YearEnd");
            b.HasKey("Id");
            b.ToTable("Companies");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.Document", b =>
        {
            b.Property<Guid>("Id");
            b.Property<string>("Checksum");
            b.Property<Guid>("CompanyId");
            b.Property<DateTime>("CreatedAtUtc");
            b.Property<string>("PathOrBlob");
            b.Property<Guid?>("RelatedId");
            b.Property<string>("RelatedType");
            b.Property<Guid>("UploadedBy");
            b.HasKey("Id");
            b.ToTable("Documents");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.DirectorCurrent", b =>
        {
            b.Property<Guid>("Id");
            b.Property<DateOnly>("AppointmentDate");
            b.Property<Guid>("CompanyId");
            b.Property<Guid>("PersonId");
            b.Property<DateOnly?>("ResignationDate");
            b.Property<string>("Role");
            b.Property<string>("ServiceAddress");
            b.HasKey("Id");
            b.ToTable("Directors");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.Filing", b =>
        {
            b.Property<Guid>("Id");
            b.Property<string>("Authority");
            b.Property<Guid>("CompanyId");
            b.Property<DateOnly>("DueDate");
            b.Property<DateOnly?>("FiledDate");
            b.Property<Guid>("FilingTypeId");
            b.Property<string>("Notes");
            b.Property<string>("Period");
            b.Property<string>("ReferenceNumber");
            b.Property<string>("Status");
            b.HasKey("Id");
            b.ToTable("Filings");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.FilingType", b =>
        {
            b.Property<Guid>("Id");
            b.Property<string>("Authority");
            b.Property<string>("ChecklistItemsJson");
            b.Property<string>("Name");
            b.Property<string>("SchemaJson");
            b.HasKey("Id");
            b.ToTable("FilingTypes");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.InterestCurrent", b =>
        {
            b.Property<Guid>("Id");
            b.Property<Guid>("CompanyId");
            b.Property<string>("Details");
            b.Property<DateOnly?>("EndDate");
            b.Property<string>("EvidenceReference");
            b.Property<string>("InterestType");
            b.Property<Guid>("PersonId");
            b.Property<DateOnly>("StartDate");
            b.HasKey("Id");
            b.ToTable("Interests");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.MemberCurrent", b =>
        {
            b.Property<Guid>("Id");
            b.Property<Guid>("CompanyId");
            b.Property<DateOnly?>("EndDate");
            b.Property<string>("HolderName");
            b.Property<Guid?>("PersonId");
            b.Property<string>("ShareClass");
            b.Property<int>("SharesHeld");
            b.Property<DateOnly>("StartDate");
            b.HasKey("Id");
            b.ToTable("Members");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.MinuteRecord", b =>
        {
            b.Property<Guid>("Id");
            b.Property<Guid>("CompanyId");
            b.Property<DateOnly>("MeetingDate");
            b.Property<string>("Summary");
            b.Property<string>("Title");
            b.HasKey("Id");
            b.ToTable("Minutes");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.Person", b =>
        {
            b.Property<Guid>("Id");
            b.Property<string>("Address");
            b.Property<DateOnly?>("DateOfBirth");
            b.Property<string>("Identifiers");
            b.Property<string>("Name");
            b.Property<string>("Nationality");
            b.HasKey("Id");
            b.ToTable("People");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.RboCurrent", b =>
        {
            b.Property<Guid>("Id");
            b.Property<string>("BeneficialOwnerName");
            b.Property<Guid>("CompanyId");
            b.Property<DateOnly?>("EndDate");
            b.Property<string>("EvidenceReference");
            b.Property<string>("IdentifierLast4");
            b.Property<string>("NatureOfControl");
            b.Property<DateOnly>("StartDate");
            b.HasKey("Id");
            b.ToTable("RboEntries");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.RegisterEvent", b =>
        {
            b.Property<Guid>("Id");
            b.Property<Guid>("CompanyId");
            b.Property<Guid>("EntityId");
            b.Property<string>("EntityType");
            b.Property<string>("EventType");
            b.Property<DateTime>("OccurredAtUtc");
            b.Property<string>("PayloadJson");
            b.Property<string>("PrevHash");
            b.Property<DateTime>("RecordedAtUtc");
            b.Property<string>("Reason");
            b.Property<string>("ThisHash");
            b.Property<Guid>("UserId");
            b.Property<string>("WorkstationId");
            b.HasKey("Id");
            b.HasIndex("CompanyId", "RecordedAtUtc");
            b.ToTable("RegisterEvents");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.SecretaryCurrent", b =>
        {
            b.Property<Guid>("Id");
            b.Property<DateOnly>("AppointmentDate");
            b.Property<Guid>("CompanyId");
            b.Property<string>("Details");
            b.Property<Guid>("PersonId");
            b.Property<DateOnly?>("ResignationDate");
            b.HasKey("Id");
            b.ToTable("Secretaries");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.ShareCertificateCurrent", b =>
        {
            b.Property<Guid>("Id");
            b.Property<Guid>("CompanyId");
            b.Property<string>("CertificateNumber");
            b.Property<DateOnly>("IssuedDate");
            b.Property<Guid>("MemberId");
            b.Property<int>("SharesCovered");
            b.HasKey("Id");
            b.ToTable("ShareCertificates");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.TaskItem", b =>
        {
            b.Property<Guid>("Id");
            b.Property<Guid>("CompanyId");
            b.Property<string>("AssignedTo");
            b.Property<DateOnly>("DueDate");
            b.Property<string>("Status");
            b.Property<string>("Title");
            b.HasKey("Id");
            b.ToTable("Tasks");
        });

        modelBuilder.Entity("AmqCompanySecretarialManager.User", b =>
        {
            b.Property<Guid>("Id");
            b.Property<string>("PasswordHash");
            b.Property<string>("Role");
            b.Property<string>("Username");
            b.HasKey("Id");
            b.ToTable("Users");
        });
    }
}
