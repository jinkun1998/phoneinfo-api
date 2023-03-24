using Microsoft.Extensions.Configuration;
using PhoneInfo.API.Domain.HttpRequest;
using PhoneInfo.API.Domain.Interfaces;
using System.Net;

namespace PhoneInfo.API.Application.Services
{
	public class SearchService : ServiceBase, ISearchService
	{
		public SearchService(IHttpService httpService, IConfiguration configuration) : base(httpService, configuration)
		{
		}

		public async Task<(string, HttpStatusCode)> SearchAll(string keyword)
		{
			return await GetResultAsync($"results.php3?sQuickSearch=yes&sName={keyword}", HttpMethod.Get);
		}
	}
}
