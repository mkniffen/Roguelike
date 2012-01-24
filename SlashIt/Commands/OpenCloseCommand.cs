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

            var tileToUse = this.map.GetTileInDirection(localKeyInfo, mapLocation);

            if (tileToUse.UniqueId == Constants.UniqueIds.Door)
            {
                this.map.ToggleDoor(tileToUse, false);
            }
            else if (tileToUse.UniqueId == Constants.UniqueIds.OpenDoor)
            {
                this.map.ToggleDoor(tileToUse, true);
            }
            else
            {
                Status.Info = "That can't be opened or closed.  Press any key to continue.";
                Status.WriteToStatus();
                Console.ReadKey(true);
            }

            Status.ClearInfo();

            this.map.Outdated = true;
        }
    }
}
