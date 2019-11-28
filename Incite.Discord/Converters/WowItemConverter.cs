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
                            wowItem = await CreateWowItemAsync(dbContext, wowHeadItem);

                            await dbContext.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        return Optional.FromNoValue<WowItem>();
                    }

                    return Optional.FromValue(wowItem);
                }
            }

            catch (Exception)
            {
            }

            return Optional.FromNoValue<WowItem>();
        }

        async Task<WowItem> CreateWowItemAsync(InciteDbContext dbContext, WowHeadItem wowHeadItem)
        {
            var wowItemClass = await dbContext.WowItemClasses
                .Include(x => x.WowItemSubclasses)
                .FirstOrDefaultAsync(x => x.WowId == wowHeadItem.ItemClass.Id);

            if (wowItemClass == null)
            {
                wowItemClass = new WowItemClass()
                {
                    WowId = wowHeadItem.Id,
                    Name = wowHeadItem.Name
                };

                wowItemClass.WowItemSubclasses.Add(new WowItemSubclass()
                {
                    WowId = wowHeadItem.ItemSubclass.Id,
                    Name = wowHeadItem.ItemSubclass.Name,
                    WowItemClass = wowItemClass,
                });
            }
            else
            {
                var wowItemSubclass = wowItemClass.WowItemSubclasses
                    .FirstOrDefault(x => x.WowId == wowHeadItem.ItemSubclass.Id);

                if (wowItemClass == null)
                {
                    wowItemClass.WowItemSubclasses.Add(new WowItemSubclass()
                    {
                        WowId = wowHeadItem.ItemSubclass.Id,
                        Name = wowHeadItem.ItemSubclass.Name,
                        WowItemClass = wowItemClass,
                    });
                }
            }

            var wowItem = new WowItem()
            {
                WowId = wowHeadItem.Id,
                Name = wowHeadItem.Name,
                ItemQuality = (WowItemQuality)wowHeadItem.Quality,
                WowHeadIcon = wowHeadItem.Icon,
                WowItemClass = wowItemClass,
                WowItemSubclass = wowItemClass.WowItemSubclasses.First(x => x.WowId == wowHeadItem.ItemSubclass.Id)
            };

            if (wowHeadItem.CreatedBy != null)
            {
                var wowSpell = await dbContext.WowSpells
                    .FirstOrDefaultAsync(x => x.WowId == wowHeadItem.CreatedBy.Id);

                if (wowSpell == null)
                {
                    wowSpell = new WowSpell()
                    {
                        WowId = wowHeadItem.CreatedBy.Id,
                        Name = wowHeadItem.CreatedBy.Name,
                    };

                    foreach(var reagent in wowHeadItem.CreatedBy.Reagents)
                    {
                        var wowReagent = new WowReagent()
                        {
                            Count = reagent.Count,
                            WowSpell = wowSpell,
                            WowItem = await CreateWowItemAsync(dbContext, reagent.Item),
                        };

                        wowSpell.WowReagents.Add(wowReagent);
                    }
                }

                wowItem.CreatedBy = wowSpell;
            }

            dbContext.Add(wowItem);
            return wowItem;
        }
    }
}
