using FluentAssertions;
using Modules.Accounting.Application.User.Queries;

namespace Modules.Accounting.IntegrationTests.User;

public class GetUserByIdHandlerTests : BaseTest
{
    [Fact]
    public async Task Handle_ShouldReturnUser_WhenUserExists()
    {
        string fullName = "Test User";
        var existingUser = Domain.Entities.User.Create(fullName);
        AccountingDbContext.Users.Add(existingUser);
        await AccountingDbContext.SaveChangesAsync();

        var query = new GetUserByIdQuery(existingUser.Id);

        var result = await Mediator.Send(query);

        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Id.Should().Be(existingUser.Id);
        result.Data.FullName.Should().Be(fullName);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
    {

        var nonExistentUserId = Guid.NewGuid();
        var query = new GetUserByIdQuery(nonExistentUserId);

        var result = await Mediator.Send(query);

        result.Succeeded.Should().BeFalse();
        result.Messages.Should().Contain("User not found!");
    }
}