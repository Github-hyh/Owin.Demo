using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppFunc = System.Func<
    System.Collections.Generic.IDictionary<string, object>,
    System.Threading.Tasks.Task
>;

namespace Owin.Demo.Middleware
{
    public class DebugMiddleware
    {
        private AppFunc _next;

        public DebugMiddleware(AppFunc next)
        {
            _next = next;
        }
    }
}