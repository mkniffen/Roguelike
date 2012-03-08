using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SlashIt
{
    public enum ItemType : int
    {
        None = 0,
        Weapon = 1,
        Head = 2,
        Chest = 4,
        Feet = 8,
        Container = 16
    }


    public class Item
    {

        public static List<Item> availableItems = new List<Item>
        {
            { new Item {Name = "Bob's wallet", DisplayCharacter="~", ItemTypes = ItemType.Container, ItemId = Constants.ItemIds.Wallet, Description = "You found Bob's wallet.  He's gonna be REAL mad if he finds out you have it"}},
            { new Item { Name = "Dagger", DisplayCharacter="|", ItemTypes = ItemType.Weapon, ItemId = Constants.ItemIds.Dagger, Description = "It's a plain dagger"}},
        };

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayCharacter { get; set; }
        public int ItemId { get;set; }
        public string ListTag { get; set; }

        public ItemType ItemTypes { get; set; }


        public static Item GetItemById(int itemId)
        {
 	        return availableItems.Where(i => i.ItemId == itemId).Single<Item>();
        }

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
    }
}
