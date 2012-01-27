﻿using System;
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

            this.transitionTable = new BobTransitionTable();
            this.Event = BobEvent.Rest;
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
    }
    
    public enum BobEvent
    {
        Attack,
        Rest,
        Chase
    }



    class BobTransitionTable : StateTransitionTable
    {
        public BobTransitionTable()
        {
            base.table.Add(BobEvent.Attack, new AttackState());
            base.table.Add(BobEvent.Rest, new RestState());
            base.table.Add(BobEvent.Chase, new ChaseState());
        }
    }
  

    public class AttackState : IState
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

    public class RestState : IState
    {
        public void Enter(Mobile mobile)
        {
            return;
        }

        public void Execute(Mobile mobile, Map map, Tile nonPlayerCharacterTile)
        {
            if (mobile.CanAttack(map, nonPlayerCharacterTile))
            {
                mobile.Event = BobEvent.Attack;
            }
            else
            {
                mobile.Event = BobEvent.Chase;
            }
        }

        public void Exit(Mobile mobile)
        {
            return;
        }
    }

    public class ChaseState : IState
    {

        public void Enter(Mobile mobile)
        {
            return;
        }

        public void Execute(Mobile mobile, Map map, Tile nonPlayerCharacterTile)
        {
            if (mobile.CanAttack(map, nonPlayerCharacterTile))
            {
                mobile.Event = BobEvent.Attack;
                return;
            }

            var playerTile = map.GetPlayerTile();

            var tileToMoveTo = map.GetShortestDistanceDirectionToPlayer(mobile, playerTile.Location, nonPlayerCharacterTile.Location);
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

        public void Exit(Mobile mobile)
        {
            return;
        }
    }
}
