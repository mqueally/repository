using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmqCompanySecretarialManager.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Companies",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                CroNumber = table.Column<string>(type: "TEXT", nullable: false),
                YearEnd = table.Column<DateOnly>(type: "TEXT", nullable: false),
                Ard = table.Column<DateOnly>(type: "TEXT", nullable: false),
                RegisteredOffice = table.Column<string>(type: "TEXT", nullable: false),
                BusinessAddress = table.Column<string>(type: "TEXT", nullable: false),
                Status = table.Column<string>(type: "TEXT", nullable: false),
                Notes = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Companies", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "FilingTypes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Authority = table.Column<string>(type: "TEXT", nullable: false),
                ChecklistItemsJson = table.Column<string>(type: "TEXT", nullable: false),
                SchemaJson = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FilingTypes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "People",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Address = table.Column<string>(type: "TEXT", nullable: false),
                DateOfBirth = table.Column<DateOnly>(type: "TEXT", nullable: true),
                Nationality = table.Column<string>(type: "TEXT", nullable: true),
                Identifiers = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_People", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Username = table.Column<string>(type: "TEXT", nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                Role = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Directors",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                PersonId = table.Column<Guid>(type: "TEXT", nullable: false),
                Role = table.Column<string>(type: "TEXT", nullable: false),
                AppointmentDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                ResignationDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                ServiceAddress = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Directors", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Secretaries",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                PersonId = table.Column<Guid>(type: "TEXT", nullable: false),
                AppointmentDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                ResignationDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                Details = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Secretaries", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Members",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                PersonId = table.Column<Guid>(type: "TEXT", nullable: true),
                HolderName = table.Column<string>(type: "TEXT", nullable: false),
                ShareClass = table.Column<string>(type: "TEXT", nullable: false),
                SharesHeld = table.Column<int>(type: "INTEGER", nullable: false),
                StartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                EndDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Members", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ShareCertificates",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                MemberId = table.Column<Guid>(type: "TEXT", nullable: false),
                CertificateNumber = table.Column<string>(type: "TEXT", nullable: false),
                SharesCovered = table.Column<int>(type: "INTEGER", nullable: false),
                IssuedDate = table.Column<DateOnly>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ShareCertificates", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Interests",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                PersonId = table.Column<Guid>(type: "TEXT", nullable: false),
                InterestType = table.Column<string>(type: "TEXT", nullable: false),
                Details = table.Column<string>(type: "TEXT", nullable: false),
                StartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                EndDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                EvidenceReference = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Interests", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RboEntries",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                BeneficialOwnerName = table.Column<string>(type: "TEXT", nullable: false),
                NatureOfControl = table.Column<string>(type: "TEXT", nullable: false),
                StartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                EndDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                IdentifierLast4 = table.Column<string>(type: "TEXT", nullable: true),
                EvidenceReference = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RboEntries", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Filings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                FilingTypeId = table.Column<Guid>(type: "TEXT", nullable: false),
                Authority = table.Column<string>(type: "TEXT", nullable: false),
                DueDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                Period = table.Column<string>(type: "TEXT", nullable: false),
                Status = table.Column<string>(type: "TEXT", nullable: false),
                FiledDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                ReferenceNumber = table.Column<string>(type: "TEXT", nullable: true),
                Notes = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Filings", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Documents",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                RelatedType = table.Column<string>(type: "TEXT", nullable: false),
                RelatedId = table.Column<Guid>(type: "TEXT", nullable: true),
                PathOrBlob = table.Column<string>(type: "TEXT", nullable: false),
                Checksum = table.Column<string>(type: "TEXT", nullable: false),
                CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                UploadedBy = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Documents", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Minutes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                MeetingDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                Title = table.Column<string>(type: "TEXT", nullable: false),
                Summary = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Minutes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Tasks",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                Title = table.Column<string>(type: "TEXT", nullable: false),
                AssignedTo = table.Column<string>(type: "TEXT", nullable: false),
                DueDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                Status = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tasks", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RegisterEvents",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                EntityType = table.Column<string>(type: "TEXT", nullable: false),
                EntityId = table.Column<Guid>(type: "TEXT", nullable: false),
                EventType = table.Column<string>(type: "TEXT", nullable: false),
                PayloadJson = table.Column<string>(type: "TEXT", nullable: false),
                Reason = table.Column<string>(type: "TEXT", nullable: false),
                OccurredAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                RecordedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                WorkstationId = table.Column<string>(type: "TEXT", nullable: false),
                PrevHash = table.Column<string>(type: "TEXT", nullable: true),
                ThisHash = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RegisterEvents", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_RegisterEvents_CompanyId_RecordedAtUtc",
            table: "RegisterEvents",
            columns: new[] { "CompanyId", "RecordedAtUtc" });

        migrationBuilder.InsertData(
            table: "Companies",
            columns: new[] { "Id", "Name", "CroNumber", "YearEnd", "Ard", "RegisteredOffice", "BusinessAddress", "Status", "Notes" },
            values: new object[]
            {
                new Guid("b95aa3b3-7694-4b7e-b14f-46ce3fc9a6d4"),
                "AMQ Sample Holdings Limited",
                "123456",
                new DateOnly(2024, 12, 31),
                new DateOnly(2025, 6, 30),
                "1 Sample Quay, Dublin",
                "1 Sample Quay, Dublin",
                "Active",
                null
            });

        migrationBuilder.InsertData(
            table: "Companies",
            columns: new[] { "Id", "Name", "CroNumber", "YearEnd", "Ard", "RegisteredOffice", "BusinessAddress", "Status", "Notes" },
            values: new object[]
            {
                new Guid("c1303587-3b33-4e97-9e65-8164b698d0de"),
                "AMQ Advisory Services Limited",
                "654321",
                new DateOnly(2024, 9, 30),
                new DateOnly(2025, 3, 31),
                "22 Finance Street, Cork",
                "22 Finance Street, Cork",
                "Active",
                null
            });

        migrationBuilder.InsertData(
            table: "Companies",
            columns: new[] { "Id", "Name", "CroNumber", "YearEnd", "Ard", "RegisteredOffice", "BusinessAddress", "Status", "Notes" },
            values: new object[]
            {
                new Guid("a3b4c2e2-56b7-4a94-97cc-1cd8b6ed27ad"),
                "AMQ Trustees Limited",
                "987654",
                new DateOnly(2024, 6, 30),
                new DateOnly(2024, 12, 31),
                "5 Harbour View, Galway",
                "5 Harbour View, Galway",
                "Active",
                null
            });

        migrationBuilder.InsertData(
            table: "FilingTypes",
            columns: new[] { "Id", "Name", "Authority", "ChecklistItemsJson", "SchemaJson" },
            values: new object[]
            {
                new Guid("a235a7d7-7ff0-43ba-9cfe-9d8b5030d11a"),
                "CRO B1 Annual Return",
                "CRO",
                "[\"Financial statements\",\"Signatures\",\"Directors declaration\"]",
                "{\"fields\":[{\"key\":\"ard\",\"label\":\"ARD\",\"type\":\"date\"},{\"key\":\"approvalDate\",\"label\":\"Approval Date\",\"type\":\"date\"}]}"
            });

        migrationBuilder.InsertData(
            table: "FilingTypes",
            columns: new[] { "Id", "Name", "Authority", "ChecklistItemsJson", "SchemaJson" },
            values: new object[]
            {
                new Guid("1691b2ea-9428-4e8a-8d61-4b78af7070a2"),
                "CRO B10 Change in Director/Secretary",
                "CRO",
                "[\"Appointment/resignation confirmations\",\"Identity evidence\"]",
                "{\"fields\":[{\"key\":\"changeType\",\"label\":\"Change Type\",\"type\":\"text\"},{\"key\":\"effectiveDate\",\"label\":\"Effective Date\",\"type\":\"date\"}]}"
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "RegisterEvents");
        migrationBuilder.DropTable(name: "Documents");
        migrationBuilder.DropTable(name: "Filings");
        migrationBuilder.DropTable(name: "FilingTypes");
        migrationBuilder.DropTable(name: "Interests");
        migrationBuilder.DropTable(name: "Members");
        migrationBuilder.DropTable(name: "Minutes");
        migrationBuilder.DropTable(name: "People");
        migrationBuilder.DropTable(name: "RboEntries");
        migrationBuilder.DropTable(name: "Secretaries");
        migrationBuilder.DropTable(name: "ShareCertificates");
        migrationBuilder.DropTable(name: "Tasks");
        migrationBuilder.DropTable(name: "Users");
        migrationBuilder.DropTable(name: "Directors");
        migrationBuilder.DropTable(name: "Companies");
    }
}
