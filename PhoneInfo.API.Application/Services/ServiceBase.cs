using Microsoft.Extensions.Configuration;
using PhoneInfo.API.Domain.HttpRequest;
using System.Net;

namespace PhoneInfo.API.Application.Services
{
	public abstract class ServiceBase
	{
		protected const string BASE_ENDPOINT = "Endpoint:Base";
		protected IConfiguration Configuration;
		protected IHttpService HttpService;
		protected ServiceBase(IHttpService httpService, IConfiguration configuration)
		{
			HttpService = httpService;
			Configuration = configuration;
		}

		protected async Task<(string, HttpStatusCode)> GetResultAsync(string url, HttpMethod httpMethod)
		{
			string endpoint = $"{Configuration[BASE_ENDPOINT]}/{url}";
			return await HttpService
				.SendRequestWithStringContentAsync<string>(httpMethod, endpoint);
		}
	}
}