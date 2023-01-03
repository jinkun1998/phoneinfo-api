using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PhoneInfo.API.Core.Bases;
using System;
using System.Threading.Tasks;

namespace PhoneInfo.API.Core.Middlewares
{
    public class MainMiddleware
    {
        private readonly RequestDelegate _next;

        public MainMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new BaseResponseModel(context.Response.StatusCode, ex.Message, null)));
            }
        }
    }
}
