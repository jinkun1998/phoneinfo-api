using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PhoneInfo.API.Domain.Jwt.Response;
using System.Text;

namespace PhoneInfo.API.Domain.Bases
{
	[ApiController]
	public class BaseController : ControllerBase
	{
		private readonly IDistributedCache Cache;

		public BaseController(IDistributedCache cache)
		{
			Cache = cache;
		}
		protected IActionResult ResponseOK(object data = null, string message = "")
		{
			return Ok(new BaseResponseModel(200, message, data));
		}

		protected IActionResult ResponseBadRequest(object data = null, string message = "", int errorCode = 400)
		{
			return Ok(new BaseResponseModel(errorCode, message, data));
		}

		protected void SetCacheToken(JwtResponseModel jwtResponseModel)
		{
			Cache.SetString($"Token_{jwtResponseModel.AccessToken}", jwtResponseModel.RefreshToken);
		}

		protected string GetRefreshToken(string accessToken)
		{
			return Cache.GetString($"Token_{accessToken}");
		}
	}
}