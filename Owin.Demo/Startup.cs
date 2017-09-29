using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Diagnostics;
using Owin.Demo.Middleware;

//[assembly: OwinStartup(typeof(Owin.Demo.Startup))]

namespace Owin.Demo
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            app.UseDebugMiddleware(new DebugMiddlewareOptions
                {
                    OnIncomingRequest = (ctx) =>
                        {
                            var watch = new Stopwatch();
                            watch.Start();
                            ctx.Environment["DebugStopWatch"] = watch;
                        },
                    OnOutcomingRequest = (ctx) =>
                        {
                            var watch = (Stopwatch)ctx.Environment["DebugStopWatch"];
                            watch.Stop();
                            Debug.WriteLine("Request took: " + watch.ElapsedMilliseconds + " ms");
                        }
                });

            app.UseNancy();

            app.Use(async (ctx, next) =>
            {
                Debug.WriteLine("the next ...");
                await ctx.Response.WriteAsync("<html><head></head><body><h1>Hello World</h1></body></html>");
            });
        }
    }
}
