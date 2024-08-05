using FluentValidation;
using trade.Logic.Request;
using trade.Logic.Requests;

public class CreateStoreValidator : AbstractValidator<CreateStoreRequest>
{
    public CreateStoreValidator()
    {
        RuleFor(category => category.Name)
            .NotEmpty().WithMessage("Category name is required.");

        RuleFor(category => category.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(category => category.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");
    }
}