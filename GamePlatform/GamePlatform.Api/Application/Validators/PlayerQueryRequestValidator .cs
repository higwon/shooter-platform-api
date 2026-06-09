using FluentValidation;
using GamePlatform.Api.Application.DTOs;

namespace GamePlatform.Api.Application.Validators
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
