using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneInfo.API.Core.Bases;
using PhoneInfo.API.Services.Product.Services;
using System.Threading.Tasks;

namespace PhoneInfo.API.Services.Product.Controllers
{
	[AllowAnonymous]
	[Route("api/catalog")]
	public class CatalogController : BaseController
	{
		private readonly ICatalogService CatalogService;

		public CatalogController(ICatalogService catalogService)
		{
			CatalogService = catalogService;
		}

		[HttpGet("brands")]
		public async Task<IActionResult> GetAllBrands()
		{
			return ResponseOK(await CatalogService.GetBrandsAsync());
		}

		[HttpGet("productByBrand")]
		public async Task<IActionResult> GetProductByBrand(string brand)
		{
			return ResponseOK(await CatalogService.GetProductByBrandAsync(brand));
		}

		[HttpGet("productDetail")]
		public async Task<IActionResult> GetProductDetail(string product)
		{
			return ResponseOK(await CatalogService.GetProductDetailAsync(product));
		}
	}
}