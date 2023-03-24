using Microsoft.Extensions.DependencyInjection;
using PhoneInfo.API.Domain.HttpRequest;

namespace PhoneInfo.API.Extensions
{
	public static class HttpRequestServiceExtension
	{
		public static void AddHttpServiceLayer(this IServiceCollection services) => services.AddScoped<IHttpService, HttpService>();
	}
}
