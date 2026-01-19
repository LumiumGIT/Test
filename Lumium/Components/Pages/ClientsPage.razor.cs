using Lumium.Context;
using Lumium.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace Lumium.Components.Pages;

public partial class ClientsPage(ApplicationDbContext db, IDialogService dialogService) : ComponentBase
{
    private string _searchString = "";
    private List<Client> Clients { get; set; } = [];
    
    protected override async Task OnInitializedAsync()
    {
        Clients = await db.Clients.ToListAsync();
        await base.OnInitializedAsync();
    }
    
    private IEnumerable<Client> FilteredClients => Clients.Where(c =>
        string.IsNullOrWhiteSpace(_searchString) ||
        c.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
        c.Email.Contains(_searchString, StringComparison.OrdinalIgnoreCase));

    private async Task OpenAddClientDialog()
    {
        var parameters = new DialogParameters<AddClientDialog>();
        var options = new DialogOptions 
        { 
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var dialog = await dialogService.ShowAsync<AddClientDialog>("Add New Client", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false, Data: Client newClient })
        {
            db.Clients.Add(newClient);
            await db.SaveChangesAsync();
            Clients.Add(newClient);
            StateHasChanged();
        }
    }
    
    private async Task EditClient(Client client)
    {
        var parameters = new DialogParameters<EditClientDialog>
        {
            { x => x.Client, client }
        };
        
        var options = new DialogOptions 
        { 
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var dialog = await dialogService.ShowAsync<EditClientDialog>("Edit Client", parameters, options);
        var result = await dialog.Result;

        if (result is not { Canceled: true } && result?.Data is Client editedClient)
        {
            // Pronađi tracked entitet u bazi
            var trackedClient = await db.Clients.FindAsync(client.Id);
        
            if (trackedClient != null)
            {
                // Ažuriraj vrednosti tracked entiteta
                trackedClient.Name = editedClient.Name;
                trackedClient.Email = editedClient.Email;
            
                await db.SaveChangesAsync();
            
                // Ažuriraj u listi
                var index = Clients.FindIndex(c => c.Id == editedClient.Id);
                if (index != -1)
                {
                    Clients[index].Name = editedClient.Name;
                    Clients[index].Email = editedClient.Email;
                }
            
                StateHasChanged();
            }
        }
    }

    private async Task DeleteClient(Client client)
    {
        Clients.Remove(client);
        db.Clients.Remove(client);
        await db.SaveChangesAsync();
    }
}