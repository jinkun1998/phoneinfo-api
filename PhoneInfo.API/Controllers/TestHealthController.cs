using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhoneInfo.API.Controllers
{
    [AllowAnonymous]
    public class TestHealthController : Controller
    {
        [HttpGet("testhealth")]
        public IActionResult Index()
        {
            return Ok(DateTime.Now);
        }
    }
}

