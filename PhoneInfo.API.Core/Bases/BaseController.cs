using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PhoneInfo.API.Core.Jwt.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneInfo.API.Core.Bases
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult ResponseOK(object data = null, string message = "")
        {
            return Ok(new BaseResponseModel(200, message, data));
        }

        protected IActionResult ResponseBadRequest(object data = null, string message = "", int errorCode = 400)
        {
            return Ok(new BaseResponseModel(errorCode, message, data));
        }

        protected void SetCacheToken(IDistributedCache cache, JwtResponseModel jwtResponseModel)
        {
            cache.SetString($"Token_{jwtResponseModel.AccessToken}", jwtResponseModel.RefreshToken);

        }

        protected string GetRefreshToken(IDistributedCache cache, string accessToken)
        {
            return cache.GetString($"Token_{accessToken}");
        }
    }
}
