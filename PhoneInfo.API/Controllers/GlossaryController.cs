using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PhoneInfo.API.Domain.Bases;
using PhoneInfo.API.Domain.Interfaces;
using PhoneInfo.API.Helpers;
using System.Net;
using System.Threading.Tasks;

namespace PhoneInfo.API.Controllers
{
	[AllowAnonymous]
	[ApiVersion("1.0")]
	[Route("api/glossary")]
	[Route("api/v{version:apiVersion}/glossary")]
	public class GlossaryController : BaseController
	{
		private readonly IGlossaryService GlossaryService;

		public GlossaryController(IGlossaryService glossaryService, IDistributedCache cache) : base(cache)
		{
			GlossaryService = glossaryService;
		}

		[HttpGet("list")]
		public async Task<IActionResult> GetGlossaries()
		{
			(string response, HttpStatusCode statusCode) = await GlossaryService.GetGlossaries();
			return statusCode is not HttpStatusCode.OK ?
				ResponseBadRequest(null, "Error", (int)statusCode) :
				ResponseOK(GlossaryParser.Parse(response, GlossaryParser.GlossaryType.LIST));
		}

		[HttpGet("term")]
		public async Task<IActionResult> GetTerm([FromQuery] string id)
		{
			(string response, HttpStatusCode statusCode) = await GlossaryService.GetTerm(id);
			return statusCode is not HttpStatusCode.OK ?
				ResponseBadRequest(null, "Error", (int)statusCode) :
				ResponseOK(GlossaryParser.Parse(response, GlossaryParser.GlossaryType.TERM));
		}
	}
}
