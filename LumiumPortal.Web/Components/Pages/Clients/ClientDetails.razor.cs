using Domain.Enums;
using Domain.Enums.Clients;
using Domain.Enums.Contracts;
using Domain.Enums.Documents;
using Lumium.Application.Features.Clients.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients;

public partial class ClientDetails : ComponentBase
{
    [Parameter] public Guid ClientId { get; set; }

    private ClientDetailsDto? _client;
    private bool _isLoading = true;
    private decimal _totalContractValue => _client?.Contracts.Sum(c => c.Value) ?? 0;

    protected override async Task OnInitializedAsync()
    {
        await LoadClientDetails();
    }

    private async Task LoadClientDetails()
    {
        _isLoading = true;

        // Simulacija async call
        await Task.Delay(200);

        // Dummy data
        _client = new ClientDetailsDto
        {
            Id = ClientId,
            Name = "TechCorp d.o.o.",
            LegalForm = LegalForm.NonGovernmentalOrganization,
            TaxNumber = "12345678",
            TaxIdentificationNumber = "987654321",
            IsPdv = true,
            ResponsiblePerson = "Marko Marković",
            BackupPerson = "Ana Anić",
            Address = "Knez Mihailova 15",
            PhoneNumber = "+381 11 1234567",
            Director = "Petar Petrović",
            Email = "info@techcorp.rs",
            Country = "Srbija",
            RiskLevel = RiskLevel.Low,
            IsActive = true,
            CreatedAt = new DateTime(2024, 6, 15),
            
            Industry = "Informacione tehnologije",
            CompanySize = "50-100 zaposlenih",
            MonthlyFee = 150000,
            BillingContact = "finance@techcorp.rs",
            AssignedTo = "Sarah Mitchell",
            
            Contracts =
            [
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Mesečno knjigovodstvo",
                    Type = ContractType.Recurring,
                    StartDate = new DateTime(2024, 6, 15),
                    EndDate = new DateTime(2025, 6, 15),
                    Value = 1800000,
                    Status = ContractStatus.Active
                },

                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Godišnja poreska prijava",
                    Type = ContractType.OneTime,
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 12, 31),
                    Value = 300000,
                    Status = ContractStatus.Completed
                }
            ],
            
            Certificates =
            [
                new()
                {
                    Id = Guid.NewGuid(),
                    CertificateName = "Poreski sertifikat",
                    IssueDate = new DateTime(2026, 1, 30),
                    ExpiryDate = new DateTime(2027, 1, 30),
                    RegulatoryBodyName = "Poreska uprava"
                },

                new()
                {
                    Id = Guid.NewGuid(),
                    CertificateName = "Poslovna dozvola",
                    IssueDate = new DateTime(2025, 12, 15),
                    ExpiryDate = new DateTime(2026, 12, 15),
                    RegulatoryBodyName = "Gradska uprava"
                }
            ],
            
            Documents =
            [
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Finansijski izveštaj Q4 2025.pdf",
                    Category = DocumentCategory.Financial,
                    UploadDate = new DateTime(2026, 1, 28),
                    UploadedBy = "Sarah Mitchell",
                    Size = "2.3 MB"
                },

                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Poreski izveštaj 2025.pdf",
                    Category = DocumentCategory.Tax,
                    UploadDate = new DateTime(2026, 1, 25),
                    UploadedBy = "John Smith",
                    Size = "1.8 MB"
                }
            ]
        };

        _isLoading = false;
    }
    
    private void HandleAddContract()
    {
        Snackbar.Add("Funkcionalnost 'Novi ugovor' - uskoro", Severity.Info);
    }

    private void HandleAddCertificate()
    {
        Snackbar.Add("Funkcionalnost 'Dodaj sertifikat' - uskoro", Severity.Info);
    }

    private void HandleUploadDocument()
    {
        Snackbar.Add("Funkcionalnost 'Otpremi dokument' - uskoro", Severity.Info);
    }

    private void HandleDownloadDocument(Guid documentId)
    {
        Snackbar.Add($"Preuzimanje dokumenta {documentId} - uskoro", Severity.Info);
    }
}