using Social.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Social.Web.Filters;

public class ValidationFilter:IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .Select(x => x.Errors.FirstOrDefault()?.ErrorMessage);
            throw new ValidationException("Validation Exception",errors);
        }
        await next();
    }
}