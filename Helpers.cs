using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace EmptyWebApplication
{
    public static class Helpers
    {
        public static IRouteBuilder AddHelloRoute(this IRouteBuilder routeBuilder, IApplicationBuilder app)
        {
            routeBuilder.Routes.Add(
                new Route(new HelloRouter(),
                    "hello/{name:alpha}",
                    app.ApplicationServices.GetService<IInlineConstraintResolver>()));

            return routeBuilder;
        }
    }
}