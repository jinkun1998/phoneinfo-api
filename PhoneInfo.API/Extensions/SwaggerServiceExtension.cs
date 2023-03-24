using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace PhoneInfo.API.Extensions
{
	public static class SwaggerServiceExtension
	{
		public static void AddSwaggerServiceLayer(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo()
				{
					Title = "Phone Info API",
					Version = "v1"
				});
				options.AddSecurityDefinition("Phone Info API v1", new OpenApiSecurityScheme()
				{
					BearerFormat = "JWT",
					Name = "Phone Info API v1",
					In = ParameterLocation.Header,
					Scheme = "Bearer",
					Type = SecuritySchemeType.Http,
					Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme()
						{
							Reference = new OpenApiReference()
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						}, new List<string>()
					}
				});
			});
		}

		public static void UseSwaggerService(this IApplicationBuilder app, IHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Phone Info API V1"));
			}
		}
	}
}
