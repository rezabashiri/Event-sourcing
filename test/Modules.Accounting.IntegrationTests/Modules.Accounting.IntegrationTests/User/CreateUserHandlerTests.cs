using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.Accounting.Application.User.Commands;
using Modules.Accounting.Domain.Events;
using Shared.Core.Exceptions;

namespace Modules.Accounting.IntegrationTests.User;

public class CreateUserHandlerTests : BaseTest
{
    private readonly TestEventHandler<UserCreated> _eventHandler = new();

    protected override void OnBuildServiceProvider(IServiceCollection services)
    {
        services.AddTransient<INotificationHandler<UserCreated>>(_ => _eventHandler);
    }
    [Fact]
    public async Task Handle_UserDoesNotExist_ShouldCreateUser()
    {
        string fullName = "New User";
        var command = new CreateUserCommand(fullName);
        var result = await Mediator.Send(command);
        Assert.True(result.Succeeded);
        result.Data.Should().NotBe(Guid.Empty);
        var user = await AccountingDbContext.Users.FirstOrDefaultAsync(x => x.Id == result.Data);
        _eventHandler.PublishedEvents.Should().ContainSingle();
        _eventHandler.PublishedEvents[0].Should().BeOfType<UserCreated>();
        _eventHandler.PublishedEvents[0].FullName.Should().Be(fullName);
        Assert.NotNull(user);
        Assert.Equal(fullName, user.FullName);
    }

    [Fact]
    public async Task Handle_UserAlreadyExists_ShouldReturnFailure()
    {
        string fullName = "Existing User";
        AccountingDbContext.Users.Add(Domain.Entities.User.Create(fullName));
        await AccountingDbContext.SaveChangesAsync();
        var command = new CreateUserCommand(fullName);
        var result = await Mediator.Send(command);
        _eventHandler.PublishedEvents.Should().HaveCount(1);
        _eventHandler.PublishedEvents[0].FullName.Should().Be(fullName);
        result.Succeeded.Should().BeFalse();
        result.Messages.Count.Should().Be(1);
        result.Messages.Should().Contain("User already exists!");
    }

    [Fact]
    public async Task Handle_UserFullNameIsNullOrEmpty_ShouldReturnFailure()
    {
        var command = new CreateUserCommand(string.Empty);
        await Assert.ThrowsAsync<CustomValidationException>(() => Mediator.Send(command));
    }
}