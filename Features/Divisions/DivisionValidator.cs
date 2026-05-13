using FluentValidation;

namespace conservation_backend.Features.Divisions;

public class DivisionValidator : AbstractValidator<DivisionRequest>
{
    public DivisionValidator()
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