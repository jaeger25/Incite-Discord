using DSharpPlus;
using DSharpPlus.CommandsNext;
using Incite.Discord.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Incite.Discord.DiscordExtensions
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
            var serviceProvider = Client.GetCommandsNext().Services;

            var baseType = typeof(BaseHandler);
            foreach(var type in assembly.GetTypes())
            {
                if (type != baseType && baseType.IsAssignableFrom(type))
                {
                    var constructors = type.GetTypeInfo()
                        .DeclaredConstructors
                        .Where(x => x.IsPublic)
                        .ToArray();

                    if (constructors.Length != 1)
                    {
                        throw new ArgumentException($"{type.FullName} contains more than 1 public constructor");
                    }

                    var constructorArgs = constructors[0].GetParameters();
                    var args = new object[constructorArgs.Length];
                    for (int i = 0; i < constructorArgs.Length; i++)
                    {
                        args[i] = serviceProvider.GetRequiredService(constructorArgs[i].ParameterType);
                    }

                    m_handlers[type] = (BaseHandler)Activator.CreateInstance(type, args);
                }
            }
        }
    }
}
