using System.Threading;
using System.Threading.Tasks;

namespace Shared.Core.Interfaces
{
    public interface IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        int SaveChanges();
    }
}