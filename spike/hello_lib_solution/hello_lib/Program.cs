namespace hello_libtcod
{
    using System;
    using libtcod;

    static class Program
    {
        [STAThread]
        static void Main()
        {
            TCODConsole.initRoot(80, 50, "my game", false);
            TCODSystem.setFps(25);
            bool endGame = false;
            while (!endGame && !TCODConsole.isWindowClosed())
            {
                TCODConsole.root.print(0, 0, "Hello, world");
                TCODConsole.flush();
                var key = TCODConsole.checkForKeypress();

                if (key.KeyCode == TCODKeyCode.Escape)
                {
                    endGame = true;
                }
            }
        }
    }
}
