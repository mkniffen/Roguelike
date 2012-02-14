using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class InventoryCommand : ICommand
    {


        //TODO WORKING still a few ### showing up in middle of inventory (after picking up walet)


        Map map;

        public InventoryCommand(Map map)
        {
            this.map = map;
        }

        public bool execute(LocalKeyInfo keyInfo)
        {
            Tile tile = this.map.GetPlayerTile();
            StringBuilder s = new StringBuilder();

            if (tile.Mobile.Items.Count > 0)
            {
                s.Append("Player Inventory:").Append(Console.Out.NewLine);

                foreach (var item in tile.Mobile.Items)
                {
                    s.Append(item.Name).Append(Console.Out.NewLine);
                }
            }
            else
            {
                s.Append("Your inventory is empty!!").Append(Console.Out.NewLine);
            }

            s.Append("Press any key to continue").Append(Console.Out.NewLine);

            Status.Info = s.ToString();
            Status.WriteToStatus();

            Console.ReadKey(true);
            Status.ClearInfo();

            Console.Clear();
            this.map.Outdated = true;

            return true;
        }
    }
}