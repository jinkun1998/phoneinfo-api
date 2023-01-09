using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PhoneInfo.API.Core.HttpRequest;
using PhoneInfo.API.Core.Jwt.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneInfo.API.Core
{
	public static class ServicesRegisteration
	{
		#region Session

		public static void AddSessionServiceLayer(this IServiceCollection services)
		{
			services.AddDistributedMemoryCache();
			services.AddSession();
		}

		#endregion Session

		#region Jwt

		public static void AddJwtServiceLayer(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddTransient<IJwtService, JwtService>();
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
					{
						ValidIssuer = configuration["Jwt:Issuer"],
						ValidAudience = configuration["Jwt:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
						ValidateIssuerSigningKey = true,
						ValidateIssuer = true,
						ValidateAudience = true,
						ClockSkew = TimeSpan.Zero
					};
				});
		}

		public static void UseJwtService(this IApplicationBuilder app)
		{
			app.UseAuthentication();
		}

		#endregion Jwt

		#region Swagger

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

		#endregion Swagger

		#region HttpService

		public static void AddHttpServiceLayer(this IServiceCollection services) => services.AddScoped<IHttpService, HttpService>();

		#endregion HttpService
	}
}