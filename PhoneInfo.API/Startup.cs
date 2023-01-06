using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhoneInfo.API.Core;
using PhoneInfo.API.Core.Middlewares;

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
            #endregion

            #region Service
            #endregion
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
            app.UseMiddleware<TokenMiddleware>();
            app.UseMiddleware<MainMiddleware>();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
