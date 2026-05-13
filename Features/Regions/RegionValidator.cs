using FluentValidation;

namespace conservation_backend.Features.Regions;

public class RegionValidator : AbstractValidator<RegionRequest>
{
    public RegionValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Name cannot exceed 50 characters.");
    }
}