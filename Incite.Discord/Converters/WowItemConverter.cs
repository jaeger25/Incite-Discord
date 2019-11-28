﻿using DSharpPlus.CommandsNext;
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
    public class WowItemConverter : IArgumentConverter<WowItem>
    {
        public async Task<Optional<WowItem>> ConvertAsync(string value, CommandContext ctx)
        {
            try
            {
                var wowHead = ctx.Services.GetService<WowHeadService>();
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
            }

            catch (Exception)
            {
            }

            return Optional.FromNoValue<WowItem>();
        }

        async Task<WowItem> GetOrCreateWowItemAsync(InciteDbContext dbContext, WowHeadItem wowHeadItem)
        {
            WowItem wowItem = await dbContext.WowItems
                .FirstOrDefaultAsync(x => x.WowId == wowHeadItem.Id);

            if (wowItem != null)
            {
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

            dbContext.Add(wowItem);

            if (wowHeadItem.CreatedBy != null)
            {
                WowSpell wowSpell = await dbContext.WowSpells
                    .FirstOrDefaultAsync(x => x.WowId == wowHeadItem.CreatedBy.Id);

                if (wowSpell == null)
                {
                    wowSpell = new WowSpell()
                    {
                        WowId = wowHeadItem.CreatedBy.Id,
                        Name = wowHeadItem.CreatedBy.Name,
                    };

                    dbContext.WowSpells.Add(wowSpell);

                    foreach (var reagent in wowHeadItem.CreatedBy.Reagents)
                    {
                        var wowReagent = new WowReagent()
                        {
                            Count = reagent.Count,
                            WowSpell = wowSpell,
                            WowItem = await GetOrCreateWowItemAsync(dbContext, reagent.Item),
                        };

                        dbContext.WowReagents.Add(wowReagent);
                    }
                }

                wowItem.CreatedBy = wowSpell;
            }

            await dbContext.SaveChangesAsync();
            return wowItem;
        }
    }
}
