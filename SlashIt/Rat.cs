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
            this.HitPoints = 10;
            this.HitMessage = "the " + this.Name + " ";
        }

        public void UpdateState(Map map, Tile nonPlayerCharacterTile)
        {
            if (Program.RandomNumber(4) < 3)
            {
                //Move
                var direction = map.GetDirectionRandom();

                if (direction != null)
                {
                    map.MoveMobile(direction, nonPlayerCharacterTile);
                }
            }
            else
            {
                //Attack
                if (nonPlayerCharacterTile.Mobile.CanAttack(map, nonPlayerCharacterTile))
                {
                    map.GetPlayer().HitPoints -= 3;
                    Status.WriteToStatusLine("The rat bites you!");
                }
            }
        }

        //TODO -- Add Load/Save (very basic for now...)
    }
}
