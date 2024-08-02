using FluentValidation;
using trade.Logic.Request;

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {
        RuleFor(category => category.CategoryName)
            .NotEmpty().WithMessage("Category name is required.");
    }
}