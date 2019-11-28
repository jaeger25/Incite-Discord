using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using Incite.Discord.ApiModels;
using Incite.Discord.Commands;
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
    public class UserWowCharacterConverter : IArgumentConverter<UserWowCharacter>
    {
        public async Task<Optional<UserWowCharacter>> ConvertAsync(string value, CommandContext ctx)
        {
            var command = (BaseInciteCommand)ctx.Command.Module.GetInstance(ctx.Services);
            await command.BeforeExecutionAsync(ctx);

            var nameServerPair = value.Split('-');
            var characters = command.User.WowCharacters
                .Where(x => x.Name.Equals(nameServerPair[0], StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (characters.Length != 1 && nameServerPair.Length == 2)
            {
                characters = characters
                    .Where(x => x.WowServer.Name.Equals(nameServerPair[1], StringComparison.OrdinalIgnoreCase))
                    .ToArray();
            }

            if (characters.Length == 1)
            {
                return Optional.FromValue(new UserWowCharacter()
                {
                    Character = characters[0]
                });
            }

            return Optional.FromNoValue<UserWowCharacter>();
        }
    }
}
