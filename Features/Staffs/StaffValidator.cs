using FluentValidation;

namespace authentication_engine.Features.Staffs;

public class StaffValidator: AbstractValidator<StaffRequest>
{
    public StaffValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("First name cannot exceed 50 characters.");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Last name cannot exceed 50 characters.");
        
        RuleFor(x => x.MiddleName)
            .NotEmpty()
            .WithMessage("Middle name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Middle name cannot exceed 50 characters.");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty.");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number cannot be empty.");
        
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status cannot be empty.");
        
        //RuleFor(x => x.TnpNumber)
          //  .NotEmpty()
           // .WithMessage("TNP number cannot be empty.");
    }
}