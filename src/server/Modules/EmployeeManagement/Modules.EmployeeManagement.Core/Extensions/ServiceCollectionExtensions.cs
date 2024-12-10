using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.EmployeeManagement.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmployeeManagementCore(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}