using Lumium.Application.Features.Documents.Commands;
using Lumium.Application.Features.Documents.DTOs;
using Lumium.Application.Features.Documents.Queries;
using LumiumPortal.Web.Helpers.Dialogs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Documents;

public partial class Documents : SecureComponentBase
{
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    private List<DocumentDto> _documents = [];
    private bool _isLoading = true;

    private int OfficialCount => _documents.Count(d => (int)d.Category < 10);
    private int SupportingCount => _documents.Count(d => (int)d.Category >= 10 && (int)d.Category < 99);
    private int RecentCount => _documents.Count(d => d.UploadedAt >= DateTime.Now.AddDays(-7));

    protected override async Task OnSecureInitializedAsync()
    {
        _isLoading = true;
        await LoadDocuments();
        _isLoading = false;
    }

    private async Task LoadDocuments()
    {
        try
        {
            _documents = await Mediator.Send(new GetDocumentsQuery());
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška pri učitavanju dokumenata: {ex.Message}", Severity.Error);
            Console.WriteLine($"[ERROR] Load documents failed: {ex}");
        }
    }

    private async Task AddDocument()
    {
        if (await DialogService.ShowAddDocumentDialog())
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