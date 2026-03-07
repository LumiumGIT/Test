using Domain.Enums.Documents;
using Lumium.Application.Features.Clients.DTOs;
using Lumium.Application.Features.Clients.Queries;
using Lumium.Application.Features.Documents.Commands;
using Lumium.Application.Features.Documents.DTOs;
using LumiumPortal.Web.Components.Pages.Documents.Validators;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Documents;

public partial class AddDocumentDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private CreateDocumentDto _model = new();
    private MudForm? _form;
    private readonly CreateDocumentDtoValidator _validator = new();
    private bool _isSubmitting;
    private List<(Guid Id, string Name)> _clients = [];

    protected override async Task OnInitializedAsync()
    {
        _model = new CreateDocumentDto
        {
            Category = DocumentCategory.Other
        };

        await LoadClients();
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

    private async Task HandleSubmit()
    {
        if (_form == null) return;

        await _form.Validate();

        if (!_form.IsValid)
        {
            Snackbar.Add("Molimo popunite sva obavezna polja", Severity.Warning);
            return;
        }

        _isSubmitting = true;

        try
        {
            var result = await Mediator.Send(new CreateDocumentCommand(_model));

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
            return Task.FromResult<IEnumerable<(Guid Id, string Name)>>(_clients);

        return Task.FromResult(_clients.Where(c => 
            c.Name.Contains(value, StringComparison.OrdinalIgnoreCase)));
    }
}