using FluentValidation;

namespace authentication_engine.Features.LessRangerGroups;

public class LessRangerGroupValidator : AbstractValidator<LessRangerGroupRequest>
{
    public LessRangerGroupValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Group name cannot be empty.");

        RuleFor(x => x.LessRangerStationId)
            .NotEmpty()
            .WithMessage("LessRangerStationId cannot be empty.");
    }
}
