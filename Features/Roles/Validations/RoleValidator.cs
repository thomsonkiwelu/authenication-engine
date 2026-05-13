using FluentValidation;

namespace conservation_backend.Features.Roles.Validations
{
    public class RoleValidator: AbstractValidator<RoleRequest>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.");
        }
    }
}
