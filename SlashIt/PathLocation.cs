using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class PathLocation
    {
        public PathLocation(Location currentLocation, int movementCost, PathLocation parentPathLocation)
        {
            this.ThisLocation = currentLocation;
            this.ParentPathLocation = parentPathLocation;
            this.MovementCost = movementCost;
        }

        public Location ThisLocation { get; set; }
        public PathLocation ParentPathLocation { get; set; }
        public int MovementCost { get; set; }
    }
}
