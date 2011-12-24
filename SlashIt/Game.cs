using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{

    public class Game
    {
        public Character character;

        private Map map;
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

            Status.WriteToStatus();

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
                    switch (this.map.map[i, j])
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

        public void CheckBounds()
        {
            //TODO -- Improve this
            if (map.map[character.TopMapPosition, character.LeftMapPosition] != 0)
            {
                character.DisallowMove();
            }
        }

        //TODO -- Refactor!!!!!!!!!!
        public void DoLook()
        {
            if (map.map[character.TopMapPosition, character.LeftMapPosition] == 0)
            {
                Status.Info = "You see empty floor";
            }
        }
    }
}
