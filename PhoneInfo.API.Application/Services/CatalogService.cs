using Microsoft.Extensions.Configuration;
using PhoneInfo.API.Domain.HttpRequest;
using System.Net;

namespace PhoneInfo.API.Application.Services
{
    public interface ICatalogService
    {
        Task<(string content, HttpStatusCode statusCode)> GetBrandsAsync();
        Task<(string content, HttpStatusCode statusCode)> GetProductByBrandAsync(string brand);
        Task<(string content, HttpStatusCode statusCode)> GetProductDetailAsync(string product);
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

        public async Task<(string content, HttpStatusCode statusCode)> GetBrandsAsync()
        {
            string endpoint = $"{Configuration[BASE_ENDPOINT]}/makers.php3";
            return await HttpService
                .SendRequestWithStringContentAsync<string>(HttpMethod.Get, endpoint);
        }

        public async Task<(string content, HttpStatusCode statusCode)> GetProductByBrandAsync(string brand)
        {
            string endpoint = $"{Configuration[BASE_ENDPOINT]}/{brand}.php";
            return await HttpService
                .SendRequestWithStringContentAsync<string>(HttpMethod.Get, endpoint);
        }

        public async Task<(string content, HttpStatusCode statusCode)> GetProductDetailAsync(string product)
        {
            string endpoint = $"{Configuration[BASE_ENDPOINT]}/{product}.php";
            return await HttpService
                .SendRequestWithStringContentAsync<string>(HttpMethod.Get, endpoint);
        }
    }
}