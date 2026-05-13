using FluentValidation;

namespace conservation_backend.Features.LessRangerStations;

public class LessRangerStationValidator : AbstractValidator<LessRangerStationRequest>
{
    public LessRangerStationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Station name cannot be empty.");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.LessOperationalZoneId) || !string.IsNullOrWhiteSpace(x.OfficeId))
            .WithMessage("LessOperationalZoneId or OfficeId must be provided.")
            .Must(x => string.IsNullOrWhiteSpace(x.LessOperationalZoneId) || string.IsNullOrWhiteSpace(x.OfficeId))
            .WithMessage("LessOperationalZoneId and OfficeId cannot both be provided.");
    }
}
