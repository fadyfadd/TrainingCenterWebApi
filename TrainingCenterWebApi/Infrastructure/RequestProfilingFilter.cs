namespace Api.Infrastructure
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Diagnostics;

    public class RequestProfilingFilter : IActionFilter
    {
        
        ILogger<RequestProfilingFilter> logger;

        public RequestProfilingFilter(ILogger<RequestProfilingFilter> logger)
        {
            this.logger = logger;
        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            context.HttpContext.Items["ActionStopwatch"] = stopwatch;
            string actionName = context.ActionDescriptor.DisplayName;
            logger.LogDebug($"[Filter] Starting execution of: {actionName}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;

            if (context.HttpContext.Items["ActionStopwatch"] is Stopwatch stopwatch)
            {
                stopwatch.Stop();
                //Console.WriteLine($"[Filter] Finished: {actionName} | Taken: {stopwatch.ElapsedMilliseconds}ms");
                logger.LogDebug($"[Filter] Finished: {actionName} | Taken: {stopwatch.ElapsedMilliseconds}ms");
            }

            if (context.Exception != null)
            {
                logger.LogDebug($"[Filter] Exception caught in action: {context.Exception.Message}");
            }
        }
    }

}
