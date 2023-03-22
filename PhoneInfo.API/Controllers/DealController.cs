using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PhoneInfo.API.Application.Services;
using PhoneInfo.API.Domain.Bases;
using PhoneInfo.API.Helpers;
using System.Net;
using System.Threading.Tasks;

namespace PhoneInfo.API.Controllers
{
	[AllowAnonymous]
	[ApiVersion("1.0")]
	[Route("api")]
	[Route("api/v{version:apiVersion}")]
	public class DealController : BaseController
	{
		private readonly IDealService DealService;

		public DealController(IDealService dealService, IDistributedCache cache) : base(cache)
		{
			DealService = dealService;
		}


		[HttpGet("deals")]
		public async Task<IActionResult> GetDeals()
		{
			(string html, HttpStatusCode statusCode) = await DealService.GetDealsAsync();
			return statusCode != HttpStatusCode.OK ?
				ResponseBadRequest(null, "Error", (int)statusCode) :
				ResponseOK(DealParser.Parse(html, DealParser.Type.Deals));
		}
	}
}
