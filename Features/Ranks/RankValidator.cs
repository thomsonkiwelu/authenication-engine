using FluentValidation;

namespace authentication_engine.Features.Ranks
{
    public class RankValidator : AbstractValidator<RankRequest>
    {
        public RankValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.");
        }
    }
}
