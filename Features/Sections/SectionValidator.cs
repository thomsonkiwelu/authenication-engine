using FluentValidation;

namespace authentication_engine.Features.Sections;

public class SectionValidator: AbstractValidator<SectionRequest>
{
    public SectionValidator()
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
        
        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithMessage("Department cannot be empty.");
    }
}