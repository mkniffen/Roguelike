using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    //TODO !!!!!!!!!!   Need to find a way to load this from config  !!!!!!!!!!!!!!!!!!1

    //    override public bool CanMoveOnPath(Tile tile)
    //    {
    //        List<int> canMoveToTiles = new List<int>
    //        {
    //            Constants.TypeIds.Floor,
    //            Constants.TypeIds.OpenDoor,
    //            Constants.TypeIds.Door
    //        };

    //        //See if this map object can make the requested move
    //        return canMoveToTiles.Contains(tile.TypeId) && ((tile.Mobile == null) || (tile.Mobile is Player));
    //    }
    //}
    



    public class BobTransitionTable : StateTransitionTable
    {
        public BobTransitionTable()
        {
            base.table.Add(Transition.Attack, new AttackStateBob());
            base.table.Add(Transition.Rest, new RestStateBob());
            base.table.Add(Transition.Chase, new ChaseStateBob());
        }
    }
  

    public class AttackStateBob : IState
    {

        public void Enter(Mobile mobile)
        {
            return;
        }

        public void Execute(Mobile mobile, Map map, Tile nonPlayerCharacterTile)
        {
            if (mobile.CanAttack(map, nonPlayerCharacterTile))
            {
                map.GetPlayer().HitPoints -= 3;
                Status.WriteToStatusLine("Bob bores you to death... literally!");

                return;
            }
        }

        public void Exit(Mobile mobile)
        {
            return;
        }
    }

    public class ChaseStateBob : IState
    {

        public void Enter(Mobile mobile)
        {
            return;
        }

        public void Execute(Mobile mobile, Map map, Tile nonPlayerCharacterTile)
        {
            if (mobile.CanAttack(map, nonPlayerCharacterTile))
            {
                mobile.Event = Transition.Attack;
                return;
            }

            var playerTile = map.GetPlayerTile();

            var tileToMoveTo = map.GetShortestDistanceDirectionToPlayer(mobile, playerTile.Location, nonPlayerCharacterTile.Location);
            //if null, don't move
            if (tileToMoveTo != null)
            {
                if (tileToMoveTo.TypeId == Constants.TypeIds.Door)
                {
                    map.ToggleDoor(tileToMoveTo, false);
                }
                else
                {
                    var tile = map.MoveMobile(nonPlayerCharacterTile, tileToMoveTo);
                }
            }
        }

        public void Exit(Mobile mobile)
        {
            return;
        }
    }

    public class RestStateBob : IState
    {
        public virtual void Enter(Mobile mobile)
        {
            return;
        }

        public virtual void Execute(Mobile mobile, Map map, Tile nonPlayerCharacterTile)
        {
            if (mobile.CanAttack(map, nonPlayerCharacterTile))
            {
                mobile.Event = Transition.Attack;
            }
            else
            {
                mobile.Event = Transition.Chase;
            }
        }

        public virtual void Exit(Mobile mobile)
        {
            return;
        }
    }
}
