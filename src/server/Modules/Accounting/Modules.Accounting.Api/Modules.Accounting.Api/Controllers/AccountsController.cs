using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Accounting.Application.Accounts.Commands;
using Modules.Accounting.Application.Accounts.Queries;
using Modules.Accounting.Application.Controllers;

namespace Modules.Accounting.Api.Controllers;

[ApiVersion("1")]
public class AccountsController(IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> CreateAccount(CreateAccountCommand command)
    {
        var result = await mediator.Send(command);

        if (result.Succeeded)
        {
            return CreatedAtAction(nameof(GetUserAccounts), new
                {
                    userId = command.UserId
                },
                command.UserId);
        }

        return BadRequest(result);
    }

    [HttpGet("[Action]/{userId}")]
    public async Task<IActionResult> GetUserAccounts(Guid userId)
    {
        var result = await mediator.Send(new GetUserAccountsByUserIdQuery(userId));

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound(result);
    }

    [HttpPut("[Action]")]
    public async Task<IActionResult> Deposit(DepositCommand command)
    {
        var result = await mediator.Send(command);

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPut("[Action]")]
    public async Task<IActionResult> Withdraw(WithdrawCommand command)
    {
        var result = await mediator.Send(command);

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("{accountId}")]
    public async Task<IActionResult> DeleteAccount(Guid accountId)
    {
        var result = await mediator.Send(new DeleteAccountCommand(accountId));

        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(result);
    }
}