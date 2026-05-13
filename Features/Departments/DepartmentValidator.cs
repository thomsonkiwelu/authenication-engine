using FluentValidation;

namespace conservation_backend.Features.Departments;

public class DepartmentValidator: AbstractValidator<DepartmentRequest>
{   
    public DepartmentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.");
        
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code cannot be empty.");

        RuleFor(x => x.OfficeId)
            .NotEmpty()
            .WithMessage("Office cannot be empty.");
    }
    
}