using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.Accounting.Application.Accounts.Commands;
using Modules.Accounting.Domain.DomainEvents;
using Modules.Accounting.Domain.Entities;
using Modules.Accounting.Domain.Exceptions;
using Shared.Core.Exceptions;

namespace Modules.Accounting.IntegrationTests.Accounts;

public class WithdrawCommandHandlerTests : BaseTest
{
    private readonly TestEventHandler<WithDraw> _handler = new();

    protected override void OnBuildServiceProvider(IServiceCollection services)
    {
        services.AddTransient<INotificationHandler<WithDraw>>(_ => _handler);
        base.OnBuildServiceProvider(services);
    }
    [Fact]
    public async Task Handle_AccountExists_ShouldWithdrawAmount()
    {
        var userId = Guid.NewGuid();
        decimal initialDeposit = 1000m;
        decimal withdrawAmount = 200m;
        var account = Account.Create(userId, "Test Account", initialDeposit);

        AccountingDbContext.Accounts.Add(account);
        await AccountingDbContext.SaveChangesAsync();

        var command = new WithdrawCommand(account.Id, withdrawAmount);
        var result = await Mediator.Send(command);

        result.Succeeded.Should().BeTrue();
        result.Data.Balance.Should().Be(initialDeposit - withdrawAmount);

        var updatedAccount = await AccountingDbContext.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id);
        updatedAccount!.Balance.Should().Be(initialDeposit - withdrawAmount);
        _handler.PublishedEvents.Should().HaveCount(1);
        _handler.PublishedEvents[0].Should().BeOfType<WithDraw>();
    }

    [Fact]
    public async Task Handle_AccountExists_ShouldWithdrawAmountMultipleTimes()
    {
        var userId = Guid.NewGuid();
        decimal initialDeposit = 1000m;
        decimal withdrawAmount = 200m;
        var account = Account.Create(userId, "Test Account", initialDeposit);

        AccountingDbContext.Accounts.Add(account);
        await AccountingDbContext.SaveChangesAsync();

        var command = new WithdrawCommand(account.Id, withdrawAmount);
        await Mediator.Send(command);
        var result = await Mediator.Send(command);

        result.Succeeded.Should().BeTrue();
        result.Data.Balance.Should().Be(initialDeposit - withdrawAmount * 2);

        var updatedAccount = await AccountingDbContext.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id);
        updatedAccount!.Balance.Should().Be(initialDeposit - withdrawAmount * 2);
        _handler.PublishedEvents.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_AccountDoesNotExist_ShouldReturnFailure()
    {
        var command = new WithdrawCommand(Guid.NewGuid(), 100m);
        var result = await Mediator.Send(command);

        result.Succeeded.Should().BeFalse();
        result.Messages.Should().Contain("Account not found!");
    }

    [Fact]
    public async Task Handle_WithdrawMoreThanAllowed_ShouldReturnFailure()
    {
        var userId = Guid.NewGuid();
        decimal initialDeposit = 1000m;
        decimal withdrawAmount = 901m;
        var account = Account.Create(userId, "Test Account", initialDeposit);

        AccountingDbContext.Accounts.Add(account);
        await AccountingDbContext.SaveChangesAsync();

        var command = new WithdrawCommand(account.Id, withdrawAmount);

        await Assert.ThrowsAsync<WithdrawMoreThanAllowanceException>(() => Mediator.Send(command));
    }

    [Fact]
    public async Task Handle_WithdrawToMinimumAccountBalance_ShouldReturnFailure()
    {
        var userId = Guid.NewGuid();
        decimal initialDeposit = 1000m;
        decimal withdrawAmount = 700m;
        var account = Account.Create(userId, "Test Account", initialDeposit);

        AccountingDbContext.Accounts.Add(account);
        await AccountingDbContext.SaveChangesAsync();
        var command = new WithdrawCommand(account.Id, withdrawAmount);
        await Mediator.Send(command);

        await Assert.ThrowsAsync<AccountBalanceLessThanMinimumException>(() => Mediator.Send(new WithdrawCommand(account.Id, 250)));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAmountIsZero()
    {
        var command = new WithdrawCommand(Guid.NewGuid(), 0);
        await Assert.ThrowsAsync<CustomValidationException>(() => Mediator.Send(command));
    }
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAmountIsNegative()
    {
        var command = new WithdrawCommand(Guid.NewGuid(), -10);
        await Assert.ThrowsAsync<CustomValidationException>(() => Mediator.Send(command));
    }
}