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
    public class WowHeadItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
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

            return item;
        }
    }
}
