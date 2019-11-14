using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Converters
{
    public static class UserFriendlyEnumTypeConverter
    {
        public static string GetUserFriendlyType<T>() where T : Enum
        {
            return $"{typeof(T).Name}. Values: {string.Join(',', Enum.GetNames(typeof(T)))}";
        }
    }

    public class InciteRoleArgumentConverter : IArgumentConverter<InciteRole>
    {
        public Task<Optional<InciteRole>> ConvertAsync(string value, CommandContext ctx)
        {
            if (!Enum.TryParse(value, out InciteRole role))
            {
                return Task.FromResult(Optional.FromNoValue<InciteRole>());
            }

            return Task.FromResult(Optional.FromValue(role));
        }
    }
}
