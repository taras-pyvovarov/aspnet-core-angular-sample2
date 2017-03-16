using System.IO;
using aspnet_core_angular_sample2.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace aspnet_core_angular_sample2
{
    /// <summary>
    /// Aspnet application entry point and startup configuration.
    /// </summary>
    internal class Startup
    {
        /// <summary>
        /// App entry point.
        /// </summary>
        /// <param name="args">Cmd params.</param>
        private static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole();
                loggerFactory.AddDebug();

                app.UseDeveloperExceptionPage();
                //Use Webpack dev middleware from aspnet core SPA services. It will host webpack live and keep sources up-to-date.
                //Additionaly Webpack requires aspnet-webpack module to work.
                //Additionaly Webpack requires webpack-hot-middleware for hot module replacement to work.
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions { HotModuleReplacement = true });
            }
            else
            {
                //Should be addded before UseMvc(), otherwise not working in .netcore1.1
                app.UseExceptionHandler($"/{nameof(HomeController.Error)}");
            }

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                //Default aspnet routing.
                //!!!Investigate using attributes.
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //SPA fallback. Will return 404 on not found pages, 
                //but has restriction on using '.' in last sections of url.
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}