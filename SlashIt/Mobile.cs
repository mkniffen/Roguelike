using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SlashIt
{
    public abstract class Mobile
    {
        protected StateTransitionTable transitionTable = null;
        protected IState currentState = null;

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
        public int TypeId { get; set; }
        public string HitMessage { get; set; }

        //This will fill up with "time" units
        public virtual int TimeBucket { get; set; }
        //This determines how much needs to be in the TimeBucket before the Mobile can move
        public virtual int CanMoveLevel { get; set; }
        //This determines how much "time" is added to the bucket each game turn
        public virtual int TimeBucketFillAmount { get; set; }
        //How much is added to the TimeBucket each game turn
        public virtual int Speed { get; set; }

        public int HitPoints { get; set; }


        public object Event
        {
            set
            {
                if (value == null)
                {
                    currentState.Exit(this);
                    currentState = null;
                    return;
                }

                IState state = transitionTable.GetState(value);

                if (state != null)
                {
                    if (currentState != null)
                        currentState.Exit(this);

                    currentState = state;
                    currentState.Enter(this);
                }
            }
        }

        public virtual bool CanAct()
        {
            return TimeBucket > CanMoveLevel;
        }

        public virtual bool IsDead()
        {
            return HitPoints < 1;
        }

        public virtual bool CanMoveTo(Tile tile)
        {
            List<int> canMoveToTiles = new List<int>
            {
                Constants.TypeIds.Floor,
                Constants.TypeIds.OpenDoor
            };

            //See if this map object can make the requested move
            return canMoveToTiles.Contains(tile.TypeId) && tile.Mobile == null;
        }

        public virtual bool CanMoveOnPath(Tile tile)
        {
            List<int> canMoveToTiles = new List<int>
            {
                Constants.TypeIds.Floor,
                Constants.TypeIds.OpenDoor
            };

            //See if this map object can make the requested move
            return canMoveToTiles.Contains(tile.TypeId) && ((tile.Mobile == null) || (tile.Mobile is Player));
        }

        public bool CanAttack(Map map, Tile nonPlayerCharacterTile)
        {
            var topStart = nonPlayerCharacterTile.Location.Top - 1 < 1 ? 1 : nonPlayerCharacterTile.Location.Top - 1;
            var leftStart = nonPlayerCharacterTile.Location.Left - 1 < 1 ? 1 : nonPlayerCharacterTile.Location.Left - 1;

            var playerTile = map.GetPlayerTile();

            for (int top = topStart; top <= nonPlayerCharacterTile.Location.Top + 1; top++)
            {
                for (int left = leftStart; left <= nonPlayerCharacterTile.Location.Left + 1; left++)
                {
                    if (playerTile.Location.Top == top && playerTile.Location.Left == left)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void AdvanceTime()
        {
            this.TimeBucket += this.Speed;
        }

        public void UpdateState(Map map, Tile nonPlayerCharacterTile)
        {
            if (currentState != null)
                currentState.Execute(this, map, nonPlayerCharacterTile);
            else
                System.Diagnostics.Trace.WriteLine("zero state");
        }

        public XElement Save()
        {
            return new XElement("Mobile", "test stuff");
        }
    }
}
