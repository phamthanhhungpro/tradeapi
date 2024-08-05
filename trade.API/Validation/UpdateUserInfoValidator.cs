using FluentValidation;
using trade.Logic.Request;
using trade.Logic.Requests;

namespace trade.API.Validation
{
    public class UpdateUserInfoValidator : AbstractValidator<UpdateUserInfoRequest>
    {
        public UpdateUserInfoValidator()
        {
            RuleFor(category => category.Name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}
