using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class Wall : Tile
    {
        public Wall()
        {
            Description = "A brick wall";
            DisplayCharacter = "#";
            Name = "Wall";
            UniqueId = Constants.UniqueIds.Wall;
        }
    }
}
