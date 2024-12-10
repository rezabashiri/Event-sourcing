using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Modules.EmployeeManagement.Core.Features.Staff.Commands.Validators;
public class EmployStaffCommandValidator : AbstractValidator<EmployStaffCommand>
{
    public EmployStaffCommandValidator(IStringLocalizer<EmployStaffCommandValidator> localizer)
    {
        RuleFor(command => command.Name).NotEmpty().NotNull().WithMessage(x => localizer["The {PropertyName} property cannot be empty or null."]);

        RuleFor(command => command.NationalCode).NotEmpty().NotNull().WithMessage(x =>
            localizer["The {PropertyName} property cannot be empty or null."]);

        RuleFor(command => command.Family).NotEmpty().NotNull().WithMessage(x =>
            localizer["The {PropertyName} property cannot be empty or null."]);
    }
}