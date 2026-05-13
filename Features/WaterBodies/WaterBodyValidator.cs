using FluentValidation;

namespace conservation_backend.Features.WaterBodies;

public class WaterBodyValidator: AbstractValidator<WaterBodyRequest>
{
    public WaterBodyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.");
        
        RuleFor(x => x.ParkId)
            .NotEmpty()
            .WithMessage("Park name cannot be empty.");
        
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Type cannot be empty.");
    }
}