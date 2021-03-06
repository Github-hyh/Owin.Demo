﻿using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private DebugMiddlewareOptions _options;

        public DebugMiddleware(AppFunc next, DebugMiddlewareOptions options)
        {
            _next = next;
            _options = options;

            if (_options.OnIncomingRequest == null)
            {
                _options.OnIncomingRequest = ctx => { Debug.WriteLine("Incoming request: " + ctx.Request.Path); };
            }
            if (_options.OnOutcomingRequest == null)
            {
                _options.OnOutcomingRequest = ctx => { Debug.WriteLine("Outcoming request: " + ctx.Request.Path); };
            }
        }

        public async Task Invoke(IDictionary<string, object> enviroment)
        {
            var ctx = new OwinContext(enviroment);

            _options.OnIncomingRequest(ctx);
            await _next(enviroment);
            _options.OnOutcomingRequest(ctx);

        }
    }
}