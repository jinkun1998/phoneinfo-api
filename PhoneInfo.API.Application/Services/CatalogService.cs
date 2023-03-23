using Microsoft.Extensions.Configuration;
using PhoneInfo.API.Domain.HttpRequest;
using PhoneInfo.API.Domain.Interfaces;
using System.Net;

namespace PhoneInfo.API.Application.Services
{
	public class CatalogService : ServiceBase, ICatalogService
	{
		public CatalogService(IHttpService httpService, IConfiguration configuration) : base(httpService, configuration)
		{
		}

		public async Task<(string content, HttpStatusCode statusCode)> GetBrandsAsync()
		{
			return await GetResultAsync("makers.php3", HttpMethod.Get);
		}

		public async Task<(string content, HttpStatusCode statusCode)> GetProductByBrandAsync(string brand)
		{
			return await GetResultAsync($"{brand}.php", HttpMethod.Get);
		}

		public async Task<(string content, HttpStatusCode statusCode)> GetProductDetailAsync(string product)
		{
			return await GetResultAsync($"{product}.php", HttpMethod.Get);
		}
	}
}