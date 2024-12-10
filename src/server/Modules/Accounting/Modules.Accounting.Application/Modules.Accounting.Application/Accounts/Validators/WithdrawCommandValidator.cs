using FluentValidation;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Application.Accounts.Commands;

namespace Modules.Accounting.Application.Accounts.Validators;

public class WithdrawCommandValidator : AbstractValidator<WithdrawCommand>
{
    public WithdrawCommandValidator(IStringLocalizer<WithdrawCommand> localizer)
    {
        RuleFor(x => x.AccountId)
            .NotEqual(Guid.Empty)
            .WithMessage(_ => localizer["The {PropertyName} property cannot be empty."]);
        RuleFor(x => x.Amount).NotEqual(0)
            .WithMessage(_ => localizer["The {PropertyName} property cannot be zero."]);
        RuleFor(x => x.Amount).GreaterThan(0)
            .WithMessage(_ => localizer["The {PropertyName} property cannot be negative."]);
    }
}