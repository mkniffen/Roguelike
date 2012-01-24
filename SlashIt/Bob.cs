using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class Bob : Mobile, INonPlayerCharacter
    {
        public Bob()
        {
            //TODO -- !!!!!! Hard coding for now.  This will probably be part of a strategy (or maybe factory) pattern.
            //                                     Could do using a container too.

            this.Name = "Bob";
            this.Description = "So plain it just bores you to death!";
            this.DisplayCharacter = "B";
            this.UniqueId = Constants.UniqueIds.Bob;
            this.HitPoints = 20;
            this.HitMessage = this.Name + " ";
        }


        override public bool CanMoveOnPath(Tile tile)
        {
            List<int> canMoveToTiles = new List<int>
            {
                Constants.UniqueIds.Floor,
                Constants.UniqueIds.OpenDoor,
                Constants.UniqueIds.Door
            };

            //See if this map object can make the requested move
            return canMoveToTiles.Contains(tile.UniqueId) && ((tile.Mobile == null) || (tile.Mobile is Player));
        }


        public void PerformAction(Map map, Tile nonPlayerCharacterTile)
        {

            //TODO WORKING HERE Use State machine to control AI


            //Attack
            if (nonPlayerCharacterTile.Mobile.CanAttack(map, nonPlayerCharacterTile))
            {
                map.GetPlayer().HitPoints -= 3;
                Status.WriteToStatusLine("Bob bores you to death... literally!");

                return;
            }

            //Otherwise get to within attack range of player
            var playerTile = map.GetPlayerTile();

            var tileToMoveTo = map.GetShortestDistanceDirectionToPlayer(this, playerTile.Location, nonPlayerCharacterTile.Location);
            //if null, don't move
            if (tileToMoveTo != null) 
            {
                if (tileToMoveTo.UniqueId == Constants.UniqueIds.Door)
                {
                    map.ToggleDoor(tileToMoveTo, false);
                }
                else
                {
                    var tile = map.MoveMobile(nonPlayerCharacterTile, tileToMoveTo);
                }
            }
        }

        //TODO -- Add Load/Save (very basic for now...)
    }
}
