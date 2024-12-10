using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Attributes;
using Shared.Core.Constants;
using Shared.Core.Contracts;
using Shared.Core.Queries;

namespace Shared.Core.Features.Common.Filters
{
    public class GetByIdCacheableFilter<TEntityId, TEntity> : ICacheable
        where TEntity : class, IEntity<TEntityId>
    {
        [FromRoute(Name = "id")]
        public TEntityId Id { get; set; }

        [FromQuery]
        public bool BypassCache { get; set; }

        [FromQuery]
        [SwaggerExclude]
        public string CacheKey => CacheKeys.Common.GetEntityByIdCacheKey<TEntityId, TEntity>(Id);

        [FromQuery]
        [RegularExpression(@"^([0-9]{1}|(?:0[0-9]|1[0-9]|2[0-3])+):([0-5]?[0-9])(?::([0-5]?[0-9])(?:.(\d{1,9}))?)?$")]
        public TimeSpan? SlidingExpiration { get; set; }

        public GetByIdCacheableFilter()
        {
        }
    }
}