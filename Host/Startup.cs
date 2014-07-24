using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;
using System.Linq;

[assembly: OwinStartup(typeof(Host.Startup))]

namespace Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                var localAddresses = new string[] { "127.0.0.1", "::1", ctx.Request.LocalIpAddress };
                if (localAddresses.Contains(ctx.Request.RemoteIpAddress))
                {
                    await next();
                }
            });

            app.UseStageMarker(PipelineStage.MapHandler);
            Core.Configuration.WebApiConfig.Configure(app);
        }
    }
}
