using Modules.Accounting.Domain.Events;
using Shared.Core.Domain;

namespace Modules.Accounting.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; private set; } = string.Empty;
    public ICollection<Account> Accounts { get; init; } = new HashSet<Account>();

    public void CreateAccount(Account account)
    {
        Accounts.Add(account);
    }

    public void DeleteAccount(Account account)
    {
        Accounts.Remove(account);
    }

    public static User Create(string fullName)
    {
        var user = new User
        {
            FullName = fullName
        };
        user.AddDomainEvent(new UserCreated(user));
        return user;
    }
}