using FluentValidation;
using ShooterPlatform.Api.Application.Features.Players.DTOs;

namespace ShooterPlatform.Api.Application.Features.Players.Validators
{
    public class PlayerQueryRequestValidator : AbstractValidator<PlayerQueryRequest>
    {
        public PlayerQueryRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page must be >= 1");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("PageSize must be between 1 and 100");
        }
    }
}
