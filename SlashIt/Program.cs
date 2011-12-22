using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    class Program
    {
        static bool quit = false;
        static Game game;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

            game = new Game();
            game.InitConsole();

            MainLoop();

        }

        private static void MainLoop()
        {
            while (!quit)
            {
                game.WriteConsole();
                HandleInput();
            }

            Console.WriteLine("Game over!");
        }




        

        /// <summary>
        /// Event handler for ^C key press
        /// </summary>
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            // Unfortunately, due to a bug in .NET Framework v4.0.30319 you can't debug this 
            // because Visual Studio 2010 gives a "No Source Available" error. 
            // http://connect.microsoft.com/VisualStudio/feedback/details/524889/debugging-c-console-application-that-handles-console-cancelkeypress-is-broken-in-net-4-0

            quit = true;
            e.Cancel = true; // Set this to true to keep the process from quitting immediately
        }

        private static void HandleInput()
        {
            var keyInfo = Console.ReadKey(true);
            Status.Message = "Key: " + keyInfo.Key;

            if (keyInfo.Key == ConsoleKey.Q)
            {
                quit = true;
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                game.character.MoveRight();
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                game.character.MoveLeft();
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                game.character.MoveUp();
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                game.character.MoveDown();
            }

        }
    }


    public class Game
    {
        public Character character;

        private Map map;

        public void InitConsole()
        {

            Console.Clear();
            Console.SetWindowSize(80, 25);
            Console.BufferWidth = 80;
            Console.BufferHeight = 25;

            Console.CursorVisible = false;

            character = new Character();
            character.SetPosition(22, 4);  //TODO Const

            map = new Map();

            this.WriteConsole();

            Status.Message = "Height: " + Console.WindowHeight + " Width: " + Console.WindowWidth;
        }


        //TODO eventually optimize how and when things are drawn.  don't necessrily need to redraw all the time.
        public void WriteConsole()
        {
            Console.Clear();

            this.GenerateMap();  //TODO only do this when really needed

            Console.SetCursorPosition(character.Left, character.Top);
            Console.Write("@");

            Status.WriteToStatusLine();

            //TODO add console bounds check
        }


        internal void GenerateMap()
        {
            int mapStartLeft = 20;
            int mapStartTop = 3;

            Console.SetCursorPosition(mapStartLeft, mapStartTop);

            for (int i = 0; i <= this.map.map.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= this.map.map.GetUpperBound(1); j++)
                {
                    switch (this.map.map[i,j])
                    {
                        case (1):
                            Console.Write("#");
                            break;
                        case (0):
                            Console.Write(" ");
                            break;
                        default:
                            break;
                    }

                }

                Console.SetCursorPosition(mapStartLeft, ++mapStartTop);

            }

        }
    }


    public class Status
    {
        private const int MessagePositionDefaultLeft = 0;
        private const int MessagePositionDefaultTop = 24;

        public static string Message { get; set; }

        public static int MessagePositionLeft { get; set; }
        public static int MessagePositionTop { get; set; }

        public Status()
        {
            MessagePositionLeft = Status.MessagePositionDefaultLeft;
            MessagePositionTop = Status.MessagePositionDefaultTop;
        }

        public static void WriteToStatusLine()
        {
            var originalCursonPositionLeft = Console.CursorLeft;
            var originalCursonPositionTop = Console.CursorTop;
            
            Console.SetCursorPosition(MessagePositionDefaultLeft, MessagePositionDefaultTop);
            Console.Write(Message);

            Console.SetCursorPosition(originalCursonPositionLeft, originalCursonPositionTop);
        }


        public static void WriteToStatusLine(string status)
        {
            Message = status;
            WriteToStatusLine();
        }
    }

    public class Character
    {
        public int Left { get; set; }
        public int Top { get; set; }

        public void SetPosition(int left, int top)
        {
            this.Left = left;
            this.Top = top;
        }

        public void MoveRight()
        {
            this.Left++;
        }

        public void MoveLeft()
        {
            this.Left--;
        }

        public void MoveUp()
        {
            this.Top--;
        }

        public void MoveDown()
        {
            this.Top++;
        }

    }


    public class Map
    {
        public int[,] map = new int[10, 10] 
            { { 1,1,1,1,1,1,1,1,1,1 }, 
              { 1,0,0,0,0,0,0,0,1,1 }, 
              { 1,1,1,1,1,1,1,0,1,1 }, 
              { 1,1,1,1,1,1,1,0,1,1 }, 
              { 1,1,0,0,0,0,0,0,1,1 }, 
              { 1,1,1,1,1,0,1,1,1,1 }, 
              { 1,1,1,1,1,0,1,1,1,1 }, 
              { 1,1,1,1,1,0,1,1,1,1 }, 
              { 1,1,0,0,0,0,0,0,0,1 }, 
              { 1,1,1,1,1,1,1,1,1,1 }, 
            };






        //Console.Write("###########################"); Console.SetCursorPosition(mapStartLeft, mapStartTop + 1);
        //Console.Write("#               +       ###"); Console.SetCursorPosition(mapStartLeft, mapStartTop + 2);
        //Console.Write("############### ###########"); Console.SetCursorPosition(mapStartLeft, mapStartTop + 3);
        //Console.Write("############### #####    ##"); Console.SetCursorPosition(mapStartLeft, mapStartTop + 4);
        //Console.Write("########        ##### #####"); Console.SetCursorPosition(mapStartLeft, mapStartTop + 5);
        //Console.Write("######## ############ #####"); Console.SetCursorPosition(mapStartLeft, mapStartTop + 6);
        //Console.Write("########              #####"); Console.SetCursorPosition(mapStartLeft, mapStartTop + 7);
        //Console.Write("###########################");
            

    }
}
