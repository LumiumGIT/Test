using Domain.Enums.Clients;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Clients.Commands;
using Lumium.Application.Features.Clients.DTOs;
using LumiumPortal.Web.Components.Pages.Clients.Validators;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients;

public partial class ClientDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    
    [Parameter] public ClientDto? ExistingClient { get; set; }
    [Parameter] public bool IsEditMode { get; set; }

    private ClientFormDto _model = new();
    private MudForm? _form;
    private readonly ClientFormDtoValidator _validator = new();
    private bool _isSubmitting;

    protected override void OnInitialized()
    {
        if (IsEditMode && ExistingClient != null)
        {
            _model = new ClientFormDto
            {
                Name = ExistingClient.Name,
                LegalForm = ExistingClient.LegalForm,
                TaxNumber = ExistingClient.TaxNumber,
                TaxIdentificationNumber = ExistingClient.TaxIdentificationNumber,
                IsPdv = ExistingClient.IsPdv,
                ResponsiblePerson = ExistingClient.ResponsiblePerson,
                BackupPerson = ExistingClient.BackupPerson,
                Address = ExistingClient.Address,
                PhoneNumber = ExistingClient.PhoneNumber,
                Director = ExistingClient.Director,
                Email = ExistingClient.Email,
                Country = ExistingClient.Country,
                EcoTax = ExistingClient.EcoTax,
                BeneficialOwners = ExistingClient.BeneficialOwners,
                Croso = ExistingClient.Croso,
                Pep = ExistingClient.Pep,
                WingsTemplate = ExistingClient.WingsTemplate,
                IsActive = ExistingClient.IsActive,
                BusinessActivity = ExistingClient.BusinessActivity,
                RiskLevel = ExistingClient.RiskLevel
            };
        }
        else
        {
            _model = new ClientFormDto
            {
                Country = "Srbija",
                RiskLevel = RiskLevel.Low,
                IsActive = true
            };
        }
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

            Result result;

            if (IsEditMode && ExistingClient != null)
            {
                var updateCommand = new UpdateClientCommand(ExistingClient.Id, _model);
                result = await Mediator.Send(updateCommand);
            }
            else
            {
                var createCommand = new CreateClientCommand(_model);
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
            Console.WriteLine($"[ERROR] Create client failed: {ex}");
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    private void Cancel() => MudDialog.Cancel();
}