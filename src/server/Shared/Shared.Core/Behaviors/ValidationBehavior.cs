using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using Shared.Core.Exceptions;

namespace Shared.Core.Behaviors
{
    public class ValidationBehavior
    {
    }

    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IStringLocalizer<ValidationBehavior> _localizer;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, IStringLocalizer<ValidationBehavior> localizer)
        {
            _validators = validators;
            _localizer = localizer;
        }

#pragma warning disable RCS1046
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
#pragma warning restore RCS1046

        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    var errorMessages = failures.Select(a => a.ErrorMessage).Distinct().ToList();
                    throw new CustomValidationException(_localizer, errorMessages);
                }
            }

            return await next();
        }
    }
}