using Modules.Accounting.Domain.Entities;
using Shared.Core.Domain;

namespace Modules.Accounting.Domain.Events;

public record AccountDeleted : Event
{
    public AccountDeleted(Account deletedAccount)
    {
        Name = deletedAccount.Name;
        AccountId = deletedAccount.Id;
    }

    public string Name { get; init; }
    public Guid AccountId { get; init; }
}