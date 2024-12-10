using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Accounting.Application.Extensions;

namespace Modules.Accounting.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAccountingApplication(configuration);

        return services;
    }
}