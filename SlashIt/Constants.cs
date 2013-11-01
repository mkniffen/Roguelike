using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class Constants
    {
        public class TypeIds
        {
            public const int Floor = 0;
            public const int Wall = 1;
            public const int Door = 2;
            public const int OpenDoor = 3;
        }

        public class MobileId
        {
            public const int Rat = 112;
            public const int Bob = 111;

            public const int Player = 1000;
        }

        public static Dictionary<string, Item.ItemType> ItemTypeDictionary = new Dictionary<string, Item.ItemType>
        {
            {"Armor", Item.ItemType.Armor},
            {"Container", Item.ItemType.Container},
            {"Weapon", Item.ItemType.Weapon}
        };

        public static Dictionary<string, Item.ItemLocation> ItemLocationDictionary = new Dictionary<string, Item.ItemLocation>
        {
            {"Head", Item.ItemLocation.Head},
            {"Chest", Item.ItemLocation.Chest},
            {"Feet", Item.ItemLocation.Feet},
            {"HandRight", Item.ItemLocation.HandRight},
            {"HandLeft", Item.ItemLocation.HandLeft},
            {"Bag1", Item.ItemLocation.Bag1}
        };

        public static Dictionary<string, StateTransitionTable> StateTransitionDictionary = new Dictionary<string, StateTransitionTable>
        {
            {"Rat", new RatTransitionTable()},
            {"Bob", new BobTransitionTable()},
            {"None", null},
            {string.Empty, null}
        };

        public static Dictionary<string, int?> TransitionDictionary = new Dictionary<string, int?>
        {
            {"Attack", (int)Transition.Attack},
            {"Chase", (int)Transition.Chase},
            {"Rest", (int)Transition.Rest},
            {"None", null},
            {string.Empty, null}
        };

        public static Dictionary<string, IState> StateDictionary = new Dictionary<string, IState>
        {
            {"AttackStateRat", new AttackStateRat()},
            {"AttackStateBob", new AttackStateBob()},
            {"ChaseStateBob", new ChaseStateBob()},
            {"RestStateBob", new RestStateBob()},
            {"None", null},
            {string.Empty, null}
        };
    }
}
