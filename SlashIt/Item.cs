using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SlashIt
{
    

    public class Item
    {
        public static List<Item> availableItems = new List<Item>
        {
            { new Item { Name = "Bob's wallet", DisplayCharacter="~", TypeId=100000, Description = "You found Bob's wallet.  He's gonna be REAL mad if he finds out you have it"}},
        };

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayCharacter { get; set; }
        public int TypeId { get;set; }

        public static Item GetItemById(int itemId)
        {
 	        return availableItems.Where(i => i.TypeId == itemId).Single<Item>();
        }

        public void Load(XElement item)
        {
 	        this.TypeId = Int32.Parse(item.Element("TypeId").Value);
        }

        public XElement Save()
        {
 	        return new XElement("Item",
                new XElement("TypeId", this.TypeId)
                );
        }
    }
}
