using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Application.Accounts.Queries;
using Modules.Accounting.Domain.Entities;

namespace Modules.Accounting.IntegrationTests.Accounts;

public class GetUserAccountsByUserIdHandlerTests : BaseTest
{
    private readonly IStringLocalizer<GetUserAccountsByUserIdHandler> _localizer;

    public GetUserAccountsByUserIdHandlerTests()
    {
        _localizer = ServiceProvider.GetRequiredService<IStringLocalizer<GetUserAccountsByUserIdHandler>>();
    }

    [Fact]
    public async Task Handle_UserExists_ShouldReturnUserAccounts()
    {
        var user = Domain.Entities.User.Create("Test User");
        var account = Account.Create(user.Id, "Savings", 500);
        user.CreateAccount(account);

        AccountingDbContext.Users.Add(user);
        await AccountingDbContext.SaveChangesAsync();

        var query = new GetUserAccountsByUserIdQuery(user.Id);

        var result = await Mediator.Send(query);

        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.UserId.Should().Be(user.Id);
        result.Data.Accounts.Should().HaveCount(1);
        result.Data.Accounts[0].Name.Should().Be(account.Name);
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ShouldReturnFailure()
    {
        var nonExistentUserId = Guid.NewGuid();
        var query = new GetUserAccountsByUserIdQuery(nonExistentUserId);

        var result = await Mediator.Send(query);

        result.Succeeded.Should().BeFalse();
        result.Messages.Should().Contain(_localizer["User not found!"]);
    }

    [Fact]
    public async Task Handle_UserHasNoAccounts_ShouldReturnEmptyAccountList()
    {
        var user = Domain.Entities.User.Create("Test User");
        AccountingDbContext.Users.Add(user);
        await AccountingDbContext.SaveChangesAsync();

        var query = new GetUserAccountsByUserIdQuery(user.Id);

        var result = await Mediator.Send(query);

        result.Succeeded.Should().BeFalse();
    }
}