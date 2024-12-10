using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Shared.Core.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.Infrastructure.Swagger.Filters
{
    public class SwaggerExcludeFilter : IOperationFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context?.MethodInfo == null)
            {
                return;
            }

            var parameters = context.MethodInfo.GetParameters();
            var properties = parameters.SelectMany(x => x.ParameterType.GetProperties());
            var propertiesToRemove = properties
                .Where(p => p.GetCustomAttribute<SwaggerExcludeAttribute>() != null && p.GetCustomAttribute<FromQueryAttribute>() != null)
                .Select(p => p.Name)
                .ToHashSet(StringComparer.InvariantCultureIgnoreCase);

            foreach (var parameter in operation.Parameters.ToList())
            {
                if (propertiesToRemove.Contains(parameter.Name))
                {
                    operation.Parameters.Remove(parameter);
                }
            }
        }
    }
}