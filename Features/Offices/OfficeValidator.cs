using FluentValidation;

namespace authentication_engine.Features.Offices;

public class OfficeValidator: AbstractValidator<OfficeRequest>
{
    public OfficeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.");

        RuleFor(x => x.StructureId)
            .NotEmpty()
            .WithMessage("Structure cannot be empty.");
    }
}