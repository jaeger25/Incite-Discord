﻿using DSharpPlus;
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
    [Group("server")]
    [RequireGuildConfigured]
    [ModuleLifespan(ModuleLifespan.Transient)]
    [Description("Commands for querying server information")]
    public class ServerCommands : BaseInciteCommand
    {
        readonly InciteDbContext m_dbContext;

        public ServerCommands(InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        [Command("list")]
        [Description("Lists the guild's which are currently using this bot")]
        public async Task List(CommandContext context)
        {
            StringBuilder message = new StringBuilder($"__**Guilds**__\n");
            foreach (var guild in context.Client.Guilds)
            {
                message.AppendLine(guild.Value.Name);
            }

            var dmChannel = await context.Member.CreateDmChannelAsync();
            await dmChannel.SendMessageAsync(message.ToString());
            ResponseString = "";
        }

        [Command("list")]
        [Priority(100)]
        [Description("Lists the users which are the specified profession")]
        public async Task List(CommandContext context,
            [Description(Descriptions.WowProfession)] WowProfession profession)
        {
            var characters = Guild.WowServer.WowCharacters
                .Where(x => x.WowCharacterProfessions
                    .Any(x => x.WowProfessionId == profession.Id))
                .OrderBy(x => x.WowCharacterProfessions
                    .Select(x => x.WowCharacterRecipes.Count));

            StringBuilder message = new StringBuilder($"__**{profession}**__\n");
            foreach (var character in characters)
            {
                message.Append($"{character}\n");
            }

            ResponseString = message.ToString();
        }

        [Command("list")]
        [Priority(90)]
        [Description("Lists the users which know the specified recipe")]
        public async Task List(CommandContext context,
            [Description(Descriptions.WowItemRecipe)] [RemainingText] WowItemRecipe recipe)
        {
            var characters = Guild.WowServer.WowCharacters
                .Where(x => x.WowCharacterProfessions
                    .Any(x => x.WowCharacterRecipes
                        .Any(x => x.RecipeId == recipe.Recipe.Id)));

            StringBuilder message = new StringBuilder($"__**{recipe.Recipe}**__\n");
            foreach (var character in characters)
            {
                message.AppendLine($"{character}");
            }

            ResponseString = message.ToString();
        }
    }
}
