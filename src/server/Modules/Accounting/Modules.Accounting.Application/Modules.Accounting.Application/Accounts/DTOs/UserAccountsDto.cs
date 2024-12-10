namespace Modules.Accounting.Application.Accounts.DTOs;

public record UserAccountsDto(Guid UserId, IReadOnlyList<AccountDTo> Accounts);