using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Test.Extensions;

namespace Modules.Accounting.UnitTests;

public abstract class BaseTest
{
    protected BaseTest()
    {
        ServiceProvider = MakeServiceProvider(new ServiceCollection());
    }

    protected IServiceProvider ServiceProvider { get; }

    private IServiceProvider MakeServiceProvider(IServiceCollection services)
    {
        var configuration = BuildConfiguration();
        services.AddSingleton(configuration);
        services.AddSharedCoreServicesForTestAssemblies(configuration);

        return services.BuildServiceProvider();
    }

    private IConfiguration BuildConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.GetFullPath("../../../../../../src/server/API/appsettings.json"));

        return configuration.Build();
    }
}