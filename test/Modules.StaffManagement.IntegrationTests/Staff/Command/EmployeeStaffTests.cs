using FluentAssertions;
using Modules.EmployeeManagement.Core.Features.Staff.Commands;
using Shared.Core.Exceptions;
using Xunit;

namespace Modules.EmployeeManagement.IntegrationTests.Staff.Command;

public class EmployeeStaffTests : BaseTest
{

    [Fact]
    public async Task Handle_EmployStaffNewStaff_ShouldInsertInDataBase()
    {
        var newEmployee = await Mediator.Send(new EmployStaffCommand("123456", "Reza", "Bashiri", DateTime.Now));

        newEmployee.Data.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_EmptyRequiredFields_ShouldThrowException()
    {
        await Assert.ThrowsAnyAsync<CustomValidationException>(() => Mediator.Send(new EmployStaffCommand("", "", "", DateTime.Now)));

    }
}