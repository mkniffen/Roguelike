using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class AttackCommand : ICommand
    {
        Map map;

        public AttackCommand(Map map)
        {
            this.map = map;
        }

        public void execute(LocalKeyInfo keyInfo)
        {
            Status.ClearInfo();
            Status.Info = "Which direction?";
            Status.WriteToStatus();

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
            var localKeyInfo = new LocalKeyInfo(consoleKeyInfo);

            //TODO -- Taking key input exists in more than one place now, REFACTOR

            var mapTile = map.GetPlayerTile();
            var mapLocation = new Location(mapTile.Location.Left, mapTile.Location.Top);
            var tile = this.map.GetTileInDirection(localKeyInfo, mapLocation);

            if (tile.Mobile is INonPlayerCharacter)
            {
                tile.Mobile.HitPoints -= 5;
                var message = "You hit " + tile.Mobile.HitMessage + ".";
                if (tile.Mobile.IsDead())
                {
                    //TODO -- This logic should probably be moved.
                    //TODO -- The death message should be moved to the specific mobile (and then later to config)
                    message += "You killed it!!!";
                    tile.Mobile = null;
                    map.Outdated = true;
                }

                Status.Info = message;
            }
            else 
            {
                Status.Info = "Not a valid target.";
            }
        }
    }
}
