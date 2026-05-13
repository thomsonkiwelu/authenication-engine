using FluentValidation;

namespace authentication_engine.Features.Units;

public class UnitValidator: AbstractValidator<UnitRequest>
{
    public UnitValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.");
        
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code cannot be empty.");

        RuleFor(x => x.OfficeId)
            .NotEmpty()
            .WithMessage("Office cannot be empty.");
    }
}