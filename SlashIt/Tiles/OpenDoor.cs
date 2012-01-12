using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class OpenDoor : Tile
    {
        public OpenDoor()
        {
            Description = "An open door";
            DisplayCharacter = "`";
            Name = "OpenDoor";
            UniqueId = Constants.UniqueIds.OpenDoor;
        }
    }
}
