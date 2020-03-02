using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Incite.Discord.ApiModels;
using Incite.Discord.Attributes;
using Incite.Discord.Extensions;
using Incite.Discord.Messages;
using Incite.Discord.Services;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("event")]
    [RequireGuildConfigured]
    [RequireWowCharacter]
    [ModuleLifespan(ModuleLifespan.Transient)]
    [Description("Commands for setting and retrieving info about the guild's events")]
    public class EventCommands : BaseInciteCommand
    {
        readonly InciteDbContext m_dbContext;
        readonly EmojiService m_emojiService;

        public EventCommands(InciteDbContext dbContext, EmojiService emojiService)
        {
            m_dbContext = dbContext;
            m_emojiService = emojiService;
        }

        [Command("create")]
        [RequireUserPermissions(Permissions.SendMessages)]
        [Description("Creates an event")]
        public async Task Create(CommandContext context,
            string name,
            string description,
            [Description(Descriptions.DateTime)] DateTimeOffset dateTime)
        {
            if (DateTimeOffset.UtcNow > dateTime.UtcDateTime)
            {
                ResponseString = "The event cannot be in the past";
                return;
            }

            var discordMessage = await context.Message.RespondAsync("\u200b");

            var message = new Message()
            {
                DiscordId = discordMessage.Id,
            };

            var memberEvent = new Models.Event()
            {
                Name = name,
                Description = description,
                DateTime = dateTime,
                GuildId = Guild.Id,
                OwnerId = Member.Id,
                EventMessage = message
            };

            m_dbContext.Events.Add(memberEvent);
            await m_dbContext.SaveChangesAsync();

            var discordEventMessage = new Messages.DiscordEventMessage(context.Client, discordMessage, memberEvent);
            await discordEventMessage.UpdateAsync();

            await discordEventMessage.AddReactionsToEventMessageAsync();
        }

        [Command("update")]
        [RequireUserPermissions(Permissions.SendMessages)]
        [Description("Updates an event")]
        public async Task Update(CommandContext context,
            int eventId,
            string name,
            string description,
            [Description(Descriptions.DateTime)] DateTimeOffset dateTime)
        {
            if (DateTimeOffset.UtcNow > dateTime.UtcDateTime)
            {
                ResponseString = "The event cannot be in the past";
                return;
            }

            var memberEvent = Member.OwnedEvents
                .FirstOrDefault(x => x.Id == eventId);

            if (memberEvent == null)
            {
                ResponseString = "Failed to find event";
                return;
            }

            var discordMessage = await context.Channel.GetMessageAsync(memberEvent.EventMessage.DiscordId);

            memberEvent.Name = name;
            memberEvent.Description = description;
            memberEvent.DateTime = dateTime;

            await m_dbContext.SaveChangesAsync();

            var eventMessage = new DiscordEventMessage(context.Client, discordMessage, memberEvent);
            await eventMessage.UpdateAsync();
        }

        //[Command("export")]
        //[RequireUserPermissions(Permissions.SendMessages)]
        //[Description("Exports an event")]
        //public async Task Export(CommandContext context, int eventId)
        //{
        //    var memberEvent = await m_dbContext.Events
        //        .Include(x => x.EventMembers)
        //            .ThenInclude(x => x.Member.PrimaryWowCharacter)
        //        .FirstOrDefaultAsync(x => x.Id == eventId && x.Guild.DiscordId == context.Guild.Id);

        //    if (memberEvent == null)
        //    {
        //        ResponseString = "Failed to find event";
        //        return;
        //    }

        //    var json = new JObject();
        //    json.Add(new JProperty("Id", memberEvent.Id));
        //    json.Add(new JProperty("Name", memberEvent.Name));
        //    json.Add(new JProperty("Description", memberEvent.Description));
        //    json.Add(new JProperty("DateTime", memberEvent.DateTime.ToString()));

        //    var lateList = memberEvent.EventMembers
        //        .Where(x => x.EmojiDiscordName == m_emojiService.Events.Icon_Late.GetDiscordName())
        //        .Select(x => x.Member.PrimaryWowCharacter.Name);

        //    var absentList = memberEvent.EventMembers
        //        .Where(x => x.EmojiDiscordName == m_emojiService.Events.Icon_Absent.GetDiscordName())
        //        .Select(x => x.Member.PrimaryWowCharacter.Name);

        //    var maybeList = memberEvent.EventMembers
        //        .Where(x => x.EmojiDiscordName == m_emojiService.Events.Icon_Maybe.GetDiscordName())
        //        .Select(x => x.Member.PrimaryWowCharacter.Name);

        //    var attendingList = memberEvent.EventMembers
        //        .Select(x => x.Member.PrimaryWowCharacter.Name)
        //        .Where(x => !lateList.Contains(x) && !absentList.Contains(x) && !maybeList.Contains(x));

        //    json.Add(new JProperty("LateList", new JArray(lateList)));
        //    json.Add(new JProperty("AbsentList", new JArray(absentList)));
        //    json.Add(new JProperty("MaybeList", new JArray(maybeList)));
        //    json.Add(new JProperty("AttendingList", new JArray(attendingList)));

        //    context.Message.RespondWithFileAsync("")
        //}
    }
}
