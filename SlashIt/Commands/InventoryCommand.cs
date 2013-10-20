using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class InventoryCommand : ICommand
    {
        //TODO -- also in InventoryCommand, move to higher level or util

        public static Dictionary<int, string> Letters = new Dictionary<int, string>
        {
            {1, "a"}, {2,"b"}, {3,"c"}, {4,"d"}, {5,"e"}, {6,"f"}, {7,"g"}, {8,"h"}, {9,"i"}
        };

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
            //if (tile.Mobile.HasEquipableItems)
            {
                s.Append("Player Inventory:").Append(Console.Out.NewLine);

                int i = 1;
                foreach (var item in tile.Mobile.Items)
                {
                    item.ListTag = Letters[i];
                    s.Append("   ").Append(Letters[i]).Append(") ");
                    s.Append(item.Name).Append(Console.Out.NewLine);
                    i++;
                }
            }
            else
            {
                s.Append("Your inventory is empty!!").Append(Console.Out.NewLine);
            }

            s.Append(Console.Out.NewLine).Append("Press any key to continue.").Append(Console.Out.NewLine);

            Status.Info = s.ToString();

            Console.Clear();
            Status.WriteToStatus();

            Console.ReadKey(true);
            Status.ClearInfo();

            Console.Clear();

            this.map.Outdated = true;

            return true;
        }
    }
}