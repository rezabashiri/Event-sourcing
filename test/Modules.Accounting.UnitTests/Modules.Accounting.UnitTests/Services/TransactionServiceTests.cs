using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Application.Accounts.Services;
using Modules.Accounting.Domain.Abstractions;
using Modules.Accounting.Domain.DomainEvents;
using Modules.Accounting.Domain.Entities;
using Moq;
using Shared.Core.EventLogging;
using Shared.Core.Interfaces.Serialization;
using Shared.Core.Interfaces.Services;
using Shared.Core.Wrapper;
using Shared.DTOs.Identity.EventLogs;

namespace Modules.Accounting.UnitTests.Services;

public class TransactionServiceTests : BaseTest
{
    private readonly Mock<IAccountingDbContext> _accountingDbContextMock;
    private readonly Mock<IEventLogService> _eventLogServiceMock;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly Mock<IStringLocalizer<TransactionService>> _localizerMock;
    private readonly TransactionService _transactionService;

    public TransactionServiceTests()
    {
        _accountingDbContextMock = new Mock<IAccountingDbContext>();
        _eventLogServiceMock = new Mock<IEventLogService>();
        _jsonSerializer = ServiceProvider.GetRequiredService<IJsonSerializer>();
        _localizerMock = new Mock<IStringLocalizer<TransactionService>>();

        _transactionService = new TransactionService(
            _eventLogServiceMock.Object,
            _accountingDbContextMock.Object,
            _jsonSerializer,
            _localizerMock.Object
        );
    }

    [Fact]
    public async Task UpdateBalance_WhenNoTransactions_ShouldReturnFailure()
    {
        var account = Account.Create(Guid.NewGuid(), "Test Account", 500);

        _eventLogServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<GetEventLogsRequest>()))
            .ReturnsAsync(PaginatedResult<EventLog>.Success(new List<EventLog>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));

        _localizerMock.Setup(x => x["Account should have at least 1 transaction"])
            .Returns(new LocalizedString("Account should have at least 1 transaction", "Account should have at least 1 transaction"));

        var result = await _transactionService.CalculateBalance(account);

        result.Succeeded.Should().BeFalse();
        result.Messages.Should().Contain("Account should have at least 1 transaction");
    }

    [Fact]
    public async Task UpdateBalance_WhenTransactionsExist_ShouldUpdateAccountBalance()
    {
        var account = Account.Create(Guid.NewGuid(), "Test Account", 500);
        var initDeposit = new Deposit(500, account.Id);
        var depositEvent = new Deposit(100, account.Id);
        var withDrawEvent = new WithDraw(50, account.Id);
        var eventLogs = new List<EventLog>
        {
            new(initDeposit, _jsonSerializer.Serialize(initDeposit), (null, null), string.Empty, Guid.NewGuid()),
            new(depositEvent, _jsonSerializer.Serialize(depositEvent), (null, null), string.Empty, Guid.NewGuid()),
            new(withDrawEvent, _jsonSerializer.Serialize(withDrawEvent), (null, null), string.Empty, Guid.NewGuid())
        };

        _eventLogServiceMock
            .Setup(x => x.GetAllAsync(It.IsAny<GetEventLogsRequest>()))
            .ReturnsAsync(PaginatedResult<EventLog>.Success(eventLogs, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));

        var result = await _transactionService.CalculateBalance(account);

        result.Succeeded.Should().BeTrue();
        result.Data.Balance.Should().Be(550);
    }
}