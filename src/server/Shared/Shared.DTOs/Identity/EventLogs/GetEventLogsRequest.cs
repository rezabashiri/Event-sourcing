#nullable enable
using System;

namespace Shared.DTOs.Identity.EventLogs;

public record GetEventLogsRequest
{
    public Guid AggregateId { get; init; }
    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public string? SearchString { get; init; }

    public string[]? OrderBy { get; init; }

    public Guid UserId { get; init; }

    public string? Email { get; init; }

    public string? MessageType { get; init; }
}