using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class NonPlayerCharacter //: IMapObject
    {
        public NonPlayerCharacter()
        {
            //TODO -- !!!!!! Hard coding for now.  This will probably be part of a strategy (or maybe factory) pattern.
            //                                     Could do using a container too.

            this.Name = "Bob";
            this.Description = "So plain it just bores you to death!";
            this.DisplayCharacter = "B";
            this.UniqueId = Constants.UniqueIds.NonPlayerCharacter;

            this.Location = new Location(22, 9);
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayCharacter { get; set; }
        public Location Location { get; set; } // TODO right now this is tracked here and in the map matrix.  Should try to reduce to one place
        public int UniqueId { get; set; }


        //TODO -- Add Load/Save (very basic for now...)
    }
}
