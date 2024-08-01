using FluentValidation;
using FluentValidation.Results;
namespace trade.API.Validation;
public interface IGenericValidator
{
    Task<ValidationResult> ValidateAsync<T>(T request);
}


public class GenericValidator : IGenericValidator
{
    private readonly IServiceProvider _serviceProvider;

    public GenericValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ValidationResult> ValidateAsync<T>(T request)
    {
        var validator = _serviceProvider.GetService<IValidator<T>>();
        if (validator == null)
        {
            throw new InvalidOperationException($"No validator found for type {typeof(T).Name}");
        }

        return await validator.ValidateAsync(request);
    }
}