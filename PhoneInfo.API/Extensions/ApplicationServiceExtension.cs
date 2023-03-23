using Microsoft.Extensions.DependencyInjection;
using PhoneInfo.API.Application.Services;
using System.Runtime.CompilerServices;

namespace PhoneInfo.API.Extensions
{
	public static class ApplicationServiceExtension
	{
		#region Services
		public static void AddCatalogService(this IServiceCollection services) => services.AddScoped<ICatalogService, CatalogService>();
		public static void AddDealService(this IServiceCollection services) => services.AddScoped<IDealService, DealService>();
		public static void AddGlossaryService(this IServiceCollection services) => services.AddScoped<IGlossaryService, GlossaryService>();
		#endregion
	}
}
