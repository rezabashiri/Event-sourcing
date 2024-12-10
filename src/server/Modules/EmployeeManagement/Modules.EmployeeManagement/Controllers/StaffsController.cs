using System.Net;
using Microsoft.AspNetCore.Mvc;
using Modules.EmployeeManagement.Core.Features.Staff.Commands;

namespace Modules.EmployeeManagement.Controllers;

[ApiVersion("1")]
public class StaffsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult> EmployStaffAsync(EmployStaffCommand command)
        => StatusCode((int)HttpStatusCode.Created, await Mediator.Send(command));
}