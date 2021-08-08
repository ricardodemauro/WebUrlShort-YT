using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebUrlShort
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILiteDatabase, LiteDatabase>(x => new LiteDatabase("lite.db"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", Delegates.RequestDelegates.GetHomePage);
                endpoints.MapPost("/url", Delegates.RequestDelegates.CreateUrl);
                endpoints.MapFallback(Delegates.RequestDelegates.RedirectToUrl);
            });
        }
    }
}
