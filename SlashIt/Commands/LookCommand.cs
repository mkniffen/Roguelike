using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class LookCommand : ICommand
    {
        Map map;

        public LookCommand(Map map)
        {
            this.map = map;
        }

        public void execute(LocalKeyInfo keyInfo)
        {
            //TODO -- will need to modify this so that all the items in the tile are put in the Info

            Status.Info = this.map.availableTiles[this.map.GetPlayerTile().TypeId].Description;
        }
    }
}
