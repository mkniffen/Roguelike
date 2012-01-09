using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class MoveMapPlayerCommand : ICommand
    {
        Map map;

        public MoveMapPlayerCommand(Map map)
        {
            this.map = map;
        }

        public void execute(LocalKeyInfo keyInfo)
        {
            var mapTile = this.map.GetPlayerTile();

            var mapLocation = new Location(mapTile.Location.Left, mapTile.Location.Top);

            var tileToMoveTo = this.map.GetTileToMoveTo(keyInfo, mapLocation);

            if (mapTile.Player.CanMoveTo(tileToMoveTo))
            {
                tileToMoveTo.Player = mapTile.Player;
                tileToMoveTo.Player.Location = mapLocation;
                mapTile.Player = null;
            }
        }
    }
}
