using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhoneInfo.API.Extensions;
using PhoneInfo.API.Middlewares;

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
            services.AddSessionServiceLayer();
            services.AddApiVersioningLayer();

            #region Domain

            services.AddJwtServiceLayer(Configuration);
            services.AddSwaggerServiceLayer();
            services.AddHttpServiceLayer();

            #endregion Domain

            #region Services

            services.AddCatalogService();
			services.AddDealService();
            services.AddGlossaryService();

			#endregion Services
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
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