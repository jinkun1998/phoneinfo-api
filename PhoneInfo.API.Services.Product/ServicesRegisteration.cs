using Microsoft.Extensions.DependencyInjection;
using PhoneInfo.API.Services.Product.Services;

namespace PhoneInfo.API.Services.Product
{
	public static class ServicesRegisteration
	{
		public static void AddProductServiceLayer(this IServiceCollection services)
		{
			services.AddScoped<ICatalogService, CatalogService>();
		}
	}
}