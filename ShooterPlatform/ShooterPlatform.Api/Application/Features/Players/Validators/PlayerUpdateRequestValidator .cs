using FluentValidation;
using ShooterPlatform.Api.Application.Features.Players.DTOs;

namespace ShooterPlatform.Api.Application.Features.Players.Validators
{
    public class PlayerUpdateRequestValidator : AbstractValidator<PlayerUpdateRequest>
    {
        public PlayerUpdateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(50);

            RuleFor(x => x.Level)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(999);
        }
    }
}
