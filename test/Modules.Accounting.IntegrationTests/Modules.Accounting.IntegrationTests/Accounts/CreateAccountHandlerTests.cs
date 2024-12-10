using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.Accounting.Application.Accounts.Commands;
using Modules.Accounting.Domain.DomainEvents;
using Modules.Accounting.Domain.Exceptions;

namespace Modules.Accounting.IntegrationTests.Accounts;

public class CreateAccountHandlerTests : BaseTest
{
    private readonly TestEventHandler<Deposit> _eventHandler = new();

    protected override void OnBuildServiceProvider(IServiceCollection services)
    {
        services.AddTransient<INotificationHandler<Deposit>>(_ => _eventHandler);
    }

    [Fact]
    public async Task Handle_UserExists_ShouldCreateAccount()
    {
        var user = Domain.Entities.User.Create("Existing User");
        AccountingDbContext.Users.Add(user);
        await AccountingDbContext.SaveChangesAsync();
        var command = new CreateAccountCommand(user.Id, "New Account", 500);
        var result = await Mediator.Send(command);

        Assert.True(result.Succeeded);
        var account = await AccountingDbContext.Accounts.FirstOrDefaultAsync(x => x.UserId == user.Id);
        var dbUser = await AccountingDbContext.Users.Include(x => x.Accounts).FirstOrDefaultAsync(x => x.Id == user.Id);
        dbUser.Should().NotBeNull();
        dbUser!.Accounts.Should().HaveCount(1);
        Assert.NotNull(account);
        Assert.Equal("New Account", account.Name);

        _eventHandler.PublishedEvents.Should().ContainSingle();
        _eventHandler.PublishedEvents[0].Should().BeOfType<Deposit>();
        _eventHandler.PublishedEvents[0].AggregateId.Should().Be(account.Id);
        _eventHandler.PublishedEvents[0].Amount.Should().Be(500);
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ShouldReturnFailure()
    {
        var command = new CreateAccountCommand(Guid.NewGuid(), "New Account", 500);
        var result = await Mediator.Send(command);

        result.Succeeded.Should().BeFalse();
        result.Messages.Should().Contain("User does not exists!");
        _eventHandler.PublishedEvents.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_InitialDepositIsInvalid_ShouldThrowValidationException()
    {
        var user = Domain.Entities.User.Create("Existing User");
        AccountingDbContext.Users.Add(user);
        await AccountingDbContext.SaveChangesAsync();

        var command = new CreateAccountCommand(user.Id, "New Account", -100);
        await Assert.ThrowsAsync<AccountBalanceLessThanMinimumException>(() => Mediator.Send(command));

        var account = await AccountingDbContext.Accounts.FirstOrDefaultAsync(x => x.UserId == user.Id);
        account.Should().BeNull();
    }
}