using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.EmployeeManagement.Core.Extensions;
using Modules.EmployeeManagement.Infrastructure.Extensions;

namespace Modules.EmployeeManagement.Extensions
{
    public static class ModuleExtensions
    {

        public static IServiceCollection AddEmployeeManagementModule(this IServiceCollection services,IConfiguration configuration)
        {

            services.AddEmployeeManagementCore();
            services.AddEmployeeManagementInfrastructure(configuration);

            return services;
        }
    }
}