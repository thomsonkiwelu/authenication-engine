using FluentValidation;

namespace conservation_backend.Features.Locations;

public class LocationValidator : AbstractValidator<LocationRequest>
{
    public LocationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.");
        
        RuleFor(x => x.ParkId)
            .NotEmpty()
            .WithMessage("ParkId cannot be empty.");
    }
}