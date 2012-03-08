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

            if (tile.Mobile.HasItems)
            {
                s.Append("Equipable Items:").Append(Console.Out.NewLine);

                var equipableItems = tile.Mobile.Items.Where(it => it.ItemTypes.HasFlag(ItemType.Weapon));

                int i = 1;
                foreach (var item in equipableItems)
                {
                    item.ListTag = InventoryCommand.letters[i];
                    s.Append("   ").Append(InventoryCommand.letters[i]).Append(") ");
                    s.Append(item.Name).Append(Console.Out.NewLine);
                    i++;
                }

                s.Append(Console.Out.NewLine).Append("Press letter of item to equip: ");
                Status.Info = s.ToString();
                Console.Clear();
                Status.WriteToStatus();

                var itemKey = Console.ReadKey(true);

                var itemToWear = equipableItems.Where(e => e.ListTag.ToUpper() == itemKey.KeyChar.ToString().ToUpper()).First();

                tile.Mobile.WearItem(itemToWear);   // <====  !!!!  Need to code this method


                //TODO MWK -- Working HERE -- 1) need to link letters to items so that can know which item to equip
                //                            2) prompt for which equipment slot
                //                            3) validate item can go in that slot
                //                            4) put in slot
                //                            5) display new equipment list with modified slot highlighted (and maybe some text to say what was taken off)







                s.Append(Console.Out.NewLine).Append(Console.Out.NewLine).Append("You are now wearing => ").Append(itemToWear.Name);


            }
            else
            {
                s.Append("You have no equipable items.").Append(Console.Out.NewLine);
            }

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