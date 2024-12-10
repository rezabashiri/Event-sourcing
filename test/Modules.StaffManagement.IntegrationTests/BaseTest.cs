using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.EmployeeManagement.Extensions;
using Share.Test.Extensions;
using Shared.Test.Extensions;

namespace Modules.EmployeeManagement.IntegrationTests;

public abstract class BaseTest
{
    protected BaseTest()
    {
        ServiceProvider = MakeServiceProvider(new ServiceCollection());
    }
    protected IMediator Mediator => ServiceProvider.GetService<IMediator>()!;

    private IServiceProvider ServiceProvider { get; }

    private IServiceProvider MakeServiceProvider(IServiceCollection services)
    {
        var configuration = BuildConfiguration();
        services.AddSingleton(configuration);
        services.AddSharedCoreServicesForTestAssemblies(configuration);
        services.AddsSharedInfrastructureForTestAssemblies(configuration);
        services.AddEmployeeManagementModule(configuration);

        return services.BuildServiceProvider();
    }

    private IConfiguration BuildConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.GetFullPath("../../../../../src/server/API/appsettings.json"))
            .AddInMemoryCollection(new List<KeyValuePair<string, string>>()
            {
                new("PersistenceSettings:UseMsSql", "false"),
                new("PersistenceSettings:UseInMemory", "true")
            });

        return configuration.Build();
    }
}