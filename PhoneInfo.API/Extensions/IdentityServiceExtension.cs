using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PhoneInfo.API.Domain.Jwt.Service;
using System;
using System.Text;

namespace PhoneInfo.API.Extensions
{
	public static class IdentityServiceExtension
	{
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
	}
}
