
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Core.Entities;
using Shared.Core.EventLogging;
using Shared.Core.Interfaces;
using Shared.Core.Settings;
using Shared.Infrastructure.Extensions;

namespace Shared.Infrastructure.Persistence
{
    internal class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly PersistenceSettings _persistenceOptions;

        protected string Schema => "Application";

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IOptions<PersistenceSettings> persistenceOptions)
                : base(options)
        {
            _persistenceOptions = persistenceOptions.Value;
        }

        public DbSet<EventLog> EventLogs { get; set; }

        public DbSet<EntityReference> EntityReferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyApplicationConfiguration(_persistenceOptions);
        }
    }
}