using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Interfaces.Services.Identity;
using Shared.Infrastructure.Extensions;
using Shared.Test.Mocks;

namespace Share.Test.Extensions
{
    public static class SharedInfrastructure
    {
        public static IServiceCollection AddsSharedInfrastructureForTestAssemblies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSharedInfrastructure(configuration);
            services.AddScoped<ICurrentUser, CurrentUser>();

            return services;
        }

    }
}