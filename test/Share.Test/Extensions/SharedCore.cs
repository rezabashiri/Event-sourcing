using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Extensions;

namespace Shared.Test.Extensions
{
    public static class SharedCore
    {
        public static IServiceCollection AddSharedCoreServicesForTestAssemblies(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSerialization(configuration);
            services.AddSharedApplication(configuration);

            return services;
        }
    }
}