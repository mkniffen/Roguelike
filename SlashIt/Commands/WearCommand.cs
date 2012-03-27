using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    //Put on or take off an item
    public class WearCommand : ICommand
    {
        Map map;

        public WearCommand(Map map)
        {
            this.map = map;
        }

        public bool execute(LocalKeyInfo keyInfo)
        {
            Tile tile = this.map.GetPlayerTile();
            StringBuilder s = new StringBuilder();

            if (tile.Mobile.HasEquipableItems)
            {
                s.Append("Equipable Items:").Append(Environment.NewLine);

                var equipableItems = tile.Mobile.Items.Where(it => it.IsEquipable());

                int i = 1;
                foreach (var item in equipableItems)
                {
                    item.ListTag = InventoryCommand.letters[i];
                    s.Append("   ").Append(InventoryCommand.letters[i]).Append(") ");
                    s.Append(item.Name).Append(Environment.NewLine);
                    i++;
                }

                s.Append(Environment.NewLine).Append("Press letter of item to equip: ");
                Status.Info = s.ToString();
                Console.Clear();
                Status.WriteToStatus();

                var itemKey = Console.ReadKey(true);

                var itemToWear = equipableItems.Where(e => e.ListTag.ToUpper() == itemKey.KeyChar.ToString().ToUpper()).First();

                tile.Mobile.WearItem(itemToWear);

                s.Append(Environment.NewLine).Append(Environment.NewLine).Append("You are now wearing => ").Append(itemToWear.Name);
            }
            else
            {
                s.Append("You have no equipable items.").Append(Environment.NewLine);
            }

            s.Append(Environment.NewLine).Append(Environment.NewLine).Append("Press any key to conintue");

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