using Domain.Enums.Contracts;
using Lumium.Application.Features.Clients.DTOs;
using Lumium.Application.Features.Clients.Queries;
using Lumium.Application.Features.Contracts.Commands;
using Lumium.Application.Features.Contracts.DTOs;
using LumiumPortal.Web.Components.Pages.Contracts.Validators;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Contracts;

public partial class AddContractDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private CreateContractDto _model = new();
    private MudForm? _form;
    private readonly CreateContractDtoValidator _validator = new();
    private bool _isSubmitting;

    private List<ClientDto> _clients = new();
    private DateTime? _startDate = DateTime.Today;
    private DateTime? _endDate = DateTime.Today.AddYears(1);

    protected override async Task OnInitializedAsync()
    {
        _model = new CreateContractDto
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddYears(1),
            Status = ContractStatus.Active,
            Type = ContractType.Recurring,
            Duration = ContractDuration.Fixed
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

    private async Task HandleSubmit()
    {
        if (_form == null) return;

        await _form.Validate();

        if (!_form.IsValid)
        {
            Snackbar.Add("Molimo popunite sva obavezna polja", Severity.Warning);
            return;
        }

        _isSubmitting = true;

        try
        {
            if (_startDate.HasValue)
            {
                _model.StartDate = _startDate.Value;
            }

            _model.EndDate = _model.Duration switch
            {
                ContractDuration.Fixed when _endDate.HasValue => _endDate.Value,
                ContractDuration.Indefinite => null,
                _ => _model.EndDate
            };

            var result = await Mediator.Send(new CreateContractCommand(_model));

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
            Console.WriteLine($"[ERROR] Create contract failed: {ex}");
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    private void Cancel() => MudDialog.Cancel();
}