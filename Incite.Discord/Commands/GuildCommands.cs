using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Incite.Discord.ApiModels;
using Incite.Discord.Attributes;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("guild")]
    [RequireGuildConfigured]
    [Description("Commands for managing guild members and settings")]
    public class GuildCommands : BaseInciteCommand
    {
        readonly InciteDbContext m_dbContext;

        public GuildCommands(InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        [Command("set-server")]
        [RequireInciteRole(RoleKind.Leader)]
        [Description("Sets the WoW server for the guild")]
        public async Task SetRealm(CommandContext context,
            [Description(Descriptions.WowServer)] WowServer server)
        {
            Guild.WowServerId = server.Id;
            await m_dbContext.SaveChangesAsync();
        }

        [Command("list")]
        [Description("Lists the users which are the specified profession")]
        public async Task List(CommandContext context,
            [Description(Descriptions.WowProfession)] WowProfession profession)
        {
            var characters = Guild.WowCharacters
                .Where(x => x.WowCharacterProfessions
                    .Any(x => x.WowProfessionId == profession.Id));

            StringBuilder message = new StringBuilder($"{profession}\n");
            foreach (var character in characters)
            {
                message.Append($"{character}\n");
            }

            var dmChannel = await context.Member.CreateDmChannelAsync();
            await dmChannel.SendMessageAsync(message.ToString());

            ResponseString = "Command executed. Check your DMs.";
        }
    }
}
