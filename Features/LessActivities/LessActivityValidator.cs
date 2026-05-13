using FluentValidation;

namespace conservation_backend.Features.LessActivities;

public class LessActivityValidator : AbstractValidator<LessActivityRequest>
{
    public LessActivityValidator()
    {
        RuleFor(x => x.Label)
            .NotEmpty()
            .WithMessage("Label cannot be empty.");
        
        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category cannot be empty.");
        
        RuleFor(x => x.Key)
            .NotEmpty()
            .WithMessage("Key cannot be empty.");
    }
}