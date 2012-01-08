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

            var keyInfo = Console.ReadKey(true);

            //TODO -- Taking key input exists in more than one place now, REFACTOR

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                Status.ClearInfo();
                return;
            }

            var mapTile = map.GetPlayerTile();

            var mapLocation = new Location(mapTile.Location.Left, mapTile.Location.Top);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    mapLocation.Top--;
                    break;

                case ConsoleKey.DownArrow:
                    mapLocation.Top++;
                    break;

                case ConsoleKey.LeftArrow:
                    mapLocation.Left--;
                    break;

                case ConsoleKey.RightArrow:
                    mapLocation.Left++;
                    break;

                default:
                    break;
            }

            var tileToUse = map.GetTileForLocation(mapLocation);

            if (tileToUse.UniqueId != Constants.UniqueIds.Door)
            {
                Status.Info = "That can't be opened or closed.  Press any key to continue.";
                Status.WriteToStatus();

                Console.ReadKey(true);
                this.execute();

                return;
            }

            map.MapObjects.Remove(tileToUse);
            map.MapObjects.Add(new OpenDoor() { Location = tileToUse.Location });
        }
    }
}
