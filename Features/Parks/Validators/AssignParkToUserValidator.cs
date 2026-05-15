using FluentValidation;

namespace authentication_engine.Features.Parks.Validators;

public class AssignParkToUserValidator: AbstractValidator<AssignParkToUserRequest>
{
    public AssignParkToUserValidator()
    {
        RuleFor<object>(x => x.ParkId)
            .NotEmpty()
            .WithMessage("Park ID cannot be empty.");
            
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID cannot be empty.");
    }
}