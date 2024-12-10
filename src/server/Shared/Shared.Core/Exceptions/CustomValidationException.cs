using Microsoft.Extensions.Localization;
using System.Net;

namespace Shared.Core.Exceptions
{
    public class CustomValidationException(IStringLocalizer localizer, List<string> errors)
        : CustomException(localizer["One or more validation failures have occurred."], errors,
            HttpStatusCode.BadRequest);
}