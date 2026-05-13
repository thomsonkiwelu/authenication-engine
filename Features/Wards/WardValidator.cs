using FluentValidation;

namespace conservation_backend.Features.Wards;

public class WardValidator : AbstractValidator<WardRequest>
{
    public WardValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Name cannot exceed 50 characters.");

        RuleFor(x => x.DistrictId)
            .NotEmpty()
            .WithMessage("Name cannot be empty.");
    }
}