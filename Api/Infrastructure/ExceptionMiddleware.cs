namespace Api.Infrastructure
{
    using Data.Exceptions;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        
        
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger; 
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (ex is BusinessException businessException)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new { ServerCode = businessException.ServerCode, Message = businessException.Message, Errors = businessException.Errors});

                    if (ex.InnerException != null) {
                        _logger.LogError(ex.InnerException , ex.Message);
                    }
                }
                else
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new { Message = "An error occurred while processing your request." });
                    _logger.LogError(ex , ex.Message);
                }

            }

        }
    }

}
