using FluentValidation;
using Lumium.Application.Features.Clients.DTOs;

namespace LumiumPortal.Web.Components.Pages.Clients.Validators;

public class CreateClientDtoValidator : AbstractValidator<ClientDto>
{
    public CreateClientDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ovo polje je obavezno!");

        RuleFor(x => x.LegalForm)
            .NotEmpty().WithMessage("Ovo polje je obavezno!");

        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage("Ovo polje je obavezno!");

        RuleFor(x => x.TaxIdentificationNumber)
            .NotEmpty().WithMessage("Ovo polje je obavezno!");

        RuleFor(x => x.ResponsiblePerson)
            .NotEmpty().WithMessage("Ovo polje je obavezno!");

        RuleFor(x => x.BackupPerson)
            .NotEmpty().WithMessage("Ovo polje je obavezno!");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Ovo polje je obavezno!");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Ovo polje je obavezno!");

        RuleFor(x => x.Director)
            .NotEmpty().WithMessage("Ovo polje je obavezno!");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Ovo polje je obavezno!")
            .EmailAddress().WithMessage("Email adresa nije validna");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Ovo polje je obavezno!");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(
            ValidationContext<ClientDto>.CreateWithOptions(
                (ClientDto)model, 
                x => x.IncludeProperties(propertyName)));

        return result.IsValid ? [] : result.Errors.Select(e => e.ErrorMessage);
    };
}