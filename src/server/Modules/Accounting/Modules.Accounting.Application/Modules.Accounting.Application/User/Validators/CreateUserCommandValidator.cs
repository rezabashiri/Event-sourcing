using FluentValidation;
using Microsoft.Extensions.Localization;
using Modules.Accounting.Application.User.Commands;

namespace Modules.Accounting.Application.User.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IStringLocalizer<CreateUserCommand> localizer)
    {
        RuleFor(command => command.FullName).NotEmpty().NotNull()
            .WithMessage(_ => localizer["The {PropertyName} property cannot be empty or null."]);
    }
}