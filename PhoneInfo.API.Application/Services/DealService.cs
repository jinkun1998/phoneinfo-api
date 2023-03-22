using Microsoft.Extensions.Configuration;
using PhoneInfo.API.Domain.HttpRequest;
using System.Net;

namespace PhoneInfo.API.Application.Services
{
	public interface IDealService
	{
		Task<(string, HttpStatusCode)> GetDealsAsync();
	}
	public class DealService : ServiceBase, IDealService
	{
		public DealService(IHttpService httpService, IConfiguration configuration) : base(httpService, configuration)
		{
		}

		public async Task<(string, HttpStatusCode)> GetDealsAsync()
		{
			return await GetResultAsync("deals.php3", HttpMethod.Get);
		}
	}
}
