using Microsoft.Extensions.Configuration;
using PhoneInfo.API.Domain.HttpRequest;
using PhoneInfo.API.Domain.Interfaces;
using System.Net;

namespace PhoneInfo.API.Application.Services
{
	public class GlossaryService : ServiceBase, IGlossaryService
	{
		public GlossaryService(IHttpService httpService, IConfiguration configuration) : base(httpService, configuration)
		{
		}

		public async Task<(string, HttpStatusCode)> GetGlossaries()
		{
			return await GetResultAsync("glossary.php3", HttpMethod.Get);
		}

		public async Task<(string, HttpStatusCode)> GetTerm(string term)
		{
			return await GetResultAsync($"glossary.php3?term={term}", HttpMethod.Get);
		}
	}
}
