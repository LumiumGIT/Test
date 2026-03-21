using Lumium.Application.Common.Models;
using Lumium.Application.Features.Documents.Commands;
using Lumium.Application.Features.Documents.DTOs;
using Lumium.Application.Features.Documents.Queries;
using LumiumPortal.Web.Components.Pages.Documents;
using LumiumPortal.Web.Helpers;
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

    private async Task OpenAddDocumentDialog()
    {
        var parameters = new DialogParameters
        {
            { nameof(AddDocumentDialog.ClientId), ClientId }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<AddDocumentDialog>("Dodaj dokument", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadDocuments();
        }
    }

    private async Task DeleteDocument(DocumentDto document)
    {
        var confirmed = await DialogHelpers.ShowConfirmDialog(
            DialogService,
            $"Da li ste sigurni da želite da obrišete dokument '{document.Name}'?",
            "Potvrda brisanja",
            "Obriši",
            Color.Error
        );

        if (confirmed)
        {
            var result = await Mediator.Send(new DeleteDocumentCommand(document.Id));

            await HandleResult(result);
        }
    }

    private async Task HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            await LoadDocuments();
            Snackbar.Add(result.Message, Severity.Success);
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }

    private async Task OpenDocument(string url) => await JsRuntime.InvokeVoidAsync("open", url, "_blank");
}