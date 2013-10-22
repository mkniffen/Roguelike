using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using SlashIt;

namespace Tests.SlashIt
{
    public class SlashIt
    {
        // Items are loaded from a config file(Items.xml)
        [Fact]
        public void AvailableItemsIsNotEmpty()
        {
            var availableItems = AvailableItems.Instance;
            Assert.NotEmpty(availableItems.AllItems); 
        }


        //Test for AvailableItems to know it's a singleton?
    }
}