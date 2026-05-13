using FluentValidation;

namespace conservation_backend.Features.Species
{
    public class SpeciesValidator: AbstractValidator<SpeciesRequest>
    {
        public SpeciesValidator() {
            RuleFor(x => x.ScientificName)
                .NotEmpty()
                .WithMessage("Scientific name cannot be empty.");

            RuleFor(x => x.CommonName)
                .NotEmpty()
                .WithMessage("Common name cannot be empty.");

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Type cannot be empty.");
        }
    }
}
