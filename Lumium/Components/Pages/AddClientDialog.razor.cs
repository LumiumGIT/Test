using Lumium.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Lumium.Components.Pages;

public partial class AddClientDialog : ComponentBase
{
    [CascadingParameter] 
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Client _client = new();
    private MudForm _form = null!;
    private bool _isValid;

    private void Submit()
    {
        if (_isValid)
        {
            MudDialog.Close(DialogResult.Ok(_client));
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}