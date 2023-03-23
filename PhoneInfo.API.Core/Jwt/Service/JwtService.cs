using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PhoneInfo.API.Domain.Jwt.Response;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PhoneInfo.API.Domain.Jwt.Service
{
	public enum TokenType
	{
		AccessToken,
		RefreshToken
	}

	public interface IJwtService
	{
		(bool, JwtResponseModel) GenerateToken(string username);

		(bool, string) ValidateToken(string tokenString, TokenType tokenType);
	}

	public class JwtService : IJwtService
	{
		private readonly IConfiguration _configuration;
		public IDistributedCache _cache { get; }

		public JwtService(IConfiguration configuration, IDistributedCache cache)
		{
			_configuration = configuration;
			_cache = cache;
		}

		public (bool, JwtResponseModel) GenerateToken(string username)
		{
			try
			{
				string accessToken = BuildToken(username, TokenType.AccessToken);
				string refreshToken = BuildToken(username, TokenType.RefreshToken);
				return (true, new JwtResponseModel() { AccessToken = accessToken, RefreshToken = refreshToken, ResponseMessage = "SUCCESS" });
			}
			catch (Exception ex)
			{
				return (false, new JwtResponseModel() { ResponseMessage = ex.Message });
			}
		}

		public (bool, string) ValidateToken(string tokenString, TokenType tokenType)
		{
			string username = string.Empty;
			JwtSecurityTokenHandler handle = new();
			if (!handle.CanReadToken(tokenString))
			{
				return (false, "Can not read token.");
			}

			JwtSecurityToken jwt = handle.ReadJwtToken(tokenString);
			if (jwt.ValidTo < DateTime.Now)
			{
				return (false, "Token is expired.");
			}

			try
			{
				handle.ValidateToken(tokenString, new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = _configuration["Jwt:Issuer"],
					ValidAudience = _configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]))
				}, out SecurityToken securityToken);
			}
			catch (Exception ex)
			{
				return (false, ex.Message);
			}

			foreach (Claim claim in jwt.Claims)
			{
				if (claim.Type == "Username")
				{
					username = claim.Value;
				}

				if (claim.Type == "Type")
				{
					if (claim.Value != tokenType.ToString())
					{
						return (false, "Token type is not valid");
					}
				}
			}

			return (true, username);
		}

		#region Additional Logics

		private DateTime GetExpired(TokenType tokenType)
		{
			return tokenType == TokenType.AccessToken ? DateTime.Now.AddMinutes(30) : DateTime.Now.AddDays(1);
		}

		private string BuildToken(string username, TokenType tokenType)
		{
			Claim[] claims = new[]
			{
				new Claim("Username", username), // username
                new Claim("Type", tokenType.ToString()), // token type
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()), // token isssued time
                new Claim(JwtRegisteredClaimNames.Exp, GetExpired(tokenType).ToString())
			};

			SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
			SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
			JwtSecurityToken token = new(
				_configuration["Jwt:Issuer"],
				_configuration["Jwt:Audience"],
				claims,
				DateTime.Now,
				GetExpired(tokenType),
				creds);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		#endregion Additional Logics
	}
}