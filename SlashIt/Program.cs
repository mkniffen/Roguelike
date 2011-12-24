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
                game.CheckBounds();
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
          //  Status.Message = "Key: " + keyInfo.Key;


            switch (keyInfo.Key)
            {
                case ConsoleKey.A:
                    break;
                case ConsoleKey.Add:
                    break;
                case ConsoleKey.Applications:
                    break;
                case ConsoleKey.Attention:
                    break;
                case ConsoleKey.B:
                    break;
                case ConsoleKey.Backspace:
                    break;
                case ConsoleKey.BrowserBack:
                    break;
                case ConsoleKey.BrowserFavorites:
                    break;
                case ConsoleKey.BrowserForward:
                    break;
                case ConsoleKey.BrowserHome:
                    break;
                case ConsoleKey.BrowserRefresh:
                    break;
                case ConsoleKey.BrowserSearch:
                    break;
                case ConsoleKey.BrowserStop:
                    break;
                case ConsoleKey.C:
                    break;
                case ConsoleKey.Clear:
                    break;
                case ConsoleKey.CrSel:
                    break;
                case ConsoleKey.D:
                    break;
                case ConsoleKey.D0:
                    break;
                case ConsoleKey.D1:
                    break;
                case ConsoleKey.D2:
                    break;
                case ConsoleKey.D3:
                    break;
                case ConsoleKey.D4:
                    break;
                case ConsoleKey.D5:
                    break;
                case ConsoleKey.D6:
                    break;
                case ConsoleKey.D7:
                    break;
                case ConsoleKey.D8:
                    break;
                case ConsoleKey.D9:
                    break;
                case ConsoleKey.Decimal:
                    break;
                case ConsoleKey.Delete:
                    break;
                case ConsoleKey.Divide:
                    break;
                case ConsoleKey.DownArrow:
                    game.character.Move(ConsoleKey.DownArrow);
                    break;
                case ConsoleKey.E:
                    break;
                case ConsoleKey.End:
                    break;
                case ConsoleKey.Enter:
                    break;
                case ConsoleKey.EraseEndOfFile:
                    break;
                case ConsoleKey.Escape:
                    break;
                case ConsoleKey.ExSel:
                    break;
                case ConsoleKey.Execute:
                    break;
                case ConsoleKey.F:
                    break;
                case ConsoleKey.F1:
                    break;
                case ConsoleKey.F10:
                    break;
                case ConsoleKey.F11:
                    break;
                case ConsoleKey.F12:
                    break;
                case ConsoleKey.F13:
                    break;
                case ConsoleKey.F14:
                    break;
                case ConsoleKey.F15:
                    break;
                case ConsoleKey.F16:
                    break;
                case ConsoleKey.F17:
                    break;
                case ConsoleKey.F18:
                    break;
                case ConsoleKey.F19:
                    break;
                case ConsoleKey.F2:
                    break;
                case ConsoleKey.F20:
                    break;
                case ConsoleKey.F21:
                    break;
                case ConsoleKey.F22:
                    break;
                case ConsoleKey.F23:
                    break;
                case ConsoleKey.F24:
                    break;
                case ConsoleKey.F3:
                    break;
                case ConsoleKey.F4:
                    break;
                case ConsoleKey.F5:
                    break;
                case ConsoleKey.F6:
                    break;
                case ConsoleKey.F7:
                    break;
                case ConsoleKey.F8:
                    break;
                case ConsoleKey.F9:
                    break;
                case ConsoleKey.G:
                    break;
                case ConsoleKey.H:
                    break;
                case ConsoleKey.Help:
                    break;
                case ConsoleKey.Home:
                    break;
                case ConsoleKey.I:
                    break;
                case ConsoleKey.Insert:
                    break;
                case ConsoleKey.J:
                    break;
                case ConsoleKey.K:
                    break;
                case ConsoleKey.L:
                    break;
                case ConsoleKey.LaunchApp1:
                    break;
                case ConsoleKey.LaunchApp2:
                    break;
                case ConsoleKey.LaunchMail:
                    break;
                case ConsoleKey.LaunchMediaSelect:
                    break;
                case ConsoleKey.LeftArrow:
                    game.character.Move(ConsoleKey.LeftArrow);
                    break;
                case ConsoleKey.LeftWindows:
                    break;
                case ConsoleKey.M:
                    break;
                case ConsoleKey.MediaNext:
                    break;
                case ConsoleKey.MediaPlay:
                    break;
                case ConsoleKey.MediaPrevious:
                    break;
                case ConsoleKey.MediaStop:
                    break;
                case ConsoleKey.Multiply:
                    break;
                case ConsoleKey.N:
                    break;
                case ConsoleKey.NoName:
                    break;
                case ConsoleKey.NumPad0:
                    break;
                case ConsoleKey.NumPad1:
                    break;
                case ConsoleKey.NumPad2:
                    break;
                case ConsoleKey.NumPad3:
                    break;
                case ConsoleKey.NumPad4:
                    break;
                case ConsoleKey.NumPad5:
                    break;
                case ConsoleKey.NumPad6:
                    break;
                case ConsoleKey.NumPad7:
                    break;
                case ConsoleKey.NumPad8:
                    break;
                case ConsoleKey.NumPad9:
                    break;
                case ConsoleKey.O:
                    break;
                case ConsoleKey.Oem1:
                    break;
                case ConsoleKey.Oem102:
                    break;
                case ConsoleKey.Oem2:
                    break;
                case ConsoleKey.Oem3:
                    break;
                case ConsoleKey.Oem4:
                    break;
                case ConsoleKey.Oem5:
                    break;
                case ConsoleKey.Oem6:
                    break;
                case ConsoleKey.Oem7:
                    break;
                case ConsoleKey.Oem8:
                    break;
                case ConsoleKey.OemClear:
                    break;
                case ConsoleKey.OemComma:
                    break;
                case ConsoleKey.OemMinus:
                    break;
                case ConsoleKey.OemPeriod:
                    break;
                case ConsoleKey.OemPlus:
                    break;
                case ConsoleKey.P:
                    break;
                case ConsoleKey.Pa1:
                    break;
                case ConsoleKey.Packet:
                    break;
                case ConsoleKey.PageDown:
                    break;
                case ConsoleKey.PageUp:
                    break;
                case ConsoleKey.Pause:
                    break;
                case ConsoleKey.Play:
                    break;
                case ConsoleKey.Print:
                    break;
                case ConsoleKey.PrintScreen:
                    break;
                case ConsoleKey.Process:
                    break;
                case ConsoleKey.Q: 
                    quit = true;
                    break;
                case ConsoleKey.R:
                    break;
                case ConsoleKey.RightArrow:
                    game.character.Move(ConsoleKey.RightArrow);
                    break;
                case ConsoleKey.RightWindows:
                    break;
                case ConsoleKey.S:
                    break;
                case ConsoleKey.Select:
                    break;
                case ConsoleKey.Separator:
                    break;
                case ConsoleKey.Sleep:
                    break;
                case ConsoleKey.Spacebar:
                    break;
                case ConsoleKey.Subtract:
                    break;
                case ConsoleKey.T:
                    break;
                case ConsoleKey.Tab:
                    break;
                case ConsoleKey.U:
                    break;
                case ConsoleKey.UpArrow:
                    game.character.Move(ConsoleKey.UpArrow);
                    break;
                case ConsoleKey.V:
                    break;
                case ConsoleKey.VolumeDown:
                    break;
                case ConsoleKey.VolumeMute:
                    break;
                case ConsoleKey.VolumeUp:
                    break;
                case ConsoleKey.W:
                    break;
                case ConsoleKey.X:
                    break;
                case ConsoleKey.Y:
                    break;
                case ConsoleKey.Z:
                    break;
                case ConsoleKey.Zoom:
                    break;
                default:
                    break;
            };
        }
    }


    public class Game
    { 
        public Character character;

        private Map map;
        public const short MapStartLeft = 20;
        public const short MapStartTop = 3;

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


        //TODO eventually optimize how and when things are drawn.  don't necessarily need to redraw all the time.
        //TODO maybe have use a second array that tracks what should be drawn on the grid.
        public void WriteConsole()
        {

            if (map.MapOutdated)
            {
                Console.Clear();
                this.GenerateMap();  //TODO only do this when really needed
            }

            Console.SetCursorPosition(character.LeftBeforeMove, character.TopBeforeMove);
            Console.Write(" ");
            Console.SetCursorPosition(character.Left, character.Top);
            Console.Write("@");
            character.SetPositionBeforeMove();

            Status.Message = "Map Left: " + character.LeftMapPosition + " :MapTop: " + character.TopMapPosition;

            Status.WriteToStatusLine();

            //TODO add console bounds check
        }


        internal void GenerateMap()
        {
            int mapStartLeft = Game.MapStartLeft;
            int mapStartTop = Game.MapStartTop;

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

            map.MapOutdated = false;

        }

        internal void CheckBounds()
        {
            //TODO -- Improve this
            if (map.map[character.TopMapPosition, character.LeftMapPosition] != 0)
            {
                character.DisallowMove();
            }
        }
    }


    public class Status
    {
        //TODO: maybe change this to be inited on porg start so that it can very more easily 
        private static string clearLine = "                                                                               ";

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
            Console.Write(clearLine);

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

        public int LeftBeforeMove { get; set; }
        public int TopBeforeMove { get; set; }
        
        public int LeftMapPosition { get { return this.Left - Game.MapStartLeft; } }
        public int TopMapPosition { get { return this.Top - Game.MapStartTop; } }

        public void DisallowMove()
        {
            this.Left = this.LeftBeforeMove;
            this.Top = this.TopBeforeMove;
        }

        public void SetPosition(int left, int top)
        {
            this.Left = left;
            this.Top = top;

            this.SetPositionBeforeMove();
        }

        //public void MoveRight()
        //{
            
        //}

        //public void MoveLeft()
        //{
            
        //}

        //public void MoveUp()
        //{
        //    this.Top--;
        //}

        //public void MoveDown()
        //{
        //}

        public void SetPositionBeforeMove()
        {
            this.LeftBeforeMove = this.Left;
            this.TopBeforeMove = this.Top;
        }

        public void Move(ConsoleKey consoleKey)
        {
            switch (consoleKey)
            {
                case ConsoleKey.UpArrow:
                    this.Top--;
                    break;

                case ConsoleKey.DownArrow:
                    this.Top++;
                    break;

                case ConsoleKey.LeftArrow:
                    this.Left--;        
                    break;

                case ConsoleKey.RightArrow:
                    this.Left++;
                    break;

                default:
                    break;
            }
        }
    }


    public class Map
    {

        public Map()
        {
            MapOutdated = true;
        }

        public bool MapOutdated { get; set; }

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

    }
}
