using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using Incite.Discord.ApiModels;
using Incite.Discord.Extensions;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Converters
{
    public class WowItemRecipeConverter : IArgumentConverter<WowItemRecipe>
    {
        public async Task<Optional<WowItemRecipe>> ConvertAsync(string value, CommandContext ctx)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Optional.FromNoValue<WowItemRecipe>();
            }

            var dbContext = ctx.Services.GetService<InciteDbContext>();
            var recipes = await dbContext.WowItems
                .Where(x => x.WowItemClass.Name == "Recipes" && EF.Functions.ILike(x.Name, $"%{value}%"))
                .ToArrayAsync();

            if (recipes.Length != 1)
            {
                StringBuilder results = new StringBuilder(recipes.Length > 1 ? "__**Ambiguous Results**__\n" : "__**No Item found**__");
                foreach (var recipe in recipes)
                {
                    results.AppendLine(recipe.ToString());
                }

                await ctx.Message.RespondAsync(results.ToString());
                return Optional.FromNoValue<WowItemRecipe>();
            }

            return Optional.FromValue(new WowItemRecipe()
            {
                Recipe = recipes[0]
            });
        }
    }
}
