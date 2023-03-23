using Microsoft.Extensions.Configuration;
using PhoneInfo.API.Domain.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PhoneInfo.API.Application.Services
{
	public interface IGlossaryService
	{
		Task<(string, HttpStatusCode)> GetGlossaries();
		Task<(string, HttpStatusCode)> GetTerm(string term);
	}

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
