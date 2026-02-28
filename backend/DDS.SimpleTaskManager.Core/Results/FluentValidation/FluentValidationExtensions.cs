using DDS.SimpleTaskManager.Core.Results.Errors;

using FluentValidation;
using FluentValidation.Results;

namespace DDS.SimpleTaskManager.Core.Results.FluentValidation;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule,
        IError error)
    {
        return rule
            .WithMessage(error.Message)
            .WithErrorCode(error.Code)
            .WithState(_ => error);
    }

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
    this IRuleBuilderOptions<T, TProperty> rule,
    Func<T, IError> errorFactory)
    {
        return rule
            .WithMessage((instance, _) => errorFactory(instance).Message)
            .WithState(instance => errorFactory(instance));
    }

    public static IReadOnlyList<IError> ToErrors(this ValidationResult validationResult)
    {
        return validationResult.Errors
            .Select(f =>
            {
                if (f.CustomState is Error domainError)
                    return domainError;

                return new Error(
                    f.ErrorCode,
                    f.ErrorMessage,
                    ErrorType.Validation);
            })
            .ToList();
    }
}