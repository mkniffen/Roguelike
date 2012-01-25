using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public interface INonPlayerCharacter
    {
        void UpdateState(Map map, Tile nonPlayerCharacterTile);
    }
}
