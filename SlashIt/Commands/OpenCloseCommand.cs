using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class OpenCloseCommand : ICommand
    {
        Map map;

        public OpenCloseCommand(Map map)
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

            var tileToUse = this.map.GetTileToMoveTo(localKeyInfo, mapLocation);

            if (tileToUse.UniqueId == Constants.UniqueIds.Door)
            {
                map.Tiles.Remove(tileToUse);
                map.Tiles.Add(new OpenDoor() { Location = tileToUse.Location });
            }
            else if (tileToUse.UniqueId == Constants.UniqueIds.OpenDoor)
            {
                map.Tiles.Remove(tileToUse);
                map.Tiles.Add(new Door() { Location = tileToUse.Location });
            }
            else
            {
                Status.Info = "That can't be opened or closed.  Press any key to continue.";
                Status.WriteToStatus();
                Console.ReadKey(true);
            }

            Status.ClearInfo();

            this.map.MapOutdated = true;
        }
    }
}
