using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public interface IState
    {
        void Enter(Mobile mobile);
        void Execute(Mobile mobile, Map map, Tile nonPlayerCharacterTile);
        void Exit(Mobile mobile);
    }
}
