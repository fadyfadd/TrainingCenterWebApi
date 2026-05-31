namespace TrainingCenterWebApi.Infrastructure
{
    using DataAccessLayer.Exceptions;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
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
                    await context.Response.WriteAsJsonAsync(new { ServerCode = businessException.ServerCode, Message = businessException.Message, Errors = businessException.Errors });

                    if (ex.InnerException != null)
                    {
                        // Logging the inner exception details for debugging purposes
                    }

                }
                else
                {
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                    {
                        //Loggin the exception details for debugging purposes
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsJsonAsync(new { Message = ex.Message, StackTrace = ex.StackTrace });
                    }
                    else
                    {   //Loggin the exception details for debugging purposes
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsJsonAsync(new { Message = "An error occurred while processing your request." });
                    }

                }

            }

        }
    }

}
