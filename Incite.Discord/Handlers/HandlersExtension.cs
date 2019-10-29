using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Incite.Discord.Handlers
{
    public class HandlersExtension : BaseExtension
    {
        Dictionary<Type, BaseHandler> m_handlers = new Dictionary<Type, BaseHandler>();

        public HandlersExtension()
        {
        }

        protected override void Setup(DiscordClient client)
        {
            Client = client;
        }

        public void RegisterHandlers(Assembly assembly)
        {
            var baseType = typeof(BaseHandler);
            foreach(var type in assembly.GetTypes())
            {
                if (type != baseType && baseType.IsAssignableFrom(type))
                {
                    m_handlers[type] = (BaseHandler)Activator.CreateInstance(type, Client);
                }
            }
        }
    }
}
