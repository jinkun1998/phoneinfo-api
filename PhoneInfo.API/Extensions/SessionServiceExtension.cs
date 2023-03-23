using Microsoft.Extensions.DependencyInjection;

namespace PhoneInfo.API.Extensions
{
	public static class SessionServiceExtension
	{
		public static void AddSessionServiceLayer(this IServiceCollection services)
		{
			services.AddDistributedMemoryCache();
			services.AddSession();
		}
	}
}
