using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class RatTransitionTable : StateTransitionTable
    {
        public RatTransitionTable()
        {
            base.table.Add(Transition.Attack, new AttackStateRat());
        }
    }

    public class AttackStateRat : IState
    {
        public void Enter(Mobile mobile)
        {
            return;
        }

        public void Execute(Mobile mobile, Map map, Tile nonPlayerCharacterTile)
        {
            if (Program.RandomNumber(4) < 3)
            {
                //Move
                var direction = map.GetDirectionRandom();

                if (direction != null)
                {
                    map.MoveMobile(direction, nonPlayerCharacterTile);
                }
            }
            else
            {
                //Attack
                if (nonPlayerCharacterTile.Mobile.CanAttack(map, nonPlayerCharacterTile))
                {
                    map.GetPlayer().HitPoints -= 3;
                    Status.WriteToStatusLine("The rat bites you!");
                }
            }
        }

        public void Exit(Mobile mobile)
        {
            return;
        }
    }
}
