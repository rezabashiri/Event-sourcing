using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Application.User.Queries;

namespace Modules.Accounting.IntegrationTests.User;

public class GetAllUsersHandlerTests : BaseTest
{
    private readonly IStringLocalizer<GetAllUsersHandler> _localizer;

    public GetAllUsersHandlerTests()
    {
        _localizer = ServiceProvider.GetRequiredService<IStringLocalizer<GetAllUsersHandler>>();
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedList_WhenUsersExist()
    {
        var user1 = Domain.Entities.User.Create("User One");
        var user2 = Domain.Entities.User.Create("User Two");

        AccountingDbContext.Users.AddRange(user1, user2);
        await AccountingDbContext.SaveChangesAsync();

        var query = new GetAllUsers();

        var result = await Mediator.Send(query, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNullOrEmpty();
        result.Data.Should().HaveCount(2);
        result.Data[0].FullName.Should().Be("User One");
        result.Data[1].FullName.Should().Be("User Two");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoUsersExist()
    {
        var query = new GetAllUsers();

        var result = await Mediator.Send(query, CancellationToken.None);

        result.Succeeded.Should().BeFalse();
        result.Messages.Should().Contain(_localizer["User not found!"]);
    }
}