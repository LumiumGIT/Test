using Domain.Enums.Clients;
using Lumium.Application.Features.ClientContacts.Commands;
using Lumium.Application.Features.ClientContacts.DTOs;
using Lumium.Application.Features.ClientContacts.Queries;
using LumiumPortal.Web.Helpers.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients.Details.ClientContacts;

public partial class ClientContacts : ComponentBase
{
    [Parameter] public Guid ClientId { get; set; }

    [Inject] private IDialogService DialogService { get; set; } = null!;

    private List<ClientContactDto> _contacts = [];
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadContacts();

        await base.OnInitializedAsync();
    }
    
    private async Task LoadContacts()
    {
        _isLoading = true;
        _contacts = await Mediator.Send(new GetClientContactsQuery(ClientId));
        _isLoading = false;
        StateHasChanged();
    }

    private async Task OpenAddDialog()
    {
        var saved = await DialogService.ShowAddContactDialog(ClientId);

        if (saved)
        {
            await LoadContacts();
        }
    }

    private async Task EditContact(ClientContactDto contact)
    {
        var saved = await DialogService.ShowEditContactDialog(contact);
        
        if (saved)
        {
            await LoadContacts();
        }
    }

    private async Task DeleteContact(ClientContactDto contact)
    {
        if (!await DialogService.ShowDeleteContactConfirmation(contact.Name))
        {
            return;
        }

        var result = await Mediator.Send(new DeleteClientContactCommand(contact.Id));

        if (result.IsSuccess)
        {
            Snackbar.Add(result.Message, Severity.Success);
            await LoadContacts();
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }

    private static string GetTypeColor(ContactType type) => type switch
    {
        ContactType.Primary => "#4CAF50",
        _ => "#9E9E9E"
    };
}