using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhoneInfo.API.Core;
using PhoneInfo.API.Core.Middlewares;
using PhoneInfo.API.Services.Product;

namespace PhoneInfo.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			#region Core

			services.AddJwtServiceLayer(Configuration);
			services.AddSwaggerServiceLayer();
			services.AddSessionServiceLayer();
			services.AddHttpServiceLayer();

			#endregion Core

			#region Services

			services.AddProductServiceLayer();

			#endregion Services
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseJwtService();
			app.UseSwaggerService(env);
			app.UseAuthorization();
			app.UseSession();

			#region Middewares

			app.UseMiddleware<MainMiddleware>();
			//app.UseMiddleware<TokenMiddleware>();

			#endregion Middewares

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}