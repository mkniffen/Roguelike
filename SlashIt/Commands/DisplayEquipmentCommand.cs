using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    //Put on or take off an item
    public class DisplayEquipmentCommand : ICommand
    {
        //TODO -- also in InventoryCommand, move to higher level or util

        //Dictionary<int, string> letters = new Dictionary<int, string>
        //{
        //    {1, "a"}, {2,"b"}, {3,"c"}, {4,"d"}, {5,"e"}, {6,"f"}, {7,"g"}, {8,"h"}, {9,"i"}
        //};

        Map map;

        public DisplayEquipmentCommand(Map map)
        {
            this.map = map;
        }

        public bool execute(LocalKeyInfo keyInfo)
        {
            //Tile tile = this.map.GetPlayerTile();
            //StringBuilder s = new StringBuilder();

            //if (tile.Mobile.Items.Count > 0)
            //{
            //    s.Append("Player Inventory:").Append(Console.Out.NewLine);

            //    int i = 1;
            //    foreach (var item in tile.Mobile.Items)
            //    {
            //        s.Append("   ").Append(letters[i]).Append(") ");
            //        s.Append(item.Name).Append(Console.Out.NewLine);
            //        i++;
            //    }
            //}
            //else
            //{
            //    s.Append("Your inventory is empty!!").Append(Console.Out.NewLine);
            //}

            //s.Append(Console.Out.NewLine).Append("Press any key to continue.").Append(Console.Out.NewLine);

            //Status.Info = s.ToString();

            //Console.Clear();
            //Status.WriteToStatus();

            //Console.ReadKey(true);
            //Status.ClearInfo();

            //Console.Clear();

            //this.map.Outdated = true;

            return true;
        }
    }
}