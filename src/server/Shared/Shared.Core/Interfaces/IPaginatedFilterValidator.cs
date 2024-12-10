using FluentValidation;
using Microsoft.Extensions.Localization;
using Shared.Core.Contracts;
using Shared.Core.Extensions;
using Shared.DTOs.Filters;

namespace Shared.Core.Interfaces
{
    internal interface IPaginatedFilterValidator<TEntityId, TEntity, TFilter>
        where TEntity : class, IEntity<TEntityId>
        where TFilter : PaginatedFilter
    {
        static void UseRules(AbstractValidator<TFilter> validator, IStringLocalizer localizer)
        {
            validator.RuleFor(request => request.PageNumber)
                .GreaterThan(0).WithMessage(localizer["The {PropertyName} property must be greater than 0."]);
            validator.RuleFor(request => request.PageSize)
                .GreaterThan(0).WithMessage(localizer["The {PropertyName} property must be greater than 0."]);
            validator.RuleFor(request => request.OrderBy)
                .MustContainCorrectOrderingsFor(typeof(TEntity), localizer);
        }
    }
}