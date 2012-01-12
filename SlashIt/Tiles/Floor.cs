using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class Floor : Tile
    {
        public Floor()
        {
            Description = "Empty floor";
            DisplayCharacter = " ";
            Name = "Floor";
            UniqueId = Constants.UniqueIds.Floor;
        }
    }
}
