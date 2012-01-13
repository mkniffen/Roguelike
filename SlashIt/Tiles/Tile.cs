using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class Tile
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayCharacter { get; set; }
        public int UniqueId { get; set; }

        public Location Location { get; set; }
        public Mobile Mobile { get; set; }
    }
}
