using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt.Tiles
{
    public class Door : Tile
    {
        public Door()
        {
            Description = "A big wooden door.  It's closed";
            DisplayCharacter = "+";
            Name = "Door";
            UniqueId = Constants.UniqueIds.Door;
        }
    }
}
