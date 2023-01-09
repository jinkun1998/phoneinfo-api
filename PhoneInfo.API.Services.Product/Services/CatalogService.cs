using Microsoft.Extensions.Configuration;
using PhoneInfo.API.Core.HttpRequest;
using System.Net;

namespace PhoneInfo.API.Services.Product.Services
{
	public interface ICatalogService
	{
		Task<object> GetBrandsAsync();

		Task<object> GetProductByBrandAsync(string brand);

		Task<object> GetProductDetailAsync(string product);
	}

	public class CatalogService : ICatalogService
	{
		private readonly IHttpService HttpService;
		private readonly IConfiguration Configuration;
		private const string BASE_ENDPOINT = "Endpoint:Base";

		public CatalogService(IHttpService httpService, IConfiguration configuration)
		{
			HttpService = httpService;
			Configuration = configuration;
		}

		public async Task<object> GetBrandsAsync()
		{
			string endpoint = $"{Configuration[BASE_ENDPOINT]}/makers.php3";
			(string content, HttpStatusCode statusCode) = await HttpService
				.SendRequestWithStringContentAsync<string>(HttpMethod.Get, endpoint);
			return content;
		}

		public async Task<object> GetProductByBrandAsync(string brand)
		{
			string endpoint = $"{Configuration[BASE_ENDPOINT]}/{brand}.php";
			(string content, HttpStatusCode statusCode) = await HttpService
				.SendRequestWithStringContentAsync<string>(HttpMethod.Get, endpoint);
			return content;
		}

		public async Task<object> GetProductDetailAsync(string product)
		{
			string endpoint = $"{Configuration[BASE_ENDPOINT]}/{product}.php";
			(string content, HttpStatusCode statusCode) = await HttpService
				.SendRequestWithStringContentAsync<string>(HttpMethod.Get, endpoint);
			return content;
		}
	}
}