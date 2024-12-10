using System.Linq.Dynamic.Core;
using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Shared.Core.EventLogging;
using Shared.Core.Exceptions;
using Shared.Core.Extensions;
using Shared.Core.Interfaces;
using Shared.Core.Interfaces.Services;
using Shared.Core.Mappings.Converters;
using Shared.Core.Wrapper;
using Shared.DTOs.Identity.EventLogs;

namespace Shared.Infrastructure.Services;

public class EventLogService : IEventLogService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IStringLocalizer<EventLogService> _localizer;
    private readonly IEventLogger _logger;
    private readonly IMapper _mapper;

    public EventLogService(
        IApplicationDbContext dbContext,
        IStringLocalizer<EventLogService> localizer,
        IMapper mapper,
        IEventLogger logger)
    {
        _dbContext = dbContext;
        _localizer = localizer;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedResult<EventLog>> GetAllAsync(GetEventLogsRequest request)
    {
        var queryable = _dbContext.EventLogs.AsNoTracking().AsQueryable();

        if (request.UserId != Guid.Empty)
        {
            queryable = queryable.Where(x => x.UserId.Equals(request.UserId));
        }

        if (request.AggregateId != Guid.Empty)
        {
            queryable = queryable.Where(x => x.AggregateId.Equals(request.AggregateId));
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            queryable = queryable.Where(x => EF.Functions.Like(x.Email.ToLower(), $"%{request.Email.ToLower()}%"));
        }

        if (!string.IsNullOrWhiteSpace(request.MessageType))
        {
            queryable = queryable.Where(x => EF.Functions.Like(x.MessageType.ToLower(), $"%{request.MessageType.ToLower()}%"));
        }

        string ordering = new OrderByConverter().Convert(request.OrderBy);
        queryable = !string.IsNullOrWhiteSpace(ordering) ? queryable.OrderBy(ordering) : queryable.OrderByDescending(a => a.Timestamp);

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            string lowerSearchString = request.SearchString.ToLower();
            queryable = queryable.Where(x => !string.IsNullOrWhiteSpace(x.Data) && EF.Functions.Like(x.Data.ToLower(), $"%{lowerSearchString}%")
                                             || !string.IsNullOrWhiteSpace(x.OldValues) && EF.Functions.Like(x.OldValues.ToLower(), $"%{lowerSearchString}%")
                                             || !string.IsNullOrWhiteSpace(x.NewValues) && EF.Functions.Like(x.NewValues.ToLower(), $"%{lowerSearchString}%")
                                             || EF.Functions.Like(x.Id.ToString().ToLower(), $"%{lowerSearchString}%"));
        }

        var eventLogList = await queryable
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);
        if (eventLogList == null)
        {
            throw new CustomException(_localizer["Event Logs Not Found!"], statusCode: HttpStatusCode.NotFound);
        }

        return eventLogList;
    }

    public async Task<Result<string>> LogCustomEventAsync(LogEventRequest request)
    {
        var log = _mapper.Map<EventLog>(request);
        await _logger.SaveAsync(log, default);
        return await Result<string>.SuccessAsync(data: log.Id.ToString());
    }
}