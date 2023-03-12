using Microsoft.Extensions.DependencyInjection;
using PhoneInfo.API.Application.Services;
using System.Runtime.CompilerServices;

namespace PhoneInfo.API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static void AddCatalogService(this IServiceCollection services) => services.AddScoped<ICatalogService, CatalogService>();
    }
}
