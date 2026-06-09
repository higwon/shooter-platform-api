using FluentValidation;
using GamePlatform.Api.Application.DTOs;

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