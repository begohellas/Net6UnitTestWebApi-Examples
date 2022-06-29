using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Net6UnitTestWebApi.API.Filter;

public class FilterGlobalException : IExceptionFilter
{
    private readonly ILogger<FilterGlobalException> _logger;

    public FilterGlobalException(ILogger<FilterGlobalException> logger)
    {
        _logger = logger;
    }
    
    public void OnException(ExceptionContext context)
    {
        if (!context.ExceptionHandled)
        {
            var exception = context.Exception;

            var statusCode = exception switch
            {
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            _logger.LogError(exception, "GlobalExceptionFilter: Error in {DisplayName}", context.ActionDescriptor.DisplayName);

            context.Result = new ObjectResult(exception.Message)
            {
                StatusCode = statusCode
            };
        }
    }
}