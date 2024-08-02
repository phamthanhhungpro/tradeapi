using FluentValidation;
using trade.Logic.Request;

namespace trade.API.Validation
{
    public class ProductRequestValidator : AbstractValidator<ProductRequest>
    {
        public ProductRequestValidator()
        {
            RuleFor(product => product.ProductName)
                .NotEmpty().WithMessage("Product name is required.");

            RuleFor(product => product.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.");
        }
    }
}
