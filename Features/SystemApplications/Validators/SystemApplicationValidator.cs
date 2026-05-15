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
            
            RuleFor(x => x.Url)
                .NotEmpty()
                .WithMessage("Url cannot be empty.");
            
            RuleFor(x => x.ApiKey)
                .NotEmpty()
                .WithMessage("ApiKey cannot be empty.");
        }
    }
}
