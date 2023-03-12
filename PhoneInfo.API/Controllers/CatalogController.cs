using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneInfo.API.Application.Services;
using PhoneInfo.API.Domain.Bases;
using PhoneInfo.API.Helpers;
using System.Net;
using System.Threading.Tasks;

namespace PhoneInfo.API.Controllers
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
            (string html, HttpStatusCode statusCode) = await CatalogService.GetBrandsAsync();
            return statusCode is not HttpStatusCode.OK ?
                ResponseBadRequest(null, "Error", (int)statusCode) :
                ResponseOK(CatalogParser.Parse(html, CatalogParser.Type.BRANCHS));
        }

        [HttpGet("productByBrand")]
        public async Task<IActionResult> GetProductByBrand(string brand)
        {
            (string html, HttpStatusCode statusCode) = await CatalogService.GetProductByBrandAsync(brand);
            return statusCode is not HttpStatusCode.OK ?
                ResponseBadRequest(null, "Error", (int)statusCode) :
                ResponseOK(CatalogParser.Parse(html, CatalogParser.Type.BRANCH));
        }

        [HttpGet("productDetail")]
        public async Task<IActionResult> GetProductDetail(string product)
        {
            (string html, HttpStatusCode statusCode) = await CatalogService.GetProductDetailAsync(product);
            return statusCode is not HttpStatusCode.OK ?
                ResponseBadRequest(null, "Error", (int)statusCode) :
                ResponseOK(CatalogParser.Parse(html, CatalogParser.Type.DEVICE));
        }
    }
}