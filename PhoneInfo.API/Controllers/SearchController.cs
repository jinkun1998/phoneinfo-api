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
	[Route("api")]
	[Route("api/v{version:apiVersion}")]
	public class SearchController : BaseController
	{
		private readonly ISearchService SearchService;

		public SearchController(ISearchService searchService, IDistributedCache cache) : base(cache)
		{
			SearchService = searchService;
		}

		[HttpGet("search")]
		public async Task<IActionResult> SearchAll([FromQuery] string keyword)
		{
			(string response, HttpStatusCode statusCode) = await SearchService.SearchAll(keyword);
			return statusCode is not HttpStatusCode.OK ?
				ResponseBadRequest(null, "Error", (int)statusCode) :
				ResponseOK(SearchParser.Parse(response, SearchParser.SearchType.ALL));
		}
	}
}
