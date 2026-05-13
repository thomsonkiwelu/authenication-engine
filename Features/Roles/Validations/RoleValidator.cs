using FluentValidation;

namespace authentication_engine.Features.Roles.Validations
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
