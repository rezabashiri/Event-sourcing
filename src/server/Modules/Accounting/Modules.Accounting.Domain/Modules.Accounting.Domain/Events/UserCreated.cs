using Modules.Accounting.Domain.Entities;
using Shared.Core.Domain;

namespace Modules.Accounting.Domain.Events;

public record UserCreated : Event
{
    public UserCreated(User createdUser)
    {
        FullName = createdUser.FullName;
    }

    public string FullName { get; init; }
}