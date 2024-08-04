using FluentValidation;
using trade.Logic.Request;

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {
        RuleFor(category => category.Name)
            .NotEmpty().WithMessage("Category name is required.");
    }
}