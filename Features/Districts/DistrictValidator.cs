using FluentValidation;

namespace conservation_backend.Features.Districts;

public class DistrictValidator : AbstractValidator<DistrictRequest>
{
    public DistrictValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Name cannot exceed 50 characters.");

        RuleFor(x => x.RegionId)
            .NotEmpty()
            .WithMessage("Name cannot be empty.");
    }
}