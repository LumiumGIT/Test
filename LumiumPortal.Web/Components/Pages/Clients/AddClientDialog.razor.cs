using Domain.Enums;
using Domain.Enums.Clients;
using Lumium.Application.Features.Clients.Commands;
using Lumium.Application.Features.Clients.DTOs;
using LumiumPortal.Web.Components.Pages.Clients.Validators;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients;

public partial class AddClientDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private ClientDto _model = new();
    private MudForm? _form;
    private readonly CreateClientDtoValidator _validator = new();
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
        await _form!.Validate();

        if (!_form.IsValid)
        {
            Snackbar.Add("Molimo popunite sva obavezna polja", Severity.Warning);
            return;
        }
        
        try
        {
            _isSubmitting = true;

            var command = new CreateClientCommand(_model);
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