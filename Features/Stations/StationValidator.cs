using FluentValidation;

namespace authentication_engine.Features.Stations;

public class StationValidator: AbstractValidator<StationRequest>
{
    public StationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Station name cannot be empty.");
        
        RuleFor(x => x.ParkId)
            .NotEmpty()
            .WithMessage("ParkId cannot be empty.");
        
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Station type cannot be empty.");
    }
}