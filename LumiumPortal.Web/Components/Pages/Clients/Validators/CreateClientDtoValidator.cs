using FluentValidation;
using Lumium.Application.Features.Clients.DTOs;

namespace LumiumPortal.Web.Components.Pages.Clients.Validators;

public class CreateClientDtoValidator : AbstractValidator<ClientDto>
{
    private const string RequiredFieldMessage = "Ovo polje je obavezno!";
    
    public CreateClientDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(RequiredFieldMessage);

        RuleFor(x => x.LegalForm)
            .NotEmpty().WithMessage(RequiredFieldMessage);

        RuleFor(x => x.TaxNumber)
            .NotEmpty().WithMessage(RequiredFieldMessage);

        RuleFor(x => x.TaxIdentificationNumber)
            .NotEmpty().WithMessage(RequiredFieldMessage);

        RuleFor(x => x.ResponsiblePerson)
            .NotEmpty().WithMessage(RequiredFieldMessage);

        RuleFor(x => x.BackupPerson)
            .NotEmpty().WithMessage(RequiredFieldMessage);

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage(RequiredFieldMessage);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(RequiredFieldMessage);

        RuleFor(x => x.Director)
            .NotEmpty().WithMessage(RequiredFieldMessage);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(RequiredFieldMessage)
            .EmailAddress().WithMessage("Email adresa nije validna");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage(RequiredFieldMessage);
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