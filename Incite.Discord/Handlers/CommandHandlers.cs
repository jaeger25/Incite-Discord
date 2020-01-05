using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Incite.Discord.Services;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Handlers
{
    public class CommandHandlers : BaseHandler
    {
        readonly GuildCommandPrefixCache m_guildCommandPrefixCache;

        public CommandHandlers(IServiceScopeFactory scopeFactory, DiscordClient client, GuildCommandPrefixCache guildCommandPrefixCache) : base(scopeFactory)
        {
            m_guildCommandPrefixCache = guildCommandPrefixCache;
            client.MessageCreated += Client_MessageCreated;
        }

        async Task Client_MessageCreated(MessageCreateEventArgs e)
        {
            if (e.Author.IsBot)
            {
                return;
            }

            char prefix = m_guildCommandPrefixCache.GetPrefix(e.Guild.Id);
            bool isCommand = e.Message.Content.Length > 1 && e.Message.Content.StartsWith(prefix) || e.Message.Content.StartsWith("!help");
            if (!isCommand)
            {
                return;
            }

            var content = e.Message.Content.Substring(1);
            var commands = e.Client.GetCommandsNext();
            var cmd = commands.FindCommand(content, out var args);
            if (cmd == null)
            {
                return;
            }

            if (content.StartsWith("help") && prefix != '!')
            {
                await e.Message.RespondAsync($"You're guild's command prefix is '{prefix}'. '!' will not work for other commands.");
            }

            var ctx = commands.CreateContext(e.Message, prefix.ToString(), cmd, args);
            _ = Task.Run(async () => await commands.ExecuteCommandAsync(ctx));
        }
    }
}
