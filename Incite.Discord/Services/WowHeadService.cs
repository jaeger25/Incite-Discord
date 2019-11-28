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

    public class WowHeadSpell
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<WowHeadReagent> Reagents { get; set; } = new List<WowHeadReagent>();
    }

    public class WowHeadItemClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class WowHeadItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quality { get; set; }
        public string Icon { get; set; }
        public WowHeadItemClass ItemClass { get; set; } = new WowHeadItemClass();
        public WowHeadItemClass ItemSubclass { get; set; } = new WowHeadItemClass();

        public WowHeadSpell? CreatedBy { get; set; }
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

        public async Task<WowHeadItem> TryGetItemInfoAsync(int wowItemId)
        {
            var itemResponse = await m_httpClient.GetAsync($"item={wowItemId}&xml");
            using var itemXmlReader = XmlReader.Create(await itemResponse.Content.ReadAsStreamAsync());

            var item = new WowHeadItem()
            {
                Id = wowItemId,
            };

            if (!itemXmlReader.ReadToDescendant("item"))
            {
                return null;
            }

            itemXmlReader.ReadToDescendant("name");
            item.Name = itemXmlReader.ReadElementContentAsString();

            itemXmlReader.ReadToNextSibling("quality");
            itemXmlReader.MoveToAttribute("id");
            item.Quality = itemXmlReader.ReadContentAsInt();

            itemXmlReader.ReadToNextSibling("class");
            itemXmlReader.MoveToAttribute("id");
            item.ItemClass.Id = itemXmlReader.ReadContentAsInt();
            itemXmlReader.MoveToContent();
            item.ItemClass.Name = itemXmlReader.ReadElementContentAsString();

            itemXmlReader.MoveToAttribute("id");
            item.ItemSubclass.Id = itemXmlReader.ReadContentAsInt();
            itemXmlReader.MoveToContent();
            item.ItemSubclass.Name = itemXmlReader.ReadElementContentAsString();

            item.Icon = itemXmlReader.ReadElementContentAsString();

            if (itemXmlReader.ReadToNextSibling("createdBy"))
            {
                itemXmlReader.ReadToDescendant("spell");
                itemXmlReader.MoveToAttribute("id");

                item.CreatedBy = new WowHeadSpell()
                {
                    Id = itemXmlReader.ReadContentAsInt(),
                };

                itemXmlReader.MoveToAttribute("name");
                item.CreatedBy.Name = itemXmlReader.ReadContentAsString();

                itemXmlReader.MoveToElement();
                itemXmlReader.ReadToDescendant("reagent");

                do
                {
                    itemXmlReader.MoveToAttribute("id");
                    int reagentId = itemXmlReader.ReadContentAsInt();

                    itemXmlReader.MoveToAttribute("count");
                    int count = itemXmlReader.ReadContentAsInt();

                    var reagentItem = await TryGetItemInfoAsync(reagentId);
                    item.CreatedBy.Reagents.Add(new WowHeadReagent()
                    {
                        Count = count,
                        Item = reagentItem,
                    });
                }
                while (itemXmlReader.ReadToNextSibling("reagent"));
            }

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
