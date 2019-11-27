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
                        .FirstOrDefaultAsync(x => x.WowItemId == wowItemId);

                    if (wowItem == null)
                    {
                        var wowHeadItem = await wowHead.GetItemInfoAsync(wowItemId);

                        wowItem = new WowItem()
                        {
                            WowItemId = wowHeadItem.Id,
                            Name = wowHeadItem.Name,
                            ItemQuality = (WowItemQuality)wowHeadItem.Quality,
                            IconUrl = $"https://wow.zamimg.com/images/wow/icons/large/{wowHeadItem.Icon}.jpg",
                            WowHeadUrl = wowHeadItem.Link,
                        };

                        dbContext.Add(wowItem);

                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            catch (Exception)
            {
            }

            return Optional.FromNoValue<WowItem>();
        }
    }
}
