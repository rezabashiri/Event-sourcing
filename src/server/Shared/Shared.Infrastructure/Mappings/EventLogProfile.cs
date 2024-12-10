using AutoMapper;
using Shared.Core.EventLogging;
using Shared.Core.Mappings.Converters;
using Shared.DTOs.Identity.EventLogs;

namespace Shared.Infrastructure.Mappings
{
    public class EventLogProfile : Profile
    {
        public EventLogProfile()
        {
            CreateMap<PaginatedEventLogsFilter, GetEventLogsRequest>()
                .ForMember(dest => dest.OrderBy, opt => opt.ConvertUsing<string>(new OrderByConverter()));
            CreateMap<LogEventRequest, EventLog>().ReverseMap();
        }
    }
}