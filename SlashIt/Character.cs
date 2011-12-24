using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{

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
}
