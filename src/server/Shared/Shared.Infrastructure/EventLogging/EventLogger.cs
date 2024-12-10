using System.Threading.Tasks;
using Shared.Core.Domain;
using Shared.Core.EventLogging;
using Shared.Core.Interfaces;
using Shared.Core.Interfaces.Serialization;
using Shared.Core.Interfaces.Services.Identity;

namespace Shared.Infrastructure.EventLogging
{
    internal class EventLogger : IEventLogger
    {
        private readonly IApplicationDbContext _context;
        private readonly IJsonSerializer _jsonSerializer;

        public EventLogger(
            IApplicationDbContext context,
            IJsonSerializer jsonSerializer)
        {
            _context = context;
            _jsonSerializer = jsonSerializer;
        }

        public async Task SaveAsync<T>(T @event, (string oldValues, string newValues) changes)
            where T : Event
        {
            if (@event is EventLog eventLog)
            {
                await _context.EventLogs.AddAsync(eventLog);
                await _context.SaveChangesAsync();
            }
            else
            {
                string serializedData = _jsonSerializer.Serialize(@event, @event.GetType());

                var thisEvent = new EventLog(
                    @event,
                    serializedData,
                    changes,
                    "rzbashiri@gmail.com",
                    Guid.NewGuid());
                await _context.EventLogs.AddAsync(thisEvent);
                await _context.SaveChangesAsync();
            }
        }
    }
}