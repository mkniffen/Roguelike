using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public abstract class Mobile
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayCharacter { get; set; }
        public int UniqueId { get; set; }

        public virtual bool CanMoveTo(Tile tile)
        {
            List<int> canMoveToTiles = new List<int>
            {
                Constants.UniqueIds.Floor,
                Constants.UniqueIds.OpenDoor
            };

            //See if this map object can make the requested move
            return (canMoveToTiles.Contains(tile.UniqueId) && tile.Mobile == null);
        }
    }
}
