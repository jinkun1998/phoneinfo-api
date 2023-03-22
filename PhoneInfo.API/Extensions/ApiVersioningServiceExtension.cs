using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using PhoneInfo.API.Domain.Bases;
using System.Net;

namespace PhoneInfo.API.Extensions
{
	public static class ApiVersioningServiceExtension
	{
		public static void AddApiVersioningLayer(this IServiceCollection services)
		{
			services.AddApiVersioning(options =>
			{
				// define default version is 1.0
				options.DefaultApiVersion = new(1, 0);
				// add 'api-supported-versions' to response header
				options.ReportApiVersions = true;
				// auto get the default version if clents do not send the version
				// you need to config the sencond route without version in controllers
				options.AssumeDefaultVersionWhenUnspecified = true;
				// custom error response
				options.ErrorResponses = new CustomErrorResponseProvider();
			});

			services.AddEndpointsApiExplorer();
		}

		public class CustomErrorResponseProvider : DefaultErrorResponseProvider
		{
			public override IActionResult CreateResponse(ErrorResponseContext context)
			{
				JsonResult response = new(new BaseResponseModel(405, "The HTTP resource does not support the API version", new()))
				{
					StatusCode = (int)HttpStatusCode.BadRequest
				};
				return response;
			}
		}
	}
}