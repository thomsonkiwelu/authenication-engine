using FluentValidation;

namespace authentication_engine.Features.Staffs;

public class StaffValidator: AbstractValidator<StaffRequest>
{
    public StaffValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name cannot be empty.");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name cannot be empty.");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty.");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number cannot be empty.");
        
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status cannot be empty.");
    }
}