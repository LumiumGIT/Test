using Lumium.Application.Features.Certificates.Commands;
using Lumium.Application.Features.Certificates.DTOs;
using Lumium.Application.Features.Clients.DTOs;
using Lumium.Application.Features.Clients.Queries;
using LumiumPortal.Web.Components.Pages.Certificates.Validators;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Certificates;

public partial class AddCertificateDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    
    private List<ClientDto> _clients = [];
    private CertificateDto _model = new();
    private MudForm? _form;
    private readonly CreateCertificateDtoValidator _validator = new();
    private bool _isSubmitting;

    private DateTime? _issueDate = DateTime.Today;
    private DateTime? _expiryDate = DateTime.Today.AddYears(1);

    protected override async Task OnInitializedAsync()
    {
        _model = new CertificateDto
        {
            ClientId = Guid.Empty,
            IssueDate = DateTime.Today,
            ExpiryDate = DateTime.Today.AddYears(1)
        };
        
        await LoadClients();
    }
    
    private async Task LoadClients()
    {
        try
        {
            _clients = await Mediator.Send(new GetClientsQuery());
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška pri učitavanju klijenata: {ex.Message}", Severity.Error);
            Console.WriteLine($"[ERROR] Load clients failed: {ex}");
        }
    }

    protected override void OnParametersSet()
    {
        if (_issueDate.HasValue)
            _model.IssueDate = _issueDate.Value;

        if (_expiryDate.HasValue)
            _model.ExpiryDate = _expiryDate.Value;
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

            var command = new CreateCertificateCommand(_model);
            var result = await Mediator.Send(command);

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

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}