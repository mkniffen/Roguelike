using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SlashIt
{
    public class Item
    {
        public enum ItemType : int
        {
            None,
            Weapon,
            Armor,
            Container,
        }

        public enum ItemLocation : int
        {
            None,
            Head,
            Chest,
            Feet,
            HandRight,
            HandLeft,
            Bag1,
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayCharacter { get; set; }
        public int ItemId { get;set; }
        public string ListTag { get; set; }

        public List<ItemLocation> ItemLocations { get; set; }
        public List<ItemType> ItemTypes { get; set; }

        public void Load(XElement item)
        {
 	        this.ItemId = Int32.Parse(item.Element("ItemId").Value);
        }

        public XElement Save()
        {
 	        return new XElement("Item",
                new XElement("ItemId", this.ItemId)
                );
        }

        public bool IsEquipable()
        {
            return  this.ItemTypes.Any(it => it == ItemType.Armor || it == ItemType.Container || it == ItemType.Weapon);
        }
    }
}
