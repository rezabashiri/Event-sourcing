#nullable enable
using System;
using Shared.DTOs.Filters;

namespace Shared.DTOs.Identity.EventLogs
{
    public class PaginatedEventLogsFilter : PaginatedFilter
    {
        public string? SearchString { get; set; }

        public Guid UserId { get; set; }

        public string? Email { get; set; }

        public string? MessageType { get; set; }
    }
}