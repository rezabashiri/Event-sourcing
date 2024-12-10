using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Domain.Abstractions;
using Modules.Accounting.Domain.DomainEvents;
using Modules.Accounting.Domain.Entities;
using Shared.Core.EventLogging;
using Shared.Core.Interfaces.Serialization;
using Shared.Core.Interfaces.Services;
using Shared.Core.Wrapper;
using Shared.DTOs.Identity.EventLogs;

namespace Modules.Accounting.Application.Accounts.Services;

public class TransactionService : ITransactionService
{
    private readonly IAccountingDbContext _accountingDbContext;
    private readonly IEventLogService _eventLogService;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IStringLocalizer<TransactionService> _localizer;
    public TransactionService(IEventLogService eventLogService, IAccountingDbContext accountingDbContext, IJsonSerializer jsonSerializer, IStringLocalizer<TransactionService> localizer)
    {
        _eventLogService = eventLogService;
        _accountingDbContext = accountingDbContext;
        _jsonSerializer = jsonSerializer;
        _localizer = localizer;
    }
    public async Task<IResult<Account>> CalculateBalance(Account account)
    {
        var transActions = await _eventLogService.GetAllAsync(new GetEventLogsRequest
        {
            AggregateId = account.Id,
            PageSize = int.MaxValue
        });

        if (transActions?.Succeeded != true || transActions.Data?.Count == 0)
        {
            return Result<Account>.Fail(_localizer["Account should have at least 1 transaction"]);
        }

        var orderedTransActions = transActions.Data!.OrderBy(x => x.Timestamp).ToList();
        var events = DeserializeEvents(orderedTransActions);

        CalculateBalanceByTransactionEvents(events, out decimal balance);

        account.SetBalance(balance, orderedTransActions[^1].Timestamp);

        return Result<Account>.Success(account);
    }

    public async Task<IResult<Account>> FindAccountAndUpdateBalance(Guid accountId)
    {
        var account = await _accountingDbContext.Accounts.FirstOrDefaultAsync(x => x.Id.Equals(accountId));

        if (account == null)
        {
            return Result<Account>.Fail(_localizer["Account not found!"]);
        }

        var calculateBalanceResult = await CalculateBalance(account);

        if (!calculateBalanceResult.Succeeded)
        {
            return Result<Account>.Fail(calculateBalanceResult.Messages);
        }

        return calculateBalanceResult;
    }

    private static void CalculateBalanceByTransactionEvents(IEnumerable<AccountTransactionEventBase> events, out decimal balance)
    {
        balance = 0;
        foreach (var domainEvent in events)
        {
            switch (domainEvent)
            {
                case Deposit deposit:
                    balance += deposit.Amount;
                    break;
                case WithDraw withdraw:
                    balance -= withdraw.Amount;
                    break;
            }
        }
    }

    private IEnumerable<AccountTransactionEventBase> DeserializeEvents(List<EventLog> transactions)
    {
        foreach (var eventLog in transactions)
        {
            var domainEvent = _jsonSerializer.Deserialize<AccountTransactionEventBase>(eventLog.Data);
            if (domainEvent.MessageType.Contains(nameof(Deposit)))
            {
                yield return _jsonSerializer.Deserialize<Deposit>(eventLog.Data);
            }
            else if (domainEvent.MessageType.Contains(nameof(WithDraw)))
            {
                yield return _jsonSerializer.Deserialize<WithDraw>(eventLog.Data);
            }
        }
    }
}