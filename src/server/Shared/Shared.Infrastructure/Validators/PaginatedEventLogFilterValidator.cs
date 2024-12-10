using System;
using Microsoft.Extensions.Localization;
using Shared.Core.EventLogging;
using Shared.Core.Features.Common.Queries.Validators;
using Shared.DTOs.Identity.EventLogs;

namespace Shared.Infrastructure.Validators
{
    public class PaginatedEventLogFilterValidator(IStringLocalizer<PaginatedEventLogFilterValidator> localizer)
        : PaginatedFilterValidator<Guid, EventLog, PaginatedEventLogsFilter>(localizer)
    {
    }
}