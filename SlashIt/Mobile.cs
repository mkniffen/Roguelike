using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public abstract class Mobile
    {
        public Mobile()
        {
            this.TimeBucket = 0;
            this.CanMoveLevel = 10;
            this.TimeBucket = 5;
            this.Speed = 5;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayCharacter { get; set; }
        public int UniqueId { get; set; }

        //This will fill up with "time" units
        public virtual int TimeBucket { get; set; }
        //This determines how much needs to be in the TimeBucket before the Mobile can move
        public virtual int CanMoveLevel { get; set; }
        //This determines how much "time" is added to the bucket each game turn
        public virtual int TimeBucketFillAmount { get; set; }
        //How much is added to the TimeBucket each game turn
        public virtual int Speed { get; set; }

        public virtual bool CanAct()
        {
            return TimeBucket > CanMoveLevel;
        }

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



        public void AdvanceTime()
        {
            this.TimeBucket += this.Speed;
        }
    }
}
