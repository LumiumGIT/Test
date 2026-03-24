using Domain.Enums.Documents;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Clients.Queries;
using Lumium.Application.Features.Documents.Commands;
using Lumium.Application.Features.Documents.DTOs;
using LumiumPortal.Web.Components.Pages.Documents.Validators;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Documents;

public partial class DocumentDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public Guid ClientId { get; set; }
    [Parameter] public DocumentDto? ExistingDocument { get; set; }
    [Parameter] public bool IsEditMode { get; set; }

    private DocumentFormDto _model = new();
    private MudForm? _form;
    private readonly DocumentFormDtoValidator _validator = new();
    private bool _isSubmitting;
    private List<(Guid Id, string Name)> _clients = [];
    private bool DisableClientSelection => ClientId != Guid.Empty || IsEditMode;

    protected override async Task OnInitializedAsync()
    {
        await LoadClients();

        if (IsEditMode && ExistingDocument != null)
        {
            _model = new DocumentFormDto
            {
                Name = ExistingDocument.Name,
                Category = ExistingDocument.Category,
                Url = ExistingDocument.Url,
                Description = ExistingDocument.Description,
                SelectedClient = _clients.FirstOrDefault(c => c.Id == ExistingDocument.ClientId)
            };
        }
        else
        {
            _model = new DocumentFormDto
            {
                Category = DocumentCategory.Other
            };

            PreselectClient();
        }
    }

    private async Task LoadClients()
    {
        try
        {
            _clients = await Mediator.Send(new GetClientIdsQuery());
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška pri učitavanju klijenata: {ex.Message}", Severity.Error);
            Console.WriteLine($"[ERROR] Load clients failed: {ex}");
        }
    }

    private void PreselectClient()
    {
        if (ClientId != Guid.Empty)
        {
            _model.SelectedClient = _clients.FirstOrDefault(c => c.Id == ClientId);
        }
    }

    private async Task HandleSubmit()
    {
        if (_form == null)
        {
            return;
        }

        await _form.Validate();

        if (!_form.IsValid)
        {
            Snackbar.Add("Molimo popunite sva obavezna polja", Severity.Warning);
            return;
        }

        _isSubmitting = true;

        try
        {
            Result result;

            if (IsEditMode && ExistingDocument != null)
            {
                var updateCommand = new UpdateDocumentCommand(ExistingDocument.Id, _model);
                result = await Mediator.Send(updateCommand);
            }
            else
            {
                var createCommand = new CreateDocumentCommand(_model);
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
            Console.WriteLine($"[ERROR] Create document failed: {ex}");
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    private void Cancel() => MudDialog.Cancel();

    private Task<IEnumerable<(Guid Id, string Name)>> SearchClients(string value, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Task.FromResult<IEnumerable<(Guid Id, string Name)>>(_clients);
        }

        return Task.FromResult(_clients.Where(c =>
            c.Name.Contains(value, StringComparison.OrdinalIgnoreCase)));
    }
}