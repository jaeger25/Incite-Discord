using DSharpPlus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Discord.Handlers
{
    public class BaseHandler
    {
        protected IServiceScopeFactory ServiceScopeFactory { get; }

        protected BaseHandler(IServiceScopeFactory scopeFactory)
        {
            ServiceScopeFactory = scopeFactory;
        }
    }
}
