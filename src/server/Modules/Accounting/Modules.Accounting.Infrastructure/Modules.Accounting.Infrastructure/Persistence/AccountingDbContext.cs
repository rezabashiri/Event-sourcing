using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Modules.Accounting.Domain.Abstractions;
using Modules.Accounting.Domain.Entities;
using Shared.Core.EventLogging;
using Shared.Core.Interfaces.Serialization;
using Shared.Core.Settings;
using Shared.Infrastructure.Persistence;

namespace Modules.Accounting.Infrastructure.Persistence;

public class AccountingDbContext(DbContextOptions options,
    IMediator mediator,
    IEventLogger eventLogger,
    IOptions<PersistenceSettings> persistenceOptions,
    IJsonSerializer jsonSerializer)
    : ModuleDbContext(options, mediator, eventLogger, persistenceOptions, jsonSerializer), IAccountingDbContext
{
    protected override string Schema => "Accounting";

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(user => user.Accounts);

        base.OnModelCreating(modelBuilder);
    }
}