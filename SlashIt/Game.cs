using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{

    public class Game
    {
        public Character character;

        public Map Map { get; set; }
        public const short MapStartLeft = 20;
        public const short MapStartTop = 1;

        public void InitConsole()
        {

            Console.Clear();
            Console.SetWindowSize(80, 25);
            Console.BufferWidth = 80;
            Console.BufferHeight = 25;

            Console.CursorVisible = false;

            character = new Character();
            character.SetPosition(22, 2);  //TODO Const

            Map = new Map();

            this.WriteConsole();

            Status.Message = "Height: " + Console.WindowHeight + " Width: " + Console.WindowWidth;
        }


        //TODO eventually optimize how and when things are drawn.  don't necessarily need to redraw all the time.
        //TODO maybe have use a second array that tracks what should be drawn on the grid.
        public void WriteConsole()
        {

            if (Map.MapOutdated)
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

            Status.WriteToStatus();

            //TODO add console bounds check
        }


        internal void GenerateMap()
        {
            int mapStartLeft = Game.MapStartLeft;
            int mapStartTop = Game.MapStartTop;

            Console.SetCursorPosition(mapStartLeft, mapStartTop);

            for (int i = 0; i <= this.Map.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= this.Map.GetUpperBound(1); j++)
                {
                    //TODO -- move this stuff to enum and or definig class soon!!!!!!!!!!!!

                    switch (this.Map[i, j])
                    {
                        case (0):
                            //Hallway
                            Console.Write(" ");
                            break;
                        case (1):
                            //Wall
                            Console.Write("#");
                            break;
                        case (2):
                            //Closed Door
                            Console.Write("+");
                            break;
                        case (3):
                            //Open Door
                            Console.Write("`");
                            break;
                        default:
                            break;
                    }
                }

                Console.SetCursorPosition(mapStartLeft, ++mapStartTop);

            }

            Map.MapOutdated = false;

        }

        public void CheckBounds()
        {
            //TODO -- Improve this
            if (Map[character.TopMapPosition, character.LeftMapPosition] != 0 && Map[character.TopMapPosition, character.LeftMapPosition] != 3)
            {
                character.DisallowMove();
            }
        }

        //TODO -- Refactor!!!!!!!!!!
        public void DoLook()
        {

                           // TODO change access to matrix to method calls throughout
            switch (Map[character.TopMapPosition, character.LeftMapPosition])
            {
                case (0):
                    Status.Info = "You see empty floor";
                    break;
                case (2):
                    Status.Info = "A big wooden door";
                    break;
                case (3):
                    Status.Info = "An open door";
                    break;
                default:
                    break;
            }
        }


        //TODO WORKING HERE -- do more testing on open
        // -- currently open door is not redrawn at the proper time
        // -- Need a way to get out of DoOpen when deciding not to open a door
        // -- Implement Close door



        public void DoOpen()
        {
            Status.ClearInfo();
            Status.Info = "Open which direction?";
            Status.WriteToStatus();

            var keyInfo = Console.ReadKey(true);

            //TODO -- Taking key input exists in more than one place now, REFACTOR

            var mapLocation = character.Move(keyInfo.Key);

            switch (Map[mapLocation.Top, mapLocation.Left])
            {
                case (2):
                    //Replace closed door with open door in map matrix
                    Map[mapLocation.Top, mapLocation.Left] = 3;
                    Map.MapOutdated = true;
                    break;
                default:
                    Status.ClearInfo();
                    Status.Info = "That can't be opened.  Press any key to continue";
                    Console.ReadKey(true);
                    DoOpen();
                    break;
            }
        }

    }

    public class Location
    {
        public int Top { get; set; }
        public int Left { get; set; }
    }
}