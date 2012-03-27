using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SlashIt
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


    public class Item
    {

        public static List<Item> availableItems = new List<Item>
        {
            { new Item {Name = "Bob's wallet", DisplayCharacter="~", ItemTypes = new List<ItemType> { ItemType.Armor, ItemType.Container}, ItemLocations = new List<ItemLocation> { ItemLocation.Bag1 }, ItemId = Constants.ItemIds.Wallet, Description = "You found Bob's wallet.  He's gonna be REAL mad if he finds out you have it"}},
            { new Item { Name = "Dagger", DisplayCharacter="|", ItemTypes = new List<ItemType> { ItemType.Weapon }, ItemLocations = new List<ItemLocation> { ItemLocation.HandRight, ItemLocation.HandLeft }, ItemId = Constants.ItemIds.Dagger, Description = "It's a plain dagger"}},
        };

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayCharacter { get; set; }
        public int ItemId { get;set; }
        public string ListTag { get; set; }

        public List<ItemLocation> ItemLocations { get; set; }
        public List<ItemType> ItemTypes { get; set; }



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

        public bool IsEquipable()
        {
            return  this.ItemTypes.Any(it => it == ItemType.Armor || it == ItemType.Container || it == ItemType.Weapon);
        }
    }
}
