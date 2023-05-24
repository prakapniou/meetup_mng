namespace MeetupManager.Web.Configuration;

/// <summary>
/// 
/// </summary>
public class ExceptionHandlerMiddleware:IMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
    {
        _logger= logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }

        catch (Exception exception)
        {
            _logger.LogError(message: exception.Message);
            await ExceptionHandleAsync(context, exception);
        }
    }

    private static Task ExceptionHandleAsync(HttpContext context, Exception exception)
    {
        ErrorDetails details = new();
        var response = context.Response;
        response.ContentType= "application/json";

        response.StatusCode=exception switch
        {                
            InvalidOperationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError,
        };

        details.Message = exception.Message;
        details.StackTrace=exception.StackTrace!;
        return response.WriteAsync(details.ToString());
    }
}
