using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Modules.Accounting.Application.Accounts.Commands;
using Modules.Accounting.Domain.Entities;
using Modules.Accounting.Domain.Events;

namespace Modules.Accounting.IntegrationTests.Accounts;

public class DeleteAccountHandlerTests : BaseTest
{
    private TestEventHandler<AccountDeleted> _handler= new();

    protected override void OnBuildServiceProvider(IServiceCollection services)
    {
        services.AddTransient<INotificationHandler<AccountDeleted>>(_ => _handler);
        base.OnBuildServiceProvider(services);
    }
    [Fact]
    public async Task Handle_AccountExists_ShouldDeleteAccount()
    {
        var userId = Guid.NewGuid();
        decimal initialDeposit = 1000m;

        var account = Account.Create(userId, "Test Account", initialDeposit);
        AccountingDbContext.Accounts.Add(account);
        await AccountingDbContext.SaveChangesAsync();

        var command = new DeleteAccountCommand(account.Id);
        var result = await Mediator.Send(command);

        result.Succeeded.Should().BeTrue();

        var deletedAccount = await AccountingDbContext.Accounts.FindAsync(account.Id);
        deletedAccount.Should().BeNull();
        _handler.PublishedEvents.Should().HaveCount(1);
        _handler.PublishedEvents.First().Should().BeOfType<AccountDeleted>();
    }

    [Fact]
    public async Task Handle_AccountDoesNotExist_ShouldReturnFailure()
    {
        var command = new DeleteAccountCommand(Guid.NewGuid());
        var result = await Mediator.Send(command);

        result.Succeeded.Should().BeFalse();
        result.Messages.Should().Contain("Account not found!");
    }
}