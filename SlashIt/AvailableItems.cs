using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SlashIt
{
    public sealed class AvailableItems
    {
        static readonly AvailableItems instance = new AvailableItems();

        public List<Item> All = new List<Item>();
        
        AvailableItems()
        {
            XDocument doc = XDocument.Load(@".\Config\Items.xml");

            foreach (XElement item in doc.Descendants("Items").Descendants("Item"))
            {
                var itemToAdd = new Item();
                itemToAdd.Name = item.Element("Name").Value;
                itemToAdd.DisplayCharacter = item.Element("DisplayCharacter").Value;
                itemToAdd.ItemTypes = item.Descendants("ItemTypes").Descendants("ItemType").Select(it => Constants.ItemTypeDictionary[it.Value]).ToList();
                itemToAdd.ItemLocations = item.Descendants("ItemLocations").Descendants("ItemLocation").Select(il => Constants.ItemLocationDictionary[il.Value]).ToList();
                itemToAdd.ItemId = Int32.Parse(item.Element("ItemId").Value);

                this.All.Add(itemToAdd);
            }

            var ids = this.All.GroupBy(ai => ai.ItemId).Where(g => g.Count() > 1);

            if (ids.Count() > 0)
            {
                throw new Exception("Found non-unique item ids: " + string.Join(",", ids.SelectMany(g => g).Select(g => g.ItemId)));
            }
        }

        public static AvailableItems Instance
        {
            get { return instance; }
        }

        public Item GetItemById(int itemId)
        {
            return this.All.Where(i => i.ItemId == itemId).Single<Item>();
        }
    }
}