using System;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmptyWebApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectoryBrowser();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseRequestLogger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseStatusCodePages("text/plain", "Response, Status code: {0}");

            app.MapWhen(context => context.Request.Path == "/missingpage", builder => { });

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            // "/errors/400"
            app.Map("/errors", error =>
            {
                error.Run(async context =>
                {
                    var builder = new StringBuilder();
                    var encoder = HtmlEncoder.Default;
                    builder.AppendLine("<html><body>");
                    builder.AppendLine("An error occurred, Status Code: " +
                        encoder.Encode(context.Request.Path.ToString().Substring(1)) + "<br />");
                    var referrer = context.Request.Headers["referer"];
                    if (!string.IsNullOrEmpty(referrer))
                    {
                        builder.AppendLine("Return to <a href=\"" + encoder.Encode(referrer) + "\">" +
                            WebUtility.HtmlEncode(referrer) + "</a><br />");
                    }
                    builder.AppendLine("</body></html>");
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync(builder.ToString());
                });
            });

            HomePage(app);
        }

        public static void HomePage(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                if (context.Request.Query.ContainsKey("throw"))
                    throw new Exception("Exception triggered!");

                var builder = new StringBuilder();
                builder.AppendLine("<html><body>Hello World!");
                builder.AppendLine("<ul>");
                builder.AppendLine("<li><a href=\"/?throw=true\">Throw Exception</a></li>");
                builder.AppendLine("<li><a href=\"/missingpage\">Missing Page</a></li>");
                builder.AppendLine("</ul>");
                builder.AppendLine("</body></html>");


                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(builder.ToString());
            });
        }
    }
}
