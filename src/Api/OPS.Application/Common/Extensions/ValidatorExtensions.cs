using FluentValidation;

namespace OPS.Application.Common.Extensions;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, Guid> IsValidGuid<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Must(guid => guid != Guid.Empty);
    }
}