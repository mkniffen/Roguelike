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

        public bool execute(LocalKeyInfo keyInfo)
        {
            var mapTile = this.map.GetPlayerTile();
            this.map.MoveMobile(keyInfo, mapTile);
            this.map.Outdated = true;

            return false;
        }
    }
}
