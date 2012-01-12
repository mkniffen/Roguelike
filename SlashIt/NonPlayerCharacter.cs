using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class NonPlayerCharacter : Mobile
    {
        public NonPlayerCharacter()
        {
            //TODO -- !!!!!! Hard coding for now.  This will probably be part of a strategy (or maybe factory) pattern.
            //                                     Could do using a container too.

            this.Name = "Bob";
            this.Description = "So plain it just bores you to death!";
            this.DisplayCharacter = "B";
            this.UniqueId = Constants.UniqueIds.NonPlayerCharacter;
        }

        public void Move()
        {

            //TODO WORKING HERE -- Adding AI for NPC.  Figure out how to move


            var direction = Program.RandomNumber(4);
        }

        //TODO -- Add Load/Save (very basic for now...)
    }
}
