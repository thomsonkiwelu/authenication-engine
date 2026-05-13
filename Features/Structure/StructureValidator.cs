using FluentValidation;

namespace conservation_backend.Features.Structure
{
    public class StructureValidator : AbstractValidator<StructureRequest>
    {
        public StructureValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.");
        }
    }
}
