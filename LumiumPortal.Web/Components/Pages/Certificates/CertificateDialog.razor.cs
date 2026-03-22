using Lumium.Application.Common.Models;
using Lumium.Application.Features.Certificates.Commands;
using Lumium.Application.Features.Certificates.DTOs;
using Lumium.Application.Features.Clients.Queries;
using Lumium.Application.Features.RegulatoryBodies.DTOs;
using Lumium.Application.Features.RegulatoryBodies.Queries;
using LumiumPortal.Web.Components.Pages.Certificates.Validators;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Certificates;

public partial class CertificateDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public Guid ClientId { get; set; }
    [Parameter] public CertificateDto? ExistingCertificate  { get; set; } = new();
    [Parameter] public bool IsEditMode { get; set; }

    private List<(Guid Id, string Name)> _clients = [];
    private List<RegulatoryBodyDto> _regulatoryBodies = [];
    private CertificateFormDto _model = new();
    private MudForm? _form;
    private readonly CertificateFormDtoValidator _validator = new();
    private bool _isSubmitting;
    private bool DisableClientSelection => ClientId != Guid.Empty || ExistingCertificate?.ClientId != Guid.Empty;

    private DateTime? _issueDate = DateTime.Today;
    private DateTime? _expiryDate = DateTime.Today.AddYears(1);

    protected override async Task OnInitializedAsync()
    {
        await LoadRegulatoryBodies();
        await LoadClients();
        
        if (IsEditMode && ExistingCertificate != null)
        {
            _model = new CertificateFormDto
            {
                CertificateName = ExistingCertificate.CertificateName,
                CertificateNumber = ExistingCertificate.CertificateNumber,
                IssueDate = ExistingCertificate.IssueDate,
                ExpiryDate = ExistingCertificate.ExpiryDate,
                RegulatoryBodyId = ExistingCertificate.RegulatoryBodyId,
                SelectedClient = _clients.FirstOrDefault(c => c.Id == ExistingCertificate.ClientId)
            };

            _issueDate = ExistingCertificate.IssueDate;
            _expiryDate = ExistingCertificate.ExpiryDate;
        }
        else
        {
            // Create mode
            _model = new CertificateFormDto
            {
                IssueDate = DateTime.Today,
                ExpiryDate = DateTime.Today.AddYears(1)
            };

            _issueDate = DateTime.Today;
            _expiryDate = DateTime.Today.AddYears(1);

            PreselectClient();
        }
    }

    private async Task LoadClients()
    {
        try
        {
            _clients = await Mediator.Send(new GetClientIdsQuery());
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška pri učitavanju klijenata: {ex.Message}", Severity.Error);
            Console.WriteLine($"[ERROR] Load clients failed: {ex}");
        }
    }

    private async Task LoadRegulatoryBodies()
    {
        try
        {
            _regulatoryBodies = await Mediator.Send(new GetRegulatoryBodiesQuery());
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška pri učitavanju regulatornih tela: {ex.Message}", Severity.Error);
            Console.WriteLine($"[ERROR] Load regulatory bodies failed: {ex}");
        }
    }

    private void PreselectClient()
    {
        if (ClientId != Guid.Empty)
        {
            _model.SelectedClient = _clients.FirstOrDefault(c => c.Id == ClientId);
        }
    }

    protected override void OnParametersSet()
    {
        if (_issueDate.HasValue)
        {
            _model.IssueDate = _issueDate.Value;
        }

        if (_expiryDate.HasValue)
        {
            _model.ExpiryDate = _expiryDate.Value;
        }
    }

    private async Task HandleSubmit()
    {
        if (_issueDate.HasValue)
        {
            _model.IssueDate = _issueDate.Value;
        }

        if (_expiryDate.HasValue)
        {
            _model.ExpiryDate = _expiryDate.Value;
        }

        if (!_issueDate.HasValue || !_expiryDate.HasValue)
        {
            Snackbar.Add("Datumi su obavezni", Severity.Warning);
            return;
        }

        await _form!.Validate();

        if (!_form.IsValid)
        {
            Snackbar.Add("Molimo popunite sva obavezna polja", Severity.Warning);
            return;
        }

        try
        {
            _isSubmitting = true;

            Result result;

            if (IsEditMode && ExistingCertificate != null)
            {
                // EDIT
                var updateCommand = new UpdateCertificateCommand(ExistingCertificate.Id, _model);
                result = await Mediator.Send(updateCommand);
            }
            else
            {
                // CREATE
                var createCommand = new CreateCertificateCommand(_model);
                result = await Mediator.Send(createCommand);
            }

            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                Snackbar.Add(result.Message, Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška: {ex.Message}", Severity.Error);
            Console.WriteLine($"[ERROR] Create certificate failed: {ex}");
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    private void Cancel() => MudDialog.Cancel();

    private Task<IEnumerable<(Guid Id, string Name)>> SearchClients(string value, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Task.FromResult<IEnumerable<(Guid Id, string Name)>>(_clients);
        }

        return Task.FromResult(_clients.Where(c =>
            c.Name.Contains(value, StringComparison.OrdinalIgnoreCase)));
    }
}