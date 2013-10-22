using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SlashIt
{
    public sealed class AvailableItems
    {
        static readonly AvailableItems instance = new AvailableItems();

        public List<Item> AllItems = new List<Item>();
        //{
        //    { new Item {Name = "Bob's wallet", DisplayCharacter="~", ItemTypes = new List<ItemType> { ItemType.Armor, ItemType.Container}, ItemLocations = new List<ItemLocation> { ItemLocation.Bag1 }, ItemId = Constants.ItemIds.Wallet, Description = "You found Bob's wallet.  He's gonna be REAL mad if he finds out you have it"}},
        //    { new Item { Name = "Dagger", DisplayCharacter="|", ItemTypes = new List<ItemType> { ItemType.Weapon }, 
        //ItemLocations = new List<ItemLocation> { ItemLocation.HandRight, ItemLocation.HandLeft }, ItemId = Constants.ItemIds.Dagger, Description = "It's a plain dagger"}},
        //};
        
        AvailableItems()
        {
            XDocument doc = XDocument.Load(@".\Config\Items.xml");

            foreach (XElement item in doc.Descendants("Items").Descendants("Item"))
            {
                var itemToAdd = new Item();
                itemToAdd.Name = item.Element("Name").Value;
                itemToAdd.DisplayCharacter = item.Element("DisplayCharacter").Value;
                itemToAdd.ItemTypes = item.Descendants("ItemTypes").Descendants("ItemType").Select(it => itemToAdd.ItemTypeDictionary[it.Value]).ToList();
                itemToAdd.ItemLocations = item.Descendants("ItemLocations").Descendants("ItemLocation").Select(il => itemToAdd.ItemLocationDictionary[il.Value]).ToList();

                //TODO !!!  Add this    itemToAdd.ItemId....

                this.AllItems.Add(itemToAdd);
            }
        }

        public static AvailableItems Instance
        {
            get { return instance; }
        }

        public Item GetItemById(int itemId)
        {
            return this.AllItems.Where(i => i.ItemId == itemId).Single<Item>();
        }
    }
}