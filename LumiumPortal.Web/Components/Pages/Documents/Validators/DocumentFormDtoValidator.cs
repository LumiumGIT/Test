using FluentValidation;
using Lumium.Application.Features.Documents.DTOs;

namespace LumiumPortal.Web.Components.Pages.Documents.Validators;

public class DocumentFormDtoValidator : AbstractValidator<DocumentFormDto>
{
    public DocumentFormDtoValidator()
    {
        RuleFor(x => x.SelectedClient)
            .Must(c => c.Id != Guid.Empty)
            .WithMessage("Klijent je obavezan");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Naziv dokumenta je obavezan")
            .MaximumLength(200).WithMessage("Naziv ne sme biti duži od 200 karaktera");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Link ka dokumentu je obavezan")
            .MaximumLength(500).WithMessage("Link ne sme biti duži od 500 karaktera")
            .Must(BeValidUrl).WithMessage("Link mora biti validna URL adresa");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Opis ne sme biti duži od 1000 karaktera");
    }

    private bool BeValidUrl(string url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(
            ValidationContext<DocumentFormDto>.CreateWithOptions(
                (DocumentFormDto)model,
                x => x.IncludeProperties(propertyName)));

        return result.IsValid ? [] : result.Errors.Select(e => e.ErrorMessage);
    };
}