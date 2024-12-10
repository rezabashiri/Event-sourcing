using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Shared.Core.Domain;
using Shared.Core.EventLogging;
using Shared.Core.Interfaces;
using Shared.Core.Interfaces.Serialization;
using Shared.Core.Settings;
using Shared.Infrastructure.Extensions;

namespace Shared.Infrastructure.Persistence
{
    public abstract class ModuleDbContext : DbContext, IModuleDbContext
    {
        private readonly IEventLogger _eventLogger;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IMediator _mediator;
        private readonly PersistenceSettings _persistenceOptions;

        protected ModuleDbContext(
            DbContextOptions options,
            IMediator mediator,
            IEventLogger eventLogger,
            IOptions<PersistenceSettings> persistenceOptions,
            IJsonSerializer jsonSerializer)
            : base(options)
        {
            _mediator = mediator;
            _eventLogger = eventLogger;
            _persistenceOptions = persistenceOptions.Value;
            _jsonSerializer = jsonSerializer;
        }

        protected abstract string Schema { get; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changes = OnBeforeSaveChanges();
            return await this.SaveChangeWithPublishEventsAsync(_eventLogger, _mediator, changes, _jsonSerializer, cancellationToken);
        }

        public override int SaveChanges()
        {
            var changes = OnBeforeSaveChanges();
            return this.SaveChangeWithPublishEvents(_eventLogger, _mediator, changes, _jsonSerializer);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (!string.IsNullOrWhiteSpace(Schema))
            {
                modelBuilder.HasDefaultSchema(Schema);
            }

            modelBuilder.Ignore<Event>();
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.ApplyModuleConfiguration(_persistenceOptions);
        }

        private List<(EntityEntry entityEntry, string oldValues, string newValues)> OnBeforeSaveChanges()
        {
            var result = new List<(EntityEntry entityEntry, string oldValues, string newValues)>();
            ChangeTracker.DetectChanges();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                var previousData = new Dictionary<string, object>();
                var currentData = new Dictionary<string, object>();
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    object originalValue = entry.GetDatabaseValues()?.GetValue<object>(propertyName);
                    switch (entry.State)
                    {
                        case EntityState.Unchanged:
                            break;
                        case EntityState.Added:
                            currentData[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            previousData[propertyName] = originalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified && originalValue?.Equals(property.CurrentValue) == false)
                            {
                                previousData[propertyName] = originalValue;
                                currentData[propertyName] = property.CurrentValue;
                            }

                            break;
                    }
                }

                string oldValues = previousData.Count == 0 ? null : _jsonSerializer.Serialize(previousData);
                string newValues = currentData.Count == 0 ? null : _jsonSerializer.Serialize(currentData);
                result.Add((entry, oldValues, newValues));
            }

            return result;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return SaveChanges();
        }
    }
}