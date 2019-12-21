using Incite.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Incite.Discord.Services
{
    public class GuildCommandPrefixCache
    {
        ConcurrentDictionary<ulong, char> m_guildCommandPrefixes = new ConcurrentDictionary<ulong, char>();

        public void SetPrefix(Guild guild, char prefix)
        {
            m_guildCommandPrefixes[guild.DiscordId] = prefix;
        }

        public char GetPrefix(Guild guild)
        {
            return GetPrefix(guild.DiscordId);
        }

        public char GetPrefix(ulong discordId)
        {
            char prefix = '!';
            m_guildCommandPrefixes.TryGetValue(discordId, out prefix);

            return prefix;
        }
    }
}
