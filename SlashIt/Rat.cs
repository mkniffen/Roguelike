using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class Rat : Mobile, INonPlayerCharacter
    {
        public Rat()
        {
            //TODO -- !!!!!! Hard coding for now.  This will probably be part of a strategy (or maybe factory) pattern.
            //                                     Could do using a container too.

            this.Name = "Rat";
            this.Description = "A simple rat that wants to EAT you!";
            this.DisplayCharacter = "r";
            this.UniqueId = Constants.UniqueIds.Rat;
        }

        public LocalKeyInfo GetDirectionToMove()
        {

            //TODO WORKING HERE -- Adding AI for NPC.  Figure out how to move


            var direction = Program.RandomNumber(5);

            switch (direction)
            {
                case 0:
                    return null;
                case 1:
                    return new LocalKeyInfo(ConsoleKey.UpArrow, false, false, false);
                case 2:
                    return new LocalKeyInfo(ConsoleKey.DownArrow, false, false, false);
                case 3:
                    return new LocalKeyInfo(ConsoleKey.LeftArrow, false, false, false);
                case 4:
                    return new LocalKeyInfo(ConsoleKey.RightArrow, false, false, false);
                default:
                    return null;
            }


        }

        //TODO -- Add Load/Save (very basic for now...)
    }
}
