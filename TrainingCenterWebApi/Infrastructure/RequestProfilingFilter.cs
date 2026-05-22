namespace TrainingCenterWebApi.Infrastructure
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Diagnostics;

    public class RequestProfilingFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            context.HttpContext.Items["ActionStopwatch"] = stopwatch;
            string actionName = context.ActionDescriptor.DisplayName;
            Console.WriteLine($"[Filter] Starting execution of: {actionName}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;

            if (context.HttpContext.Items["ActionStopwatch"] is Stopwatch stopwatch)
            {
                stopwatch.Stop();
                Console.WriteLine($"[Filter] Finished: {actionName} | Taken: {stopwatch.ElapsedMilliseconds}ms");
            }

            if (context.Exception != null)
            {
                Console.WriteLine($"[Filter] Exception caught in action: {context.Exception.Message}");
            }
        }
    }

}
