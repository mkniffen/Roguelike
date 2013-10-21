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
            Assert.NotEmpty(Item.AvailableItems); 
        }
    }
}