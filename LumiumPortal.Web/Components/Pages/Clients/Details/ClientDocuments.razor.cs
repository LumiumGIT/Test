using Lumium.Application.Features.Documents.Commands;
using Lumium.Application.Features.Documents.DTOs;
using Lumium.Application.Features.Documents.Queries;
using LumiumPortal.Web.Helpers.Dialogs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients.Details;

public partial class ClientDocuments : ComponentBase
{
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    [Parameter] public Guid ClientId { get; set; }

    private List<DocumentDto> _documents = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadDocuments();

        await base.OnInitializedAsync();
    }

    private async Task LoadDocuments()
    {
        try
        {
            _documents = await Mediator.Send(new GetDocumentsByClientQuery(ClientId));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška pri učitavanju dokumenata: {ex.Message}", Severity.Error);
            Console.WriteLine($"[ERROR] Load contracts failed: {ex}");
        }
    }

    private async Task AddDocument()
    {
        if (await DialogService.ShowAddDocumentDialog(ClientId))
        {
            await LoadDocuments();
        }
    }

    private async Task EditDocument(DocumentDto document)
    {
        if (await DialogService.ShowEditDocumentDialog(document))
        {
            await LoadDocuments();
        }
    }
    
    private async Task DeleteDocument(DocumentDto document)
    {
        if (!await DialogService.ShowDeleteDocumentConfirmation(document.Name))
        {
            return;
        }
        
        var result = await Mediator.Send(new DeleteDocumentCommand(document.Id));

        if (result.IsSuccess)
        {
            Snackbar.Add(result.Message, Severity.Success);
            await LoadDocuments();
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }

    private async Task OpenDocument(string url) => await JsRuntime.InvokeVoidAsync("open", url, "_blank");
}