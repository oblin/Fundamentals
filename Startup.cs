using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace EmptyWebApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            var defaultHandler = new RouteHandler((c) =>
                    c.Response.WriteAsync($"Hello world! Route values: " +
                    $"{string.Join(", ", c.GetRouteData().Values)}")
               );

            var routeBuilder = new RouteBuilder(app, defaultHandler);

            routeBuilder.AddHelloRoute(app);

            routeBuilder.MapRoute("Track Package Route",
                "package/{operation:regex(track|create|detonate)}/{id:int}");

            app.UseRouter(routeBuilder.Build());

            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("Hello from Hebe!");
            // });
        }
    }
}
