using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class QuitCommand : ICommand
    {
        Map map;

        public QuitCommand(Map map)
        {
            this.map = map;
        }

        public void execute(LocalKeyInfo keyInfo)
        {
            bool quit = true;

            Status.ClearInfo();
            Status.Info = "Really quit (Y or N)?";
            Status.WriteToStatus();

            var consoleKeyInfo = Console.ReadKey(true);

            if (consoleKeyInfo.Key == ConsoleKey.Y)
            {
                Program.Save();
            }
            else if (consoleKeyInfo.Key == ConsoleKey.N)
            {
                Status.ClearInfo();
                Status.Info = "Returning to game.";
                quit = false;
            }
            else
            {
                this.execute(keyInfo);
            }

            Program.Quit = quit;
        }
    }
}
