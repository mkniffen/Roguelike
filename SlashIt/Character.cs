﻿using System;
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

        public void SetPositionBeforeMove()
        {
            this.LeftBeforeMove = this.Left;
            this.TopBeforeMove = this.Top;
        }

        public void Move(ConsoleKey consoleKey, Map map)
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

        //TODO look at moving to this style of move throughout
        public Location Move(ConsoleKey consoleKey)
        {
            var mapLocation = new Location();
            mapLocation.Top = TopMapPosition;
            mapLocation.Left = LeftMapPosition;

            switch (consoleKey)
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

            return mapLocation;
        }
    }
}
