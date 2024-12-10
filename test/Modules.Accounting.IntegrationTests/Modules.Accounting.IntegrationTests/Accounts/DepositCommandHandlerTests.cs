using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Application.Accounts.Commands;
using Modules.Accounting.Domain.DomainEvents;
using Modules.Accounting.Domain.Entities;
using Shared.Core.Exceptions;
using Shared.Core.Interfaces.Services;
using Shared.Core.Utilities;
using Shared.DTOs.Identity.EventLogs;

namespace Modules.Accounting.IntegrationTests.Accounts;

public class DepositCommandHandlerTests : BaseTest
{
    private readonly TestEventHandler<Deposit> _eventHandler = new();
    private readonly IEventLogService _eventLogService;
    private readonly IStringLocalizer<DepositCommandHandler> _localizer;
    public DepositCommandHandlerTests()
    {
        _localizer = ServiceProvider.GetRequiredService<IStringLocalizer<DepositCommandHandler>>();
        _eventLogService = ServiceProvider.GetRequiredService<IEventLogService>();
    }
    protected override void OnBuildServiceProvider(IServiceCollection services)
    {
        services.AddTransient<INotificationHandler<Deposit>>(_ => _eventHandler);
    }
    [Fact]
    public async Task Handle_ShouldDeposit_WhenAccountExists()
    {
        var account = Account.Create(Guid.NewGuid(), "Test Account", 1000);
        AccountingDbContext.Accounts.Add(account);
        await AccountingDbContext.SaveChangesAsync();

        var result = await Mediator.Send(new DepositCommand(account.Id, 500));

        result.Succeeded.Should().BeTrue();
        result.Data.Balance.Should().Be(1500);

        var updatedAccount = await AccountingDbContext.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id);
        var addedDepositEvents = await _eventLogService.GetAllAsync(new GetEventLogsRequest
        {
            AggregateId = account.Id
        });

        updatedAccount.Should().NotBeNull();
        updatedAccount!.Balance.Should().Be(1500);

        addedDepositEvents.Data.Should().HaveCount(2);
        addedDepositEvents.Data.Should().AllSatisfy(x => x.MessageType.Equals(typeof(Deposit).GetGenericTypeName()));
        _eventHandler.PublishedEvents.Should().HaveCount(2);
        foreach (var eventHandlerPublishedEvent in _eventHandler.PublishedEvents)
        {
            eventHandlerPublishedEvent.Should().BeOfType<Deposit>();
        }
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAccountNotFound()
    {
        var command = new DepositCommand(Guid.NewGuid(), 500);
        var result = await Mediator.Send(command);

        result.Succeeded.Should().BeFalse();
        result.Messages.Should().Contain(_localizer["Account not found!"]);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAmountIsZero()
    {
        var command = new DepositCommand(Guid.NewGuid(), 0);
        await Assert.ThrowsAsync<CustomValidationException>(() => Mediator.Send(command));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAmountIsNegative()
    {
        var command = new DepositCommand(Guid.NewGuid(), -10);
        await Assert.ThrowsAsync<CustomValidationException>(() => Mediator.Send(command));
    }
}