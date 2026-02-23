using FluentValidation;
using Lumium.Application.Features.Certificates.DTOs;

namespace LumiumPortal.Web.Components.Pages.Certificates.Validators;

public class CreateCertificateDtoValidator : AbstractValidator<CreateCertificateDto>
{
    private const string RequiredFieldMessage = "Ovo polje je obavezno!";

    public CreateCertificateDtoValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage(RequiredFieldMessage);

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

        RuleFor(x => x.IssuedBy)
            .NotEmpty().WithMessage(RequiredFieldMessage);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(
            ValidationContext<CreateCertificateDto>.CreateWithOptions(
                (CreateCertificateDto)model,
                x => x.IncludeProperties(propertyName)));

        return result.IsValid ? [] : result.Errors.Select(e => e.ErrorMessage);
    };
}