using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class Tile
    {
        public int TypeId { get; set; }
        public Location Location { get; set; }
        public Mobile Mobile { get; set; }
        public Item Item { get; set; }

        public bool HasItem()
        {
            return Item != null;
        }
    }

    public class TileDetail
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayCharacter { get; set; }
    }
}
