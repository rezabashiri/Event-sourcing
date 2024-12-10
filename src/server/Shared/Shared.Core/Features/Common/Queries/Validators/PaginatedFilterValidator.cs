using FluentValidation;
using Microsoft.Extensions.Localization;
using Shared.Core.Contracts;
using Shared.Core.Interfaces;
using Shared.DTOs.Filters;

namespace Shared.Core.Features.Common.Queries.Validators
{
    public abstract class PaginatedFilterValidator<TEntityId, TEntity, TFilter> :
        AbstractValidator<TFilter>,
        IPaginatedFilterValidator<TEntityId, TEntity, TFilter>
            where TEntity : class, IEntity<TEntityId>
            where TFilter : PaginatedFilter
    {
        protected PaginatedFilterValidator(IStringLocalizer localizer)
        {
            IPaginatedFilterValidator<TEntityId, TEntity, TFilter>.UseRules(this, localizer);
        }
    }
}