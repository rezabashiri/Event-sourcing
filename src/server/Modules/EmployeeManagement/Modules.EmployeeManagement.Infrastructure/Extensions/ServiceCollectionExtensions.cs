using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.EmployeeManagement.Core.Abstractions;
using Modules.EmployeeManagement.Infrastructure.Persistence;
using Shared.Infrastructure.Persistence;

namespace Modules.EmployeeManagement.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmployeeManagementInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddDatabaseContext<EmployeeManagementDbContext>(configuration);
            services.AddScoped<IStaffManagementDbContext>(provider => provider.GetService<EmployeeManagementDbContext>());
            return services;
        }

    }
}