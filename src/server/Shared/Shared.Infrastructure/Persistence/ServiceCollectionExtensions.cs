using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Extensions;
using Shared.Core.Settings;

namespace Shared.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseContext<T>(this IServiceCollection services,
        IConfiguration configuration)
        where T : DbContext
    {
        var options = services.GetOptions<PersistenceSettings>(nameof(PersistenceSettings), configuration);
        if (options.UseInMemory)
        {
            services.AddInMemoryDataBase<T>(Guid.NewGuid().ToString());
        }
        else if (options.UsePostgres)
        {
            string connectionString = options.ConnectionStrings.Postgres;

            services.AddPostgres<T>(connectionString);
        }
        else if (options.UseMsSql)
        {
            string connectionString = options.ConnectionStrings.MSSQL;
            services.AddMSSQL<T>(connectionString);
        }

        return services;
    }

    private static IServiceCollection AddPostgres<T>(this IServiceCollection services, string connectionString)
        where T : DbContext
    {
        services.AddDbContext<T>(m =>
            m.UseNpgsql(connectionString, e => e.MigrationsAssembly(typeof(T).Assembly.FullName)));
        MigrateDataBase<T>(services);
        services.AddHangfire(x => x.UsePostgreSqlStorage(connectionString));
        return services;
    }

    private static IServiceCollection AddMSSQL<T>(this IServiceCollection services, string connectionString)
        where T : DbContext
    {
        services.AddDbContext<T>(m =>
            m.UseSqlServer(connectionString, e => e.MigrationsAssembly(typeof(T).Assembly.FullName)));
        services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
        MigrateDataBase<T>(services);

        return services;
    }

    private static IServiceCollection AddInMemoryDataBase<T>(this IServiceCollection services, string name)
        where T : DbContext
    {
        services.AddDbContext<T>(m => m.UseInMemoryDatabase(name));
        return services;
    }

    private static void MigrateDataBase<T>(IServiceCollection services) where T : DbContext
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();
        dbContext.Database.Migrate();
    }
}