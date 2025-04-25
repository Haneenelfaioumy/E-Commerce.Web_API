using System.Net;
using System.Text.Json;
using Shared.ErrorModels;

namespace E_Commerce.Web.CustomMiddleWares
{
    public class CustomExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleWare> _logger;

        public CustomExceptionHandlerMiddleWare(RequestDelegate Next , ILogger<CustomExceptionHandlerMiddleWare> logger) 
        {
            _next = Next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext) 
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something Went Wrong");

                // Set Status Code For Response
                //httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                //// Set Content Type For Response
                //httpContext.Response.ContentType = "application/json";

                // Response Object
                var Response = new ErrorToReturn()
                {
                    StatusCode = StatusCodes.Status500InternalServerError ,
                    ErrorMessage = ex.Message
                };

                // Return Object As JSON
                await httpContext.Response.WriteAsJsonAsync(Response);
            }
        }
    }
}
