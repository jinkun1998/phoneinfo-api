using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneInfo.API.Core.Middlewares
{
	public class TokenMiddleware
	{
		private readonly RequestDelegate _next;

		public TokenMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			if (!string.IsNullOrEmpty(context.Request.Headers["Authentication"])
				&& context.Request.Headers["Authentication"].Contains("Bearer"))
			{
				if (!string.IsNullOrEmpty(context.Request.Headers["Authentication"].ToString()[7..]))
				{
					await _next(context);
				}
			}

			//context.Response.ContentType = "application/json";
			//context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			//await context.Response.WriteAsync(JsonConvert.SerializeObject(new BaseResponseModel(context.Response.StatusCode, "Invalid token or token is expired", null)));
		}
	}
}