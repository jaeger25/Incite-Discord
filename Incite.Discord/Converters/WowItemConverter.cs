using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using Incite.Discord.Extensions;
using Incite.Discord.Services;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Incite.Discord.Converters
{
    public class WowItemsConverter : IArgumentConverter<IEnumerable<WowItem>>
    {
        public async Task<Optional<IEnumerable<WowItem>>> ConvertAsync(string value, CommandContext ctx)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Optional.FromNoValue<IEnumerable<WowItem>>();
            }

            var sqlLikeSearchValue = $"%{value}%";

            var dbContext = ctx.Services.GetService<InciteDbContext>();

            var wowItems = await dbContext.WowItems
                .Include(x => x.WowItemClass)
                .Include(x => x.WowItemSubclass)
                .Include(x => x.CreatedBy)
                    .ThenInclude(x => x.WowReagents)
                        .ThenInclude(x => x.WowItem)
                .Where(x => EF.Functions.ILike(x.Name, sqlLikeSearchValue))
                .ToArrayAsync();

            if (wowItems.Length == 0)
            {
                await ctx.Message.RespondAsync($"No items found.");
            }

            return Optional.FromValue(wowItems
                .OrderBy(x => CalculateDamerauLevenshteinDistance(value.ToLower(), x.Name.ToLower()))
                .AsEnumerable());
        }

        public static int CalculateDamerauLevenshteinDistance(string s, string t)
        {
            var bounds = new { Height = s.Length + 1, Width = t.Length + 1 };

            int[,] matrix = new int[bounds.Height, bounds.Width];

            for (int height = 0; height < bounds.Height; height++) { matrix[height, 0] = height; };
            for (int width = 0; width < bounds.Width; width++) { matrix[0, width] = width; };

            for (int height = 1; height < bounds.Height; height++)
            {
                for (int width = 1; width < bounds.Width; width++)
                {
                    int cost = (s[height - 1] == t[width - 1]) ? 0 : 1;
                    int insertion = matrix[height, width - 1] + 1;
                    int deletion = matrix[height - 1, width] + 1;
                    int substitution = matrix[height - 1, width - 1] + cost;

                    int distance = Math.Min(insertion, Math.Min(deletion, substitution));

                    if (height > 1 && width > 1 && s[height - 1] == t[width - 2] && s[height - 2] == t[width - 1])
                    {
                        distance = Math.Min(distance, matrix[height - 2, width - 2] + cost);
                    }

                    matrix[height, width] = distance;
                }
            }

            return matrix[bounds.Height - 1, bounds.Width - 1];
        }
    }

    public class WowItemConverter : IArgumentConverter<WowItem>
    {
        public async Task<Optional<WowItem>> ConvertAsync(string value, CommandContext ctx)
        {
            try
            {
                var dbContext = ctx.Services.GetService<InciteDbContext>();

                if (int.TryParse(value, out int wowItemId))
                {
                    var wowItem = await dbContext.WowItems
                        .Include(x => x.CreatedBy)
                            .ThenInclude(x => x.WowReagents)
                                .ThenInclude(x => x.WowItem)
                        .FirstOrDefaultAsync(x => x.WowId == wowItemId);

                    if (wowItem == null)
                    {
                        var wowHead = ctx.Services.GetService<WowHeadService>();
                        var wowHeadItem = await wowHead.TryGetItemInfoAsync(wowItemId);
                        if (wowHeadItem != null)
                        {
                            wowItem = await GetOrCreateWowItemAsync(dbContext, wowHeadItem);

                            return Optional.FromValue(wowItem);
                        }
                    }
                    else
                    {
                        return Optional.FromValue(wowItem);
                    }
                }
                else
                {
                    var itemsConverter = new WowItemsConverter();
                    var items = await itemsConverter.ConvertAsync(value, ctx);
                    if(items.HasValue)
                    {
                        return Optional.FromValue(items.Value.First());
                    }
                }
            }

            catch (Exception)
            {
            }

            return Optional.FromNoValue<WowItem>();
        }

        public Task<WowItem> GetOrCreateWowItemAsync(InciteDbContext dbContext, WowHeadItem wowHeadItem)
        {
            return GetOrCreateWowItemAsync(dbContext, wowHeadItem, new Dictionary<int, WowItem>());
        }

        public async Task<WowItem> GetOrCreateWowItemAsync(InciteDbContext dbContext, WowHeadItem wowHeadItem, Dictionary<int, WowItem> seenItems)
        {
            if (seenItems.ContainsKey(wowHeadItem.Id))
            {
                return seenItems[wowHeadItem.Id];
            }

            WowItem wowItem = await dbContext.WowItems
                .FirstOrDefaultAsync(x => x.WowId == wowHeadItem.Id);

            if (wowItem != null)
            {
                seenItems[wowHeadItem.Id] = wowItem;
                return wowItem;
            }

            WowItemSubclass wowItemSubclass = null;
            WowItemClass wowItemClass = await dbContext.WowItemClasses
                .Include(x => x.WowItemSubclasses)
                .FirstOrDefaultAsync(x => x.WowId == wowHeadItem.ItemClass.Id);

            if (wowItemClass == null)
            {
                wowItemClass = new WowItemClass()
                {
                    WowId = wowHeadItem.ItemClass.Id,
                    Name = wowHeadItem.ItemClass.Name
                };

                wowItemSubclass = new WowItemSubclass()
                {
                    WowId = wowHeadItem.ItemSubclass.Id,
                    Name = wowHeadItem.ItemSubclass.Name,
                    WowItemClass = wowItemClass,
                };

                dbContext.WowItemClasses.Add(wowItemClass);
                dbContext.WowItemSubclasses.Add(wowItemSubclass);
            }
            else
            {
                wowItemSubclass = wowItemClass.WowItemSubclasses
                    .FirstOrDefault(x => x.WowId == wowHeadItem.ItemSubclass.Id);

                if (wowItemSubclass == null)
                {
                    wowItemSubclass = new WowItemSubclass()
                    {
                        WowId = wowHeadItem.ItemSubclass.Id,
                        Name = wowHeadItem.ItemSubclass.Name,
                        WowItemClass = wowItemClass,
                    };

                    dbContext.WowItemSubclasses.Add(wowItemSubclass);
                }
            }

            wowItem = new WowItem()
            {
                WowId = wowHeadItem.Id,
                Name = wowHeadItem.Name,
                ItemQuality = (WowItemQuality)wowHeadItem.Quality,
                WowHeadIcon = wowHeadItem.Icon,
                WowItemClass = wowItemClass,
                WowItemSubclass = wowItemSubclass,
            };

            seenItems[wowHeadItem.Id] = wowItem;
            dbContext.Add(wowItem);

            if (wowHeadItem.CreatedBy.Count > 0)
            {
                foreach (var wowHeadSpell in wowHeadItem.CreatedBy)
                {
                    WowSpell wowSpell = await dbContext.WowSpells
                        .FirstOrDefaultAsync(x => x.WowId == wowHeadSpell.Id);

                    if (wowSpell == null)
                    {
                        wowSpell = new WowSpell()
                        {
                            WowId = wowHeadSpell.Id,
                            Name = wowHeadSpell.Name,
                            CreatedItem = wowItem,
                        };

                        dbContext.WowSpells.Add(wowSpell);

                        foreach (var reagent in wowHeadSpell.Reagents)
                        {
                            var wowReagent = new WowReagent()
                            {
                                Count = reagent.Count,
                                WowSpell = wowSpell,
                                WowItem = await GetOrCreateWowItemAsync(dbContext, reagent.Item, seenItems),
                            };

                            dbContext.WowReagents.Add(wowReagent);
                        }
                    }

                    wowItem.CreatedBy.Add(wowSpell);
                }
            }

            await dbContext.SaveChangesAsync();
            return wowItem;
        }
    }
}
