using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace conservation_backend.Shared
{
    public class GlobalValidationFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public GlobalValidationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                var validatorType = typeof(IValidator<>).MakeGenericType(argument!.GetType());
                var validator = _serviceProvider.GetService(validatorType) as IValidator;

                if (validator != null)
                {
                    var validationResult = await validator.ValidateAsync(
                        new ValidationContext<object>(argument));

                    if (!validationResult.IsValid)
                    {
                        foreach (var error in validationResult.Errors)
                            context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                        //context.Result = new BadRequestObjectResult(new
                        //{
                          //  success = false,
                           // message = validationResult.Errors.First().ErrorMessage, // first error only
                           // errors = context.ModelState
                             //   .Where(x => x.Value!.Errors.Count > 0)
                               // .ToDictionary(
                                 //   kvp => kvp.Key,
                                   // kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                                //)
                        // });
                        
                        throw new BadHttpRequestException(
                            validationResult.Errors.First().ErrorMessage,
                            StatusCodes.Status400BadRequest
                        );
                        
                    }
                }
            }

            await next();
        }
    }
}

