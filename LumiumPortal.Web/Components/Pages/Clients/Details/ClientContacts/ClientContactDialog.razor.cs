using Domain.Enums.Clients;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.ClientContacts.Commands;
using Lumium.Application.Features.ClientContacts.DTOs;
using LumiumPortal.Web.Components.Pages.Clients.Details.ClientContacts.Validators;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients.Details.ClientContacts;

public partial class ClientContactDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    
    [Parameter] public Guid ClientId { get; set; }
    [Parameter] public ClientContactDto? ExistingContact { get; set; }
    [Parameter] public bool IsEditMode { get; set; }

    private MudForm? _form;
    private readonly ClientContactFormDtoValidator _validator = new();
    private ClientContactFormDto _model = new();
    private bool _isSubmitting;

    protected override void OnInitialized()
    {
        if (IsEditMode && ExistingContact != null)
        {
            _model = new ClientContactFormDto
            {
                ClientId = ExistingContact.ClientId,
                Name = ExistingContact.Name,
                Phone = ExistingContact.Phone,
                Email = ExistingContact.Email,
                Description = ExistingContact.Description,
                Type = ExistingContact.Type
            };
        }
        else
        {
            _model = new ClientContactFormDto
            {
                ClientId = ClientId,
                Type = ContactType.Secondary
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

            if (IsEditMode && ExistingContact != null)
            {
                var command = new UpdateClientContactCommand(ExistingContact.Id, _model);
                result = await Mediator.Send(command);
            }
            else
            {
                var command = new CreateClientContactCommand(_model);
                result = await Mediator.Send(command);
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
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    private void Cancel() => MudDialog.Cancel();
}