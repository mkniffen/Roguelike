using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class PickUpCommand : ICommand
    {
        Map map;


        //TODO !!!!!!!  WORKING HERE  !!!!!!  Implement inventory


        public PickUpCommand(Map map)
        {
            this.map = map;
        }

        public void execute(LocalKeyInfo keyInfo)
        {
            Tile tile = this.map.GetPlayerTile();
            if (tile.HasItem())
            {
                Status.Info = "You take " + tile.Item.Name + ".";
            }
            else 
            {
                Status.Info = "This is nothing to pick up here.";
            }
        }
    }
}
