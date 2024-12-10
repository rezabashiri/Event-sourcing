using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Accounting.Application.Extensions;
using Modules.Accounting.Domain.Abstractions;
using Share.Test.Extensions;
using Shared.Test.Extensions;

namespace Modules.Accounting.IntegrationTests;

public abstract class BaseTest
{
    protected BaseTest()
    {
        ServiceProvider = BuildServiceProvider(new ServiceCollection());
    }
    protected IAccountingDbContext AccountingDbContext => ServiceProvider.GetService<IAccountingDbContext>()!;
    protected IMediator Mediator => ServiceProvider.GetService<IMediator>()!;

    protected IServiceProvider ServiceProvider { get; }
    protected virtual void OnBuildServiceProvider(IServiceCollection services)
    {

    }
    private IServiceProvider BuildServiceProvider(IServiceCollection services)
    {
        var configuration = BuildConfiguration();
        services.AddSingleton(configuration);
        services.AddSharedCoreServicesForTestAssemblies(configuration);
        services.AddsSharedInfrastructureForTestAssemblies(configuration);
        services.AddAccountingApplication(configuration);
        OnBuildServiceProvider(services);
        return services.BuildServiceProvider();
    }

    private IConfiguration BuildConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.GetFullPath("../../../../../../src/server/API/appsettings.json"))
            .AddInMemoryCollection(new List<KeyValuePair<string, string>>()
            {
                new("PersistenceSettings:UseMsSql", "false"),
                new("PersistenceSettings:UseInMemory", "true")
            });

        return configuration.Build();
    }
}