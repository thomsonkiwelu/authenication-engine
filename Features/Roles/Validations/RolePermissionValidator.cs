using FluentValidation;

namespace conservation_backend.Features.Roles.Validations
{
    public class RolePermissionValidator : AbstractValidator<AssignRolePermissionRequest>
    { 
        public RolePermissionValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty()
                .WithMessage("Role Id cannot be empty.");

            RuleFor(x => x.PermissionIds)
                .NotEmpty()
                .WithMessage("At least one permission must be selected.")
                .Must(x => x.Count > 0)
                .WithMessage("Permission list cannot be empty.");
        }
        
    }
}
