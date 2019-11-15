using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using Incite.Models;
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
            return $"{typeof(T).Name}";
        }
    }

    public class RoleKindArgumentConverter : IArgumentConverter<RoleKind>
    {
        public Task<Optional<RoleKind>> ConvertAsync(string value, CommandContext ctx)
        {
            if (!Enum.TryParse(value, out RoleKind role))
            {
                return Task.FromResult(Optional.FromNoValue<RoleKind>());
            }

            return Task.FromResult(Optional.FromValue(role));
        }
    }
}
