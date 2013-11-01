using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using SlashIt;

namespace Tests.SlashIt
{
    public class AvailableItemsTests
    {
        //TODO Possible Tests
        //
        //  --Tests around what are required fields on an Item
        //  


        // Items are loaded from a config file(Items.xml)
        [Fact]
        public void AvailableItemsIsNotEmpty()
        {
            var availableItems = AvailableItems.Instance;
            Assert.NotEmpty(availableItems.All); 
        }

        [Fact]
        public void GetForIdReturnsAppropriateItem()
        {
            int itemId = 1;
            int itemId2 = 2;

            var availableItems = AvailableItems.Instance;
            availableItems.All = new List<Item> 
            {
                { new Item { Name = "Dagger", DisplayCharacter="|", ItemTypes = new List<Item.ItemType> { Item.ItemType.Weapon }, ItemLocations = new List<Item.ItemLocation> { Item.ItemLocation.HandRight, Item.ItemLocation.HandLeft }, ItemId = itemId, Description = "It's a plain dagger"}},
                { new Item { Name = "D", DisplayCharacter="|", ItemTypes = new List<Item.ItemType> { Item.ItemType.Weapon }, ItemLocations = new List<Item.ItemLocation> { Item.ItemLocation.HandRight, Item.ItemLocation.HandLeft }, ItemId = itemId2, Description = "It is"}}
            };

            var item = availableItems.GetItemById(itemId);
            Assert.Equal("Dagger", item.Name);
            Assert.NotEqual("D", item.Name);
        }
    }
}