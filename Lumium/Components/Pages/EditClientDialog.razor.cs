using Lumium.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Lumium.Components.Pages;

public partial class EditClientDialog : ComponentBase
{
    [CascadingParameter] 
    private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public Client Client { get; set; } = null!;

    private Client _clientCopy = new();
    private MudForm _form = null!;
    private bool _isValid;

    protected override void OnInitialized()
    {
        // Kreiraj kopiju da ne menjamo original dok korisnik ne klikne Save
        _clientCopy = new Client
        {
            Id = Client.Id,
            Name = Client.Name,
            Email = Client.Email
        };
    }

    private void Submit()
    {
        if (_isValid)
        {
            MudDialog.Close(DialogResult.Ok(_clientCopy));
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}