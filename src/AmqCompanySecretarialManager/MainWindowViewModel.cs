using System;
using System.Collections.ObjectModel;

namespace AmqCompanySecretarialManager;

public sealed class MainWindowViewModel
{
    public DashboardViewModel Dashboard { get; } = new();
    public ObservableCollection<Company> Companies { get; } = new();
    public Company? SelectedCompany { get; set; }
    public ObservableCollection<OverviewItem> SelectedCompanyOverview { get; } = new();

    public ObservableCollection<string> RegisterTypes { get; } = new()
    {
        "Register of Members",
        "Register of Directors",
        "Register of Secretary",
        "Register of Interests",
        "Register of Beneficial Ownership (RBO)"
    };

    public string? SelectedRegisterType { get; set; }
    public DateTime? AsAtDate { get; set; } = DateTime.Today;

    public ObservableCollection<object> RegisterRows { get; } = new();
    public ObservableCollection<Filing> FilingItems { get; } = new();
    public ObservableCollection<object> Minutes { get; } = new();
    public ObservableCollection<Document> Documents { get; } = new();
    public ObservableCollection<object> Tasks { get; } = new();
    public ObservableCollection<RegisterEvent> AuditEvents { get; } = new();

    public DateTime? PackStartDate { get; set; } = DateTime.Today.AddMonths(-12);
    public DateTime? PackEndDate { get; set; } = DateTime.Today;
    public bool IncludeRegisters { get; set; } = true;
    public bool IncludeMinutes { get; set; } = true;
    public bool IncludeFilings { get; set; } = true;
    public bool IncludeRbo { get; set; } = true;
    public bool IncludeAudit { get; set; } = true;

    public MainWindowViewModel()
    {
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "AMQ Sample Holdings Limited",
            CroNumber = "123456",
            Status = "Active",
            RegisteredOffice = "1 Sample Quay, Dublin",
            BusinessAddress = "1 Sample Quay, Dublin",
            YearEnd = new DateOnly(2024, 12, 31),
            Ard = new DateOnly(2025, 6, 30)
        };
        Companies.Add(company);
        SelectedCompany = company;
        SelectedCompanyOverview.Add(new OverviewItem("CRO Number", company.CroNumber));
        SelectedCompanyOverview.Add(new OverviewItem("Year End", company.YearEnd.ToString()));
        SelectedCompanyOverview.Add(new OverviewItem("ARD", company.Ard.ToString()));
        SelectedCompanyOverview.Add(new OverviewItem("Registered Office", company.RegisteredOffice));
        SelectedCompanyOverview.Add(new OverviewItem("Business Address", company.BusinessAddress));
        SelectedCompanyOverview.Add(new OverviewItem("Status", company.Status));
    }
}

public sealed record OverviewItem(string Label, string Value);

public sealed class DashboardViewModel
{
    public ObservableCollection<string> UpcomingDeadlines { get; } = new()
    {
        "B1 Annual Return due in 14 days",
        "RBO confirmation due in 30 days"
    };

    public ObservableCollection<string> RecentRegisterChanges { get; } = new()
    {
        "Director appointment recorded for Mary O'Connor",
        "Share transfer recorded from John Byrne to Anne Walsh"
    };

    public ObservableCollection<string> FilingsInProgress { get; } = new()
    {
        "B10 - Director resignation (Draft)",
        "B1 - Annual return (Awaiting evidence)"
    };
}
