using FluentValidation;

namespace conservation_backend.Features.GovernmentLeaders;

public class GovernmentLeaderValidator: AbstractValidator<GovernmentLeaderRequestDto>
{
    public GovernmentLeaderValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Full name cannot exceed 50 characters.");

        RuleFor(x => x.Position)
            .NotEmpty()
            .WithMessage("Position cannot be empty.")
            .MaximumLength(100)
            .WithMessage("Position cannot exceed 100 characters.");
        
        RuleFor(x => x.Mobile)
            .NotEmpty()
            .WithMessage("Mobile cannot be empty.")
            .MaximumLength(15)
            .WithMessage("Mobile cannot exceed 15 characters.");
        
        RuleFor(x => x.TelephoneNumber)
            .MaximumLength(20)
            .WithMessage("Telephone number cannot exceed 15 characters.");
        
        RuleFor(x => x.Address)
            .MaximumLength(20)
            .WithMessage("Address cannot exceed 15 characters.");
    }
}