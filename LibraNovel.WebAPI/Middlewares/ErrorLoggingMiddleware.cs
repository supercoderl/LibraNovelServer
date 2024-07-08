using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using System.Text.Json;
using LibraNovel.Application.Wrappers;
using LibraNovel.Application.Exceptions;

namespace LibraNovel.WebAPI.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {RequestCode}";
        static readonly Serilog.ILogger Log = Serilog.Log.ForContext<ErrorLoggingMiddleware>();
        private readonly RequestDelegate _next;

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                var request = httpContext.Request;

                var log = Log
                    .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                    .ForContext("RequestHost", request.Host)
                    .ForContext("RequestProtocol", request.Protocol);

                if (request.HasFormContentType)
                    log = log.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));

                log.Error(e, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, 500);

                var response = httpContext.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false, Message = e?.Message };

                switch (e)
                {
                    case Application.Exceptions.ApiException ex:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ValidationException ex:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = ex.Errors;
                        break;
                    case KeyNotFoundException ex:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
        }
    }
}
