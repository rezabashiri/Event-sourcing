using Microsoft.EntityFrameworkCore;
using Shared.Core.Entities;
using Shared.Core.EventLogging;

namespace Shared.Core.Interfaces
{
    public interface IApplicationDbContext : IDbContext
    {
        public DbSet<EventLog> EventLogs { get; set; }

        public DbSet<EntityReference> EntityReferences { get; set; }
    }
}