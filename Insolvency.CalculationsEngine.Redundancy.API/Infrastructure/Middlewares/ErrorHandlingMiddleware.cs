using System;
using System.Net;
using System.Threading.Tasks;
using Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError((int)System.Net.HttpStatusCode.InternalServerError, e, e.Message, e.StackTrace);
                await HandleExceptionAsync(context, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // if it's not one of the expected exception, set it to 500
            var code = HttpStatusCode.InternalServerError;

            return WriteExceptionAsync(context, exception, code);
        }

        private Task WriteExceptionAsync(HttpContext context, Exception exception, HttpStatusCode code)
        {
            var response = context.Response;
            var errorResponseModel = new ErrorResponseModel();

            if (code == HttpStatusCode.InternalServerError)
            {
                errorResponseModel.Code = (int)System.Net.HttpStatusCode.InternalServerError;
                errorResponseModel.Message = "\"An unexpected fault happened. Try again later.\"";
                errorResponseModel.Exception = "Internal Server Error";
            }

            response.ContentType = "application/json";
            response.StatusCode = (int) code;
            return response.WriteAsync(JsonConvert.SerializeObject(errorResponseModel));
        }
    }
}