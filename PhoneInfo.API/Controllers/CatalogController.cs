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
	[Route("api/catalogs")]
	[Route("api/v{version:apiVersion}/catalogs")]
	public class CatalogController : BaseController
	{
		private readonly ICatalogService CatalogService;

		public CatalogController(ICatalogService catalogService, IDistributedCache cache) : base(cache)
		{
			CatalogService = catalogService;
		}

		[HttpGet("brands")]
		public async Task<IActionResult> GetAllBrands()
		{
			(string html, HttpStatusCode statusCode) = await CatalogService.GetBrandsAsync();
			return statusCode is not HttpStatusCode.OK ?
				ResponseBadRequest(null, "Error", (int)statusCode) :
				ResponseOK(CatalogParser.Parse(html, CatalogParser.CatalogType.BRANCHS));
		}

		[HttpGet("productByBrand")]
		public async Task<IActionResult> GetProductByBrand([FromQuery] string brand)
		{
			(string html, HttpStatusCode statusCode) = await CatalogService.GetProductByBrandAsync(brand);
			return statusCode is not HttpStatusCode.OK ?
				ResponseBadRequest(null, "Error", (int)statusCode) :
				ResponseOK(CatalogParser.Parse(html, CatalogParser.CatalogType.BRANCH));
		}

		[HttpGet("productDetail")]
		public async Task<IActionResult> GetProductDetail([FromQuery] string product)
		{
			(string html, HttpStatusCode statusCode) = await CatalogService.GetProductDetailAsync(product);
			return statusCode is not HttpStatusCode.OK ?
				ResponseBadRequest(null, "Error", (int)statusCode) :
				ResponseOK(CatalogParser.Parse(html, CatalogParser.CatalogType.DEVICE));
		}
	}
}