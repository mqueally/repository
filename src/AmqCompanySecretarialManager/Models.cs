using System;
using System.Collections.Generic;

namespace AmqCompanySecretarialManager;

public sealed class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CroNumber { get; set; } = string.Empty;
    public DateOnly YearEnd { get; set; }
    public DateOnly Ard { get; set; }
    public string RegisteredOffice { get; set; } = string.Empty;
    public string BusinessAddress { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }

    public ICollection<RegisterEvent> RegisterEvents { get; set; } = new List<RegisterEvent>();
    public ICollection<Filing> Filings { get; set; } = new List<Filing>();
}

public sealed class Person
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public string? Identifiers { get; set; }
}

public sealed class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Staff";
}

public sealed class RegisterEvent
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string PayloadJson { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime OccurredAtUtc { get; set; }
    public DateTime RecordedAtUtc { get; set; }
    public Guid UserId { get; set; }
    public string WorkstationId { get; set; } = string.Empty;
    public string? PrevHash { get; set; }
    public string ThisHash { get; set; } = string.Empty;

    public Company? Company { get; set; }
}

public sealed class DirectorCurrent
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid PersonId { get; set; }
    public string Role { get; set; } = "Director";
    public DateOnly AppointmentDate { get; set; }
    public DateOnly? ResignationDate { get; set; }
    public string ServiceAddress { get; set; } = string.Empty;
}

public sealed class SecretaryCurrent
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid PersonId { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public DateOnly? ResignationDate { get; set; }
    public string Details { get; set; } = string.Empty;
}

public sealed class MemberCurrent
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? PersonId { get; set; }
    public string HolderName { get; set; } = string.Empty;
    public string ShareClass { get; set; } = string.Empty;
    public int SharesHeld { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}

public sealed class ShareCertificateCurrent
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid MemberId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public int SharesCovered { get; set; }
    public DateOnly IssuedDate { get; set; }
}

public sealed class InterestCurrent
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid PersonId { get; set; }
    public string InterestType { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? EvidenceReference { get; set; }
}

public sealed class RboCurrent
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string BeneficialOwnerName { get; set; } = string.Empty;
    public string NatureOfControl { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? IdentifierLast4 { get; set; }
    public string? EvidenceReference { get; set; }
}

public sealed class FilingType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Authority { get; set; } = "CRO";
    public string ChecklistItemsJson { get; set; } = "[]";
    public string SchemaJson { get; set; } = "{}";
}

public sealed class Filing
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid FilingTypeId { get; set; }
    public string Authority { get; set; } = "CRO";
    public DateOnly DueDate { get; set; }
    public string Period { get; set; } = string.Empty;
    public string Status { get; set; } = "Draft";
    public DateOnly? FiledDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Notes { get; set; }

    public Company? Company { get; set; }
    public FilingType? FilingType { get; set; }
}

public sealed class Document
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string RelatedType { get; set; } = string.Empty;
    public Guid? RelatedId { get; set; }
    public string PathOrBlob { get; set; } = string.Empty;
    public string Checksum { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public Guid UploadedBy { get; set; }
}

public sealed class MinuteRecord
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public DateOnly MeetingDate { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
}

public sealed class TaskItem
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    public DateOnly DueDate { get; set; }
    public string Status { get; set; } = "Open";
}
