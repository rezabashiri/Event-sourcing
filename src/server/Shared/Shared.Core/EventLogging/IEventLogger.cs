using System.Threading.Tasks;
using Shared.Core.Domain;

namespace Shared.Core.EventLogging
{
    public interface IEventLogger
    {
        Task SaveAsync<T>(T @event, (string oldValues, string newValues) changes)
            where T : Event;
    }
}