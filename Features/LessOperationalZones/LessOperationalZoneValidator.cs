using FluentValidation;

namespace authentication_engine.Features.LessOperationalZones;

public class LessOperationalZoneValidator : AbstractValidator<LessOperationalZoneRequest>
{
    public LessOperationalZoneValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Zone name cannot be empty.");

        RuleFor(x => x.ParkId)
            .NotEmpty()
            .WithMessage("ParkId cannot be empty.");
    }
}
