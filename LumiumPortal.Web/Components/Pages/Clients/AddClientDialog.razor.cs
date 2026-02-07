using Domain.Enums;
using Lumium.Application.Features.Clients.Commands;
using Lumium.Application.Features.Clients.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients;

public partial class AddClientDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private ClientDto _model = new();
    private bool _isSubmitting;

    protected override void OnInitialized()
    {
        _model = new ClientDto
        {
            Country = "Srbija",
            RiskLevel = RiskLevel.Low,
            IsActive = true
        };
    }

    private async Task HandleSubmit()
    {
        try
        {
            _isSubmitting = true;

            var success = await Mediator.Send(new CreateClientCommand(_model));

            if (success)
            {
                Snackbar.Add($"Klijent '{_model.Name}' je uspešno dodat!", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                Snackbar.Add("Greška pri dodavanju klijenta.", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška: {ex.Message}", Severity.Error);
            Console.WriteLine($"[ERROR] Create client failed: {ex}");
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