using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class PickUpCommand : ICommand
    {
        Map map;

        public PickUpCommand(Map map)
        {
            this.map = map;
        }

        public void execute(LocalKeyInfo keyInfo)
        {
            Tile tile = this.map.GetPlayerTile();
            if (tile.HasItem())
            {
                tile.Mobile.Items.Add(tile.Item);
                Status.Info = "You take " + tile.Item.Name + ".";
                tile.Item = null;
            }
            else 
            {
                Status.Info = "This is nothing to pick up here.";
            }
        }
    }
}
