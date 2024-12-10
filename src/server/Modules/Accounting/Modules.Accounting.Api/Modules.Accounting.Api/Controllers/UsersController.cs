using MediatR;
using Microsoft.AspNetCore.Mvc;
using Modules.Accounting.Application.Controllers;
using Modules.Accounting.Application.User.Commands;
using Modules.Accounting.Application.User.Queries;

namespace Modules.Accounting.Api.Controllers;

[ApiVersion("1")]
public class UsersController(IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var result = await mediator.Send(command);

        if (result.Succeeded)
        {
            return CreatedAtAction(nameof(GetUser), new
                {
                    id = result.Data
                },
                result.Data);
        }

        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await mediator.Send(new GetAllUsers());

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound(result);
    }
}