﻿using System;
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

        public bool execute(LocalKeyInfo keyInfo)
        {
            Tile tile = this.map.GetPlayerTile();
            var availableItems = AvailableItems.Instance;

            if (tile.HasItem())
            {
                Status.Info = availableItems.All[tile.TypeId].Description;
            }
            else 
            {
                Status.Info = Map.availableTiles[tile.TypeId].Description;
            }

            return true;
        }
    }
}
