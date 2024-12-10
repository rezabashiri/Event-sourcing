using FluentValidation;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Application.Accounts.Commands;

namespace Modules.Accounting.Application.Accounts.Validators;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator(IStringLocalizer<CreateAccountCommand> localizer)
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage(_ => localizer["The {PropertyName} property cannot be empty or null."]);
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage(_ => localizer["The {PropertyName} property cannot be empty."]);
        RuleFor(command => command.InitialDeposit)
            .NotEqual(0)
            .WithMessage(_ => localizer["The {PropertyName} property cannot be zero."]);
    }
}