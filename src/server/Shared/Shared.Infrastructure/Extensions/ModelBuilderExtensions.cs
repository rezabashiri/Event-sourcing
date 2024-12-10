using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Shared.Core.Domain;
using Shared.Core.Settings;
using Shared.Infrastructure.Persistence;

namespace Shared.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyApplicationConfiguration(this ModelBuilder builder, PersistenceSettings persistenceOptions)
    {
        if (persistenceOptions.UseMsSql)
        {
            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(23,2)");
            }
        }

        SetDateTimeToUtc(builder);
    }

    public static void ApplyModuleConfiguration(this ModelBuilder builder, PersistenceSettings persistenceOptions)
    {
        foreach (var entity in builder.Model.GetEntityTypes().Where(m => typeof(BaseEntity).IsAssignableFrom(m.ClrType)))
        {
            var property = entity.GetProperties().Single(p => p.IsKey() && p.Name == nameof(BaseEntity.Id));
            property.ValueGenerated = ValueGenerated.Never;
        }

        SetDateTimeToUtc(builder);
    }
    public static void SetDateTimeToUtc(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var dateTimeProperties = entityType.ClrType.GetProperties()
                .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

            foreach (var property in dateTimeProperties)
            {
                modelBuilder.Entity(entityType.Name).Property(property.Name)
                    .HasConversion(typeof(UtcDateTimeConverter));
            }
        }
    }
}