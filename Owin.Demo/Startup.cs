using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Diagnostics;
using Owin.Demo.Middleware;
using Nancy.Owin;
using System.Web.Http;

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

            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Auth/Login")
            });

            app.Use(async (ctx, next) =>
                {
                    if (ctx.Authentication.User.Identity.IsAuthenticated)
                        Debug.WriteLine("User: " + ctx.Authentication.User.Identity.Name);
                    else
                        Debug.WriteLine("User Not Authenticated");
                    await next();
                });

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);

            app.Map("/nancy", mappedApp => { mappedApp.UseNancy(); });
            //app.UseNancy(conf =>
            //    {
            //        conf.PassThroughWhenStatusCodesAre(Nancy.HttpStatusCode.NotFound);
            //    });

            //app.Use(async (ctx, next) =>
            //{
            //    Debug.WriteLine("the next ...");
            //    await ctx.Response.WriteAsync("<html><head></head><body><h1>Hello World</h1></body></html>");
            //});
        }
    }
}
