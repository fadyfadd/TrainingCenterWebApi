namespace TrainingCenterWebApi.Infrastructure
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class MyCustomMiddleware
    {
        private readonly RequestDelegate _next;

         public MyCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

         public async Task InvokeAsync(HttpContext context)
        {            
            await _next(context);         
        }
    }

}
