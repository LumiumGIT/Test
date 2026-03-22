using FluentValidation;
using Lumium.Application.Features.Certificates.DTOs;

namespace LumiumPortal.Web.Components.Pages.Certificates.Validators;

public class CertificateFormDtoValidator : AbstractValidator<CertificateFormDto>
{
    private const string RequiredFieldMessage = "Ovo polje je obavezno!";

    public CertificateFormDtoValidator()
    {
        RuleFor(x => x.SelectedClient)
            .Must(c => c.Id != Guid.Empty)
            .WithMessage(RequiredFieldMessage);

        RuleFor(x => x.CertificateName)
            .NotEmpty().WithMessage(RequiredFieldMessage);

        RuleFor(x => x.CertificateNumber)
            .NotEmpty().WithMessage(RequiredFieldMessage);

        RuleFor(x => x.IssueDate)
            .NotEmpty().WithMessage(RequiredFieldMessage)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Datum izdavanja ne može biti u budućnosti");

        RuleFor(x => x.ExpiryDate)
            .NotEmpty().WithMessage(RequiredFieldMessage)
            .GreaterThan(x => x.IssueDate).WithMessage("Datum isteka mora biti nakon datuma izdavanja");

        RuleFor(x => x.RegulatoryBodyId)
            .NotEmpty().WithMessage(RequiredFieldMessage);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(
            ValidationContext<CertificateFormDto>.CreateWithOptions(
                (CertificateFormDto)model,
                x => x.IncludeProperties(propertyName)));

        return result.IsValid ? [] : result.Errors.Select(e => e.ErrorMessage);
    };
}