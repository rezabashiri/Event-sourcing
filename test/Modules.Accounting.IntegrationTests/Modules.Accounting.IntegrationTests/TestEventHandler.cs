using MediatR;
using Shared.Core.Domain;

namespace Modules.Accounting.IntegrationTests;

internal class TestEventHandler<T> : IDisposable, INotificationHandler<T> where T : Event
{
    internal List<T> PublishedEvents { get; } = [];

    public void Dispose()
    {
        PublishedEvents.Clear();
    }
    public Task Handle(T notification, CancellationToken cancellationToken)
    {
        PublishedEvents.Add(notification);
        return Task.CompletedTask;
    }
}