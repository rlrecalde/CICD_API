using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace CICD.Filters
{
    public class ApiActionFilter : ActionFilterAttribute
    {
        private readonly ILogger<ApiActionFilter> _logger;

        public ApiActionFilter(ILogger<ApiActionFilter> logger)
        {
            this._logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            HttpRequest httpRequest = context.HttpContext.Request;
            string request = $"Method: {httpRequest.Method}; Path: {httpRequest.Path}; Action: {httpRequest.RouteValues["action"]}";

            foreach (var argument in context.ActionArguments)
                request += $"; Body: {argument.Key} = {JsonSerializer.Serialize(argument.Value)};";

            this._logger.LogInformation(request);

            base.OnActionExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            string response = $"Response code: {context.HttpContext.Response.StatusCode}";

            if (context.Result.GetType().BaseType == typeof(ObjectResult))
            {
                var objectResult = (ObjectResult)context.Result;

                if (objectResult.Value != null)
                    response += $"; Body: {objectResult.Value.GetType().Name} = {JsonSerializer.Serialize(objectResult.Value)}";
            }

            this._logger.LogInformation(response);

            base.OnResultExecuted(context);
        }
    }
}
