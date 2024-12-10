using System.Threading.Tasks;
using Shared.Core.EventLogging;
using Shared.Core.Wrapper;
using Shared.DTOs.Identity.EventLogs;

namespace Shared.Core.Interfaces.Services
{
    public interface IEventLogService
    {
        Task<PaginatedResult<EventLog>> GetAllAsync(GetEventLogsRequest request);

        Task<Result<string>> LogCustomEventAsync(LogEventRequest request);
    }
}