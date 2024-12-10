using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Accounting.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountingCore(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}