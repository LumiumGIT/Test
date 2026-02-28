using Domain.Enums.Contracts;
using FluentValidation;
using Lumium.Application.Features.Contracts.DTOs;

namespace LumiumPortal.Web.Components.Pages.Contracts.Validators;

public class CreateContractDtoValidator : AbstractValidator<CreateContractDto>
{
    public CreateContractDtoValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("Klijent je obavezan");
        
        RuleFor(x => x.ContractNumber)
            .MaximumLength(100).WithMessage("Broj ugovora ne sme biti duži od 100 karaktera");
        
        RuleFor(x => x.MonthlyFee)
            .GreaterThan(0).WithMessage("Mesečna obaveza mora biti veća od 0");
        
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Datum početka je obavezan");
        
        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Komentar ne sme biti duži od 1000 karaktera");
        
        // EndDate is required only if duration is Fixed
        When(x => x.Duration == ContractDuration.Fixed, () =>
        {
            RuleFor(x => x.EndDate)
                .NotNull().WithMessage("Datum završetka je obavezan za vremenski određen ugovor")
                .GreaterThan(x => x.StartDate).WithMessage("Datum završetka mora biti posle datuma početka");
        });
        
        // EndDate must be null if duration is Indefinite
        When(x => x.Duration == ContractDuration.Indefinite, () =>
        {
            RuleFor(x => x.EndDate)
                .Null().WithMessage("Datum završetka ne sme biti setovan za ugovor na neodređeno");
        });
    }
    
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(
            ValidationContext<CreateContractDto>.CreateWithOptions(
                (CreateContractDto)model, 
                x => x.IncludeProperties(propertyName)));

        return result.IsValid ? [] : result.Errors.Select(e => e.ErrorMessage);
    };
}