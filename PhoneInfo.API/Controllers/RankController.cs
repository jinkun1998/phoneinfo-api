using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PhoneInfo.API.Application.Services;
using PhoneInfo.API.Domain.Bases;
using PhoneInfo.API.Domain.Interfaces;
using PhoneInfo.API.Helpers;
using System.Net;
using System.Threading.Tasks;

namespace PhoneInfo.API.Controllers
{
	[AllowAnonymous]
	[ApiVersion("1.0")]
	[Route("api/ranks")]
	[Route("api/v{version:apiVersion}/ranks")]
	public class RankController : BaseController
	{
		private readonly IDealService DealService;

		public RankController(IDealService dealService, IDistributedCache cache) : base(cache)
		{
			DealService = dealService;
		}

		[HttpGet("dailyInterest")]
		public async Task<IActionResult> GetTop10DailyInterest()
		{
			(string response, HttpStatusCode statusCode) = await DealService.GetDealsAsync();
			return statusCode is not HttpStatusCode.OK?
				ResponseBadRequest(null, "Error", (int)statusCode):
				ResponseOK(DealParser.Parse(response, DealParser.DealType.Top10DailyInterest));
		}

		[HttpGet("byFans")]
		public async Task<IActionResult> GetTop10ByFans()
		{
			(string response, HttpStatusCode statusCode) = await DealService.GetDealsAsync();
			return statusCode is not HttpStatusCode.OK ?
				ResponseBadRequest(null, "Error", (int)statusCode) :
				ResponseOK(DealParser.Parse(response, DealParser.DealType.Top10ByFans));
		}
	}
}
