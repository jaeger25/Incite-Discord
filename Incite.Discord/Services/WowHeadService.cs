using DSharpPlus;
using DSharpPlus.Entities;
using Incite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Incite.Discord.Services
{
    public enum WowHeadIconSize
    {
        Small,
        Medium,
        Large
    }

    public class WowHeadReagent
    {
        public WowHeadItem Item { get; set; }
        public int Count { get; set; }
    }

    public class WowHeadItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quality { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }

        public List<WowHeadReagent> CreatedBy { get; } = new List<WowHeadReagent>();
    }

    public class WowHeadService
    {
        readonly HttpClient m_httpClient;

        public WowHeadService()
        {
            m_httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://classic.wowhead.com/"),
            };
        }

        public async Task<WowHeadItem> GetItemInfoAsync(int wowItemId)
        {
            var itemResponse = await m_httpClient.GetAsync($"item={wowItemId}&xml");
            using var itemXmlReader = XmlReader.Create(await itemResponse.Content.ReadAsStreamAsync());

            var item = new WowHeadItem()
            {
                Id = wowItemId,
            };

            itemXmlReader.ReadToDescendant("item");

            itemXmlReader.ReadToDescendant("name");
            item.Name = itemXmlReader.ReadElementContentAsString();

            itemXmlReader.ReadToNextSibling("quality");
            itemXmlReader.MoveToAttribute("id");

            item.Quality = itemXmlReader.ReadContentAsInt();

            itemXmlReader.ReadToNextSibling("icon");
            item.Icon = itemXmlReader.ReadElementContentAsString();

            itemXmlReader.ReadToNextSibling("link");
            item.Link = itemXmlReader.ReadElementContentAsString();

            return item;
        }

        public string GetWowHeadItemUrl(int wowItemId)
        {
            return $"https://classic.wowhead.com/item={wowItemId}";
        }

        public string GetWowHeadIconUrl(string icon, WowHeadIconSize size)
        {
            string iconSize = size.ToString();
            return $"https://wow.zamimg.com/images/wow/icons/large/{icon}.jpg";
        }
    }
}
