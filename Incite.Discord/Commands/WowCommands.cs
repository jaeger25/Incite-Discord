using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Incite.Discord.Attributes;
using Incite.Discord.Extensions;
using Incite.Discord.ApiModels;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Incite.Discord.Services;
using DSharpPlus.Entities;
using Incite.Discord.Converters;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using DSharpPlus;

namespace Incite.Discord.Commands
{
    [Group("wow")]
    [ModuleLifespan(ModuleLifespan.Transient)]
    [Description("Commands for managing info related to in-game WoW concepts")]
    public class WowCommands : BaseCommandModule
    {
        [Group("character")]
        [Aliases("char")]
        [ModuleLifespan(ModuleLifespan.Transient)]
        [Description("Commands for managing your characters")]
        public class WowCharacterCommands : BaseInciteCommand
        {
            readonly InciteDbContext m_dbContext;

            public WowCharacterCommands(InciteDbContext dbContext)
            {
                m_dbContext = dbContext;
            }

            [Command("list")]
            [Description("Lists your characters")]
            public async Task List(CommandContext context)
            {
                StringBuilder characterList = new StringBuilder("__**Character**__ , __**Class**__");
                foreach (var character in User.WowCharacters.OrderBy(x => x.WowServerId))
                {
                    characterList.Append($"\n{character} , {character.WowClass}");
                }

                var dmChannel = await context.Member.CreateDmChannelAsync();
                await dmChannel.SendMessageAsync(characterList.ToString());

                ResponseString = "Command executed. Check your DMs.";
            }

