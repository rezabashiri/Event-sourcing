using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shared.Core.Settings;

namespace Modules.Accounting.Infrastructure.Persistence
{
    public class DesignTimeAccountingDbContextFactory : IDesignTimeDbContextFactory<AccountingDbContext>
    {

        public AccountingDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.GetFullPath("../API/appsettings.json"))
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AccountingDbContext>();
            var persistenceSettings = configuration.GetSection(nameof(PersistenceSettings)).Get<PersistenceSettings>()!;

            if (persistenceSettings.UsePostgres)
            {
                optionsBuilder.UseNpgsql(persistenceSettings.ConnectionStrings.Postgres, x =>
                    x.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
            }
            else if (persistenceSettings.UseMsSql)
            {
                optionsBuilder.UseSqlServer(persistenceSettings.ConnectionStrings.MSSQL, x =>
                    x.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
            }
            else
            {
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }

            var persistenceOptions = Options.Create(persistenceSettings);

            return new AccountingDbContext(
                optionsBuilder.Options,
                null,
                null,
                persistenceOptions,
                null);
        }
    }
}