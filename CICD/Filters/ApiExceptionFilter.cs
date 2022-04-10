using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.Json;

namespace CICD.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ApiExceptionFilter> _logger;
        private readonly Dictionary<Type, HttpStatusCode> _results;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            this._logger = logger;

            this._results = new Dictionary<Type, HttpStatusCode>
            {
                { typeof(BO.CustomExceptions.NotFoundException), HttpStatusCode.NotFound },
                { typeof(BO.CustomExceptions.ConflictException), HttpStatusCode.Conflict },
                { typeof(BO.CustomExceptions.BadRequestException), HttpStatusCode.BadRequest },
                { typeof(BO.CustomExceptions.ApiCallerException), HttpStatusCode.InternalServerError },
                { typeof(BO.CustomExceptions.UnexpectedException), HttpStatusCode.InternalServerError },
            };
        }

        public override void OnException(ExceptionContext context)
        {
            var errorData = new BO.ErrorData
            {
                Message = context.Exception.Message,
                StackTrace = context.Exception.StackTrace ?? string.Empty,
            };
            context.HttpContext.Response.StatusCode = 500;

            if (context.Exception.GetType().BaseType == typeof(BO.CustomExceptions.CustomExceptionBase))
            {
                context.HttpContext.Response.StatusCode = (int)this._results[context.Exception.GetType()];
                errorData = ((BO.CustomExceptions.CustomExceptionBase)context.Exception).ErrorData;
            }

            context.Result = new JsonResult(errorData);

            string errorMessage = $"Response code: {context.HttpContext.Response.StatusCode}; Body: {JsonSerializer.Serialize(errorData)}";
            this._logger.LogError(errorMessage);

            base.OnException(context);
        }
    }
}
