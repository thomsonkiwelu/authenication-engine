using FluentValidation;

namespace authentication_engine.Features.SystemApplications
{
    public class SystemApplicationValidator : AbstractValidator<SystemApplicationRequestDto>
    {
        public SystemApplicationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.");
            
            RuleFor(x => x.Slug)
                .NotEmpty()
                .WithMessage("Slug cannot be empty.");
            
            RuleFor(x => x.ApiKey)
                .NotEmpty()
                .WithMessage("ApiKey cannot be empty.");
        }
    }
}