            [Command("add")]
            [Description("Adds a character to your character list")]
            public async Task AddCharacter(CommandContext context,
                [Description(Descriptions.WowCharacter)] QualifiedWowCharacter character,
                [Description(Descriptions.WowClass)] WowClass characterClass,
                [Description(Descriptions.WowFaction)] WowFaction faction)
            {
                if (User.WowCharacters.Any(x => x.WowServerId == character.Server.Id && x.Name.Equals(character.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    ResponseString = "You have already registered this character";
                    return;
                }

                var wowCharacter = new WowCharacter()
                {
                    Name = character.Name,
                    WowFaction = faction,
                    GuildId = Guild.Id,
                    UserId = User.Id,
                    WowClassId = characterClass.Id,
                    WowServerId = character.Server.Id,
                };

                User.WowCharacters.Add(wowCharacter);

                if (Member.PrimaryWowCharacter == null)
                {
                    Member.PrimaryWowCharacter = wowCharacter;
                }

                await m_dbContext.SaveChangesAsync();
            }

            [Command("remove")]
            [Description("Removes a character from your character list")]
            public async Task RemoveCharacter(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character)
            {
                m_dbContext.WowCharacters.Remove(character.Character);
                await m_dbContext.SaveChangesAsync();
            }

            [Command("set-class")]
            [Description("Sets a property for one of your characters")]
            public async Task SetClass(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character,
                [Description(Descriptions.WowClass)] WowClass wowClass)
            {
                character.Character.WowClassId = wowClass.Id;
                await m_dbContext.SaveChangesAsync();
            }

            [Command("set-server")]
            [Description("Sets a property for one of your characters")]
            public async Task SetServer(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character,
                [Description(Descriptions.WowServer)] WowServer server)
            {
                character.Character.WowServerId = server.Id;
                await m_dbContext.SaveChangesAsync();
            }

            [Command("set-faction")]
            [Description("Sets a property for one of your characters")]
            public async Task SetFaction(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character,
                [Description(Descriptions.WowFaction)] WowFaction faction)
            {
                character.Character.WowFaction = faction;
                await m_dbContext.SaveChangesAsync();
            }
        }

        [Group("profession")]
        [Aliases("prof")]
        [ModuleLifespan(ModuleLifespan.Transient)]
        [Description("Commands for searching and managing professions")]
        public class WowProfessionCommands : BaseInciteCommand
        {
            readonly InciteDbContext m_dbContext;

            public WowProfessionCommands(InciteDbContext dbContext)
            {
                m_dbContext = dbContext;
            }

            [Command("list")]
            [Description("Lists your characters' profressions")]
            public async Task List(CommandContext context)
            {
                StringBuilder message = new StringBuilder();
                foreach (var character in User.WowCharacters)
                {
                    message.Append($"{character}\n");

                    foreach (var profession in character.WowCharacterProfessions)
                    {
                        message.Append($"\t{profession.WowProfession}\n");
                        foreach (var recipe in profession.WowCharacterRecipes)
                        {
                            message.Append($"\t\t{recipe.Recipe}\n");
                        }
                    }
                }

                var dmChannel = await context.Member.CreateDmChannelAsync();
                await dmChannel.SendMessageAsync(message.ToString());

                ResponseString = "Command executed. Check your DMs.";
            }

            [Command("list")]
            [Description("Lists your character's profressions")]
            public async Task List(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character)
            {
                StringBuilder message = new StringBuilder();

                message.Append($"{character.Character}\n");
                foreach (var profession in character.Character.WowCharacterProfessions)
                {
                    message.Append($"\t{profession.WowProfession}\n");
                    foreach(var recipe in profession.WowCharacterRecipes)
                    {
                        message.Append($"\t\t{recipe.Recipe}\n");
                    }
                }

                var dmChannel = await context.Member.CreateDmChannelAsync();
                await dmChannel.SendMessageAsync(message.ToString());

                ResponseString = "Command executed. Check your DMs.";
            }

            [Command("add")]
            [Priority(100)]
            [Description("Adds a profession to the specified character")]
            public async Task Add(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character,
                [Description(Descriptions.WowProfession)] WowProfession profession)
            {
                if (character.Character.WowCharacterProfessions.Any(x => x.WowProfessionId == profession.Id))
                {
                    ResponseString = $"{character.Character} already has this profession added.";
                    return;
                }

                m_dbContext.WowCharacterProfessions.Add(new WowCharacterProfession()
                {
                    WowCharacterId = character.Character.Id,
                    WowProfessionId = profession.Id,
                });

                await m_dbContext.SaveChangesAsync();
            }

            [Command("add")]
            [Priority(90)]
            [Description("Adds a recipe to the specified character's profession")]
            public async Task Add(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character,
                [Description(Descriptions.WowProfession)] WowProfession profession,
                [Description(Descriptions.WowItemRecipe)] [RemainingText] WowItemRecipe recipe)
            {
                var characterProfession = character.Character.WowCharacterProfessions
                    .FirstOrDefault(x => x.WowProfessionId == profession.Id);

                if (characterProfession == null)
                {
                    ResponseString = $"{character.Character} does not know {profession}";
                    return;
                }

                var charRecipe = character.Character.WowCharacterProfessions
                    .FirstOrDefault(x => x.WowProfessionId == profession.Id)
                    ?.WowCharacterRecipes
                    .FirstOrDefault(x => x.RecipeId == recipe.Recipe.Id);

                if (charRecipe != null)
                {
                    ResponseString = $"{character.Character} already knows {recipe.Recipe}";
                    return;
                }

                charRecipe = new WowCharacterRecipe()
                {
                    WowCharacterProfession = characterProfession,
                    Recipe = recipe.Recipe,
                };

                m_dbContext.WowCharacterRecipes.Add(charRecipe);
                await m_dbContext.SaveChangesAsync();
            }

            [Command("remove")]
            [Priority(100)]
            [Description("Removes a profession to the specified character")]
            public async Task Remove(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character,
                [Description(Descriptions.WowProfession)] WowProfession profession,
                [Description("Confirm that you would like to remove this profession and all associated recipes.")] bool removeAllRecipes)
            {
                if (!removeAllRecipes)
                {
                    ResponseString = "You must pass 'true' for removeAllRecipes to confirm.";
                    return;
                }

                var charProfession = character.Character.WowCharacterProfessions
                    .FirstOrDefault(x => x.WowProfessionId == profession.Id);

                if (charProfession != null)
                {
                    var recipes = character.Character.WowCharacterProfessions
                        .Where(x => x.WowProfessionId == profession.Id)
                        .SelectMany(x => x.WowCharacterRecipes);

                    m_dbContext.WowCharacterRecipes.RemoveRange(recipes);
                    m_dbContext.WowCharacterProfessions.Remove(charProfession);
                    await m_dbContext.SaveChangesAsync();
                }
            }

            [Command("remove")]
            [Priority(90)]
            [Description("Removes a recipe from the specified character's profession")]
            public async Task Remove(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character,
                [Description(Descriptions.WowProfession)] WowProfession profession,
                [Description(Descriptions.WowItemRecipe)] [RemainingText] WowItemRecipe recipe)
            {
                var charRecipe = character.Character.WowCharacterProfessions
                    .FirstOrDefault(x => x.WowProfessionId == profession.Id)
                    ?.WowCharacterRecipes
                    .FirstOrDefault(x => x.RecipeId == recipe.Recipe.Id);

                if (charRecipe != null)
                {
                    m_dbContext.WowCharacterRecipes.Remove(charRecipe);
                    await m_dbContext.SaveChangesAsync();
                }
                else
                {
                    ResponseString = "Recipe not found";
                    return;
                }
            }
        }

        [Group("item")]
        [ModuleLifespan(ModuleLifespan.Transient)]
        [Description("Commands for searching wow items")]
        public class WowItemCommands : BaseInciteCommand
        {
            readonly WowHeadService m_wowHead;

            public WowItemCommands(WowHeadService wowHead)
            {
                m_wowHead = wowHead;
            }

            [Command("search")]
            [Description("Gets the WowHead link for the given item")]
            public async Task Search(CommandContext context,
                [Description(Descriptions.WowItem)] [RemainingText] IEnumerable<WowItem> items)
            {
                var wowItems = items.ToArray();
                if (wowItems.Length <= 4)
                {
                    foreach (var item in wowItems)
                    {
                        await context.Message.RespondAsync(embed: CreateEmbedForWowItem(item));
                    }

                    ResponseString = "";
                }
                else
                {
                    var channel = await context.Member.CreateDmChannelAsync();

                    StringBuilder results = new StringBuilder();
                    foreach (var item in wowItems)
                    {
                        var itemResult = $"`{item} | {m_wowHead.GetWowHeadItemUrl(item.WowId)}`";
                        if (results.Length + itemResult.Length >= 2000)
                        {
                            await channel.SendMessageAsync(results.ToString());
                            results = new StringBuilder();
                        }
                        results.AppendLine(itemResult);
                    }

                    await channel.SendMessageAsync(results.ToString());
                    ResponseString = "";
                }
            }

            [Command("seed")]
            [RequireOwner]
            public async Task Seed(CommandContext context)
            {
                var converter = new WowItemConverter();

                var scope = context.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetService<InciteDbContext>();

                for (int wowItemId = 0; wowItemId <= 23328; wowItemId++)
                {
                    try
                    {
                        var wowItem = await dbContext.WowItems
                            .FirstOrDefaultAsync(x => x.WowId == wowItemId);

                        if (wowItem == null)
                        {
                            var wowHeadItem = await m_wowHead.TryGetItemInfoAsync(wowItemId);
                            if (wowHeadItem != null)
                            {
                                await converter.GetOrCreateWowItemAsync(dbContext, wowHeadItem);
                            }

                            await Task.Delay(10);
                        }
                    }
                    catch (Exception ex)
                    {
                        context.Client.DebugLogger.LogMessage(LogLevel.Warning, "WowItemCommands-Seed", $"Seeding error: {wowItemId}", DateTimeOffset.UtcNow.DateTime, ex);
                    }

                    if (wowItemId % 500 == 0)
                    {
                        dbContext.Dispose();
                        scope.Dispose();

                        scope = context.Services.CreateScope();
                        dbContext = scope.ServiceProvider.GetService<InciteDbContext>();


                        context.Client.DebugLogger.LogMessage(LogLevel.Warning, "WowItemCommands-Seed", $"Seeding: {wowItemId} out of {23328}", DateTimeOffset.UtcNow.DateTime);
                    }

                    await Task.Delay(1);
                }

                dbContext.Dispose();
                scope.Dispose();
            }

            DiscordEmbed CreateEmbedForWowItem(WowItem item)
            {
                StringBuilder createdBy = new StringBuilder("__**Created By**__\n");
                foreach (var spell in item.CreatedBy)
                {
                    createdBy.AppendLine(spell.Name);
                    foreach (var reagent in spell.WowReagents)
                    {
                        createdBy.AppendLine($"\t{reagent.WowItem.Name} x {reagent.Count}");
                    }
                }

                return new DiscordEmbedBuilder()
                    .WithTitle(item.Name)
                    .WithThumbnailUrl(m_wowHead.GetWowHeadIconUrl(item.WowHeadIcon, WowHeadIconSize.Medium))
                    .WithUrl(m_wowHead.GetWowHeadItemUrl(item.WowId))
                    .WithColor(WowItemQualityToColor(item.ItemQuality))
                    .WithDescription(createdBy.Length > 0 ? createdBy.ToString() : "")
                    .Build();
            }

            DiscordColor WowItemQualityToColor(WowItemQuality quality)
            {
                switch (quality)
                {
                    case WowItemQuality.Poor:
                        return DiscordColor.LightGray;
                    case WowItemQuality.Common:
                        return DiscordColor.White;
                    case WowItemQuality.Uncommon:
                        return DiscordColor.Green;
                    case WowItemQuality.Rare:
                        return DiscordColor.Blue;
                    case WowItemQuality.Epic:
                        return DiscordColor.Purple;
                    case WowItemQuality.Legendary:
                        return DiscordColor.Orange;
                }

                throw new ArgumentException();
            }
        }
    }
}