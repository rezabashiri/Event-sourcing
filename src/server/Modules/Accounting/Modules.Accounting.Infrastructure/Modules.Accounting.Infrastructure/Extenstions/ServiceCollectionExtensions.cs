using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Accounting.Domain.Abstractions;
using Modules.Accounting.Infrastructure.Persistence;
using Shared.Infrastructure.Persistence;

namespace Modules.Accounting.Infrastructure.Extenstions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountingInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabaseContext<AccountingDbContext>(configuration);

        services.AddScoped<IAccountingDbContext>(provider => provider.GetService<AccountingDbContext>()!);

        return services;
    }
}