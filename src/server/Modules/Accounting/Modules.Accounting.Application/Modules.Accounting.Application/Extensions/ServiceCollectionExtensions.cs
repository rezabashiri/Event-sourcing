using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Accounting.Application.Accounts.Services;
using Modules.Accounting.Domain.Extensions;
using Modules.Accounting.Infrastructure.Extenstions;

namespace Modules.Accounting.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountingApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAccountingCore()
            .AddAccountingInfrastructure(configuration)
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddScoped<ITransactionService, TransactionService>()
            .AddMediatR(serviceConfiguration => serviceConfiguration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}