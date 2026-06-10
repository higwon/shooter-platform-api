using FluentValidation;
using ShooterPlatform.Api.Application.Features.Players.DTOs;

namespace ShooterPlatform.Api.Application.Features.Players.Validators
{
    public class PlayerCreateRequestValidator : AbstractValidator<PlayerCreateRequest>
    {
        public PlayerCreateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name max length is 50");

            RuleFor(x => x.Level)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Level must be >= 1")
                .LessThanOrEqualTo(999)
                .WithMessage("Level must be <= 999");
        }
    }
}
