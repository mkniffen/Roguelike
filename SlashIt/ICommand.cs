using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public interface ICommand
    {
        void execute();
    }

    public class TestCommand : ICommand
    {
        public void execute()
        {
            Status.Info = "hello from Test Command";
        }
    }

    public class OpenCloseCommand : ICommand
    {
        Map map;

        public OpenCloseCommand(Map map)
        {
            this.map = map;
        }

        public void execute()
        {
            Status.ClearInfo();
            Status.Info = "Which direction?";
            Status.WriteToStatus();

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            var localKeyInfo = new LocalKeyInfo(keyInfo);

            //TODO -- Taking key input exists in more than one place now, REFACTOR

            var mapTile = map.GetPlayerTile();

            var mapLocation = new Location(mapTile.Location.Left, mapTile.Location.Top);

            var tileToUse = this.map.GetTileToMoveTo(localKeyInfo, mapLocation);

            if (tileToUse.UniqueId == Constants.UniqueIds.Door)
            {
                map.MapObjects.Remove(tileToUse);
                map.MapObjects.Add(new OpenDoor() { Location = tileToUse.Location });
            }
            else if(tileToUse.UniqueId == Constants.UniqueIds.OpenDoor)
            {
                map.MapObjects.Remove(tileToUse);
                map.MapObjects.Add(new Door() { Location = tileToUse.Location });
            }
            else
            {
                Status.Info = "That can't be opened or closed.  Press any key to continue.";
                Status.WriteToStatus();
                Console.ReadKey(true);
            }

            Status.ClearInfo();
        }
    }
}
