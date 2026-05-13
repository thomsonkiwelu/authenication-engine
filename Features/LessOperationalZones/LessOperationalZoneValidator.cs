using FluentValidation;

namespace conservation_backend.Features.LessOperationalZones;

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
