using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class Bob : Mobile, INonPlayerCharacter
    {
        private Context<Bob> context;

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

            context = new Context<Bob>();
            context.Configure(this, RestState.Instance);
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


        public void ChangeState(State<Bob> state)
        {
            this.context.ChangeState(state);
        }


        // Simple States rest->chase->attack
        //               rest->attack
        //               attack->chase
        //               chase->attack
        public void PerformAction(Map map, Tile nonPlayerCharacterTile)
        {


            //TODO WORKING HERE -- Refined the state machine.  Maybe  use a transition Dictionary instead of embeding in state class

            context.Request(map, nonPlayerCharacterTile);
        }

    }
    

    public class AttackState : State<Bob>
    {
        static readonly AttackState instance = new AttackState();
        public static AttackState Instance 
        {
            get 
            {
                return instance;
            }
        }

        static AttackState() { }
        private AttackState() { }

        public override void Handle(Bob entity, Map map, Tile nonPlayerCharacterTile)
        {
            //Attack
            if (entity.CanAttack(map, nonPlayerCharacterTile))
            {
                map.GetPlayer().HitPoints -= 3;
                Status.WriteToStatusLine("Bob bores you to death... literally!");

                return;
            }
        }
    }

    public class RestState : State<Bob>
    {
        static readonly RestState instance = new RestState();
        public static RestState Instance 
        {
            get 
            {
                return instance;
            }
        }

        static RestState() { }
        private RestState() { }

        public override void Handle(Bob entity, Map map, Tile nonPlayerCharacterTile)
        {
            if (entity.CanAttack(map, nonPlayerCharacterTile))
            {
                entity.ChangeState(AttackState.Instance);
            }
            else 
            {
                entity.ChangeState(ChaseState.Instance);
            }
        }
    }

    public class ChaseState : State<Bob>
    {
        static readonly ChaseState instance = new ChaseState();
        public static ChaseState Instance 
        {
            get 
            {
                return instance;
            }
        }

        static ChaseState() { }
        private ChaseState() { }

        public override void Handle(Bob entity, Map map, Tile nonPlayerCharacterTile)
        {

            if (entity.CanAttack(map, nonPlayerCharacterTile))
            {
                entity.ChangeState(AttackState.Instance);
                return;
            }

            var playerTile = map.GetPlayerTile();

            var tileToMoveTo = map.GetShortestDistanceDirectionToPlayer(entity, playerTile.Location, nonPlayerCharacterTile.Location);
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
    }
}
