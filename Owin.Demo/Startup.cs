using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Diagnostics;

//[assembly: OwinStartup(typeof(Owin.Demo.Startup))]

namespace Owin.Demo
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                Debug.WriteLine("Incoming request: " + ctx.Request.Path);
                await next();
                Debug.WriteLine("Outcoming request: " + ctx.Request.Path);
            });

            app.Use(async (ctx, next) =>
            {
                Debug.WriteLine("the next ...");
                await ctx.Response.WriteAsync("<html><head></head><body><h1>Hello World</h1></body></html>");
            });
        }
    }
}
