using Social.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Social.Web.Filters;

public class ExceptionFilter:IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        var exception = context.Exception;
        int? statusCode = exception switch
        {
            ForbiddenException => 403,
            ValidationException => 400,
            NotFoundException => 404,
            ConflictException => 409,
            AppException => 500,
            _ => null
        };
        
        if (statusCode != null)
        {
            if (exception is ValidationException validationException)
            {
                context.Result = new JsonResult(new {message = "One or more validation errors occured", errors = validationException.Errors})
                {
                    StatusCode = statusCode
                };
            }
            else
            {
                context.Result = new JsonResult(new {message = exception.Message})
                {
                    StatusCode = statusCode
                };
            }
            
            context.ExceptionHandled = true;
        }
       
        return Task.CompletedTask;
    }
}