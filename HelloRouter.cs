using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EmptyWebApplication {
    public class HelloRouter : IRouter
    {
        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        public Task RouteAsync(RouteContext context)
        {
            var name = context.RouteData.Values["name"] as string;
            if (String.IsNullOrEmpty(name))
                return Task.FromResult(0);
            var requestPath = context.HttpContext.Request.Path;
            if (requestPath.StartsWithSegments("/hello", StringComparison.OrdinalIgnoreCase)){
                context.Handler = async c => {
                    await c.Response.WriteAsync($"Hi, {name}!");
                };
            }

            return Task.FromResult(0);
        }
    }
}