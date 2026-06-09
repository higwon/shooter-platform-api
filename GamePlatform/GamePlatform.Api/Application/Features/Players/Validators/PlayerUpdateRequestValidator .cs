using FluentValidation;
using GamePlatform.Api.Application.Features.Players.DTOs;

namespace GamePlatform.Api.Application.Features.Players.Validators
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
