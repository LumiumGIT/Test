using FluentValidation;
using Lumium.Application.Features.ClientContacts.DTOs;

namespace LumiumPortal.Web.Components.Pages.Clients.Details.ClientContacts.Validators;

public class ClientContactFormDtoValidator : AbstractValidator<ClientContactFormDto>
{
    public ClientContactFormDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ime i prezime je obavezno.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Telefon je obavezan.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email je obavezan.")
            .EmailAddress().WithMessage("Email adresa nije ispravna.");
    }
    
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(
            ValidationContext<ClientContactFormDto>.CreateWithOptions(
                (ClientContactFormDto)model,
                x => x.IncludeProperties(propertyName)));

        return result.IsValid ? [] : result.Errors.Select(e => e.ErrorMessage);
    };
}