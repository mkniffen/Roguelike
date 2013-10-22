using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SlashIt
{
    public class EquipedItemSet
    {

        public EquipedItemSet()
        {
            this.EquipedItems = new List<EquipedItem>
            {
                {new EquipedItem { Location = ItemLocation.Head, AcceptableTypes = new List<ItemType> { ItemType.Armor }}},
                {new EquipedItem { Location = ItemLocation.HandLeft, AcceptableTypes = new List<ItemType> {  ItemType.Weapon}}},
                {new EquipedItem { Location = ItemLocation.HandRight, AcceptableTypes =  new List<ItemType> { ItemType.Weapon}}},
                {new EquipedItem { Location = ItemLocation.Chest, AcceptableTypes =  new List<ItemType> { ItemType.Armor}}},
                {new EquipedItem { Location = ItemLocation.Feet, AcceptableTypes =  new List<ItemType> { ItemType.Armor}}},
                {new EquipedItem { Location = ItemLocation.Bag1, AcceptableTypes =  new List<ItemType> { ItemType.Container}}},
            };
        }

        public List<EquipedItem> EquipedItems { get; set; }

        //public bool ActiveItemSet { get; set; }

        //public EquipedItem Head { get; set; }
        //public EquipedItem WeaponLeft { get; set; }
        //public EquipedItem WeaponRight { get; set; }
        //public EquipedItem Chest { get; set; }
        //public EquipedItem Feet { get; set; }
    }

    public class EquipedItem
    {
        public List<ItemType> AcceptableTypes { get; set; }
        public ItemLocation Location { get; set; }
        public Item Item { get; set; }
        public string ListTag { get; set; }
    }

    public class Mobile : INonPlayerCharacter
    {


        //TODO move this to loading from config (maybe remove name) -- convert to Factory??????????


        public static List<Mobile> availableMobiles = new List<Mobile>
        {
            { new Mobile {TypeId = Constants.TypeIds.Player, DisplayCharacter = "@", Description =  "This guy is a newb!!", HitMessage = "The player ", Name = "Player", HitPoints = 30, TransitionTable = null, CurrentTransition = null, currentState = null }},
            { new Mobile {TypeId = Constants.TypeIds.Rat, DisplayCharacter = "r", Description = "A simple rat that wants to EAT you!", HitMessage = "The rat bites you!!!", Name = "Rat" , HitPoints = 10, TransitionTable = new RatTransitionTable(), CurrentTransition = (int)Transition.Attack, currentState = new AttackStateRat() }},
            { new Mobile {TypeId = Constants.TypeIds.Bob, DisplayCharacter = "B", Description =  "So plain it just bores you to death!", HitMessage = "Bob ", Name = "Bob", HitPoints = 15, TransitionTable = new BobTransitionTable(), CurrentTransition = (int)Transition.Rest, currentState = new RestStateBob() }}
        };


        protected IState currentState = null;

        public Mobile()
        {
            this.CanMoveLevel = 10;
            this.TimeBucket = 5;
            this.Speed = 5;
            this.Items = new List<Item>();
            this.EquipedItemSet = new EquipedItemSet();
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

        public List<Item> Items { get; set; }
        public EquipedItemSet EquipedItemSet { get; set; }   //TODO finish setting up equiped items

        public StateTransitionTable TransitionTable { get; set; }
        public int? CurrentTransition { get; set; }

        //public enum Transitions;

        public bool HasEquipableItems
        {
            get
            {
                return Items.Any(i => i.IsEquipable());
            }
        }


        public object Event
        {
            set
            {
                if (value == null)
                {
                    currentState.Exit(this);
                    currentState = null;
                    CurrentTransition = null;
                    return;
                }

                IState state = TransitionTable.GetState(value);

                if (state != null)
                {
                    if (currentState != null)
                        currentState.Exit(this);

                    currentState = state;
                    CurrentTransition = (int)value;
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
            //TODO This list will need to vary depending on the mobile (Can Move To also)

            List<int> canMoveToTiles = new List<int>
            {
                Constants.TypeIds.Floor,
                Constants.TypeIds.OpenDoor
            };

            //See if this map object can make the requested move
            return canMoveToTiles.Contains(tile.TypeId) && ((tile.Mobile == null) || (tile.Mobile.TypeId == Constants.TypeIds.Player));
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

        public static Mobile GetMobileById(int id)
        {
            return availableMobiles.Where(m => m.TypeId == id).Single<Mobile>();
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
            return new XElement("Mobile",
                new XElement("TypeId", this.TypeId),
                new XElement("CanMoveLevel",this.CanMoveLevel),
                new XElement("TimeBucket",this.TimeBucket),
                new XElement("Speed",this.Speed),
                new XElement("HitPoints",this.HitPoints),
                new XElement("CurrentTransition", this.CurrentTransition),
                Items.Count <= 0 ? new XElement("Inventory", "") : new XElement("Inventory", Items.Select(i => i.Save()))
                );
        }

        public void Load(XElement mobile)
        {
            this.TypeId = Int32.Parse(mobile.Element("TypeId").Value);
            this.CanMoveLevel = Int32.Parse(mobile.Element("CanMoveLevel").Value);
            this.TimeBucket = Int32.Parse(mobile.Element("TimeBucket").Value);
            this.Speed = Int32.Parse(mobile.Element("Speed").Value);
            this.HitPoints = Int32.Parse(mobile.Element("HitPoints").Value);
            int i;
            this.CurrentTransition = Int32.TryParse(mobile.Element("CurrentTransition").Value, out i) ? (int?)i : null;

            var availableItems = AvailableItems.Instance;

            foreach (XElement item in mobile.Descendants("Inventory"))
            {
                if (item.HasElements)
                {
                    var itemToLoad = availableItems.GetItemById((Int32.Parse(item.Element("Item").Element("TypeId").Value)));
                    itemToLoad.Load(item.Element("Item"));
                    this.Items.Add(itemToLoad);
                }
            }
        }

        public void WearItem(Item itemToWear)
        {
            List<EquipedItem> itemSlots = EquipedItemSet.EquipedItems.Where(ei => ei.AcceptableTypes.Any(at => itemToWear.ItemTypes.Contains(at)) && itemToWear.ItemLocations.Contains(ei.Location)).ToList();

            if (itemSlots.Count < 1)
            {
                throw new Exception("These must be at least one slot for this item");

                //TODO : maybe change this to, "You can't wear that item"

                return;
            }

            if (itemSlots.Count > 1)
            {
                var s = new StringBuilder();

                s.Append("Which location do you want to use? ").Append(Environment.NewLine);

                int i = 1;
                foreach (var itemSlot in itemSlots)
                {
                    itemSlot.ListTag = InventoryCommand.Letters[i];
                    s.Append("   ").Append(InventoryCommand.Letters[i]).Append(") ");
                    s.Append(itemSlot.Location).Append(Console.Out.NewLine);
                    i++;
                }

                Status.Info = s.ToString();
                Console.Clear();
                Status.WriteToStatus();

                var itemSlotKey = Console.ReadKey(true);

                var selectedItemSlot = itemSlots.Where(isl => isl.ListTag.ToUpper() == itemSlotKey.KeyChar.ToString().ToUpper()).Single();

                MoveItemFromInventoryToEquiped(itemToWear, selectedItemSlot);

                return;
            }

            MoveItemFromInventoryToEquiped(itemToWear, itemSlots[0]);
        }

        private void MoveItemFromInventoryToEquiped(Item itemToWear, EquipedItem itemSlot)
        {
            if (itemSlot.Item != null)
            {
                this.Items.Add(itemSlot.Item);
            }
            itemSlot.Item = itemToWear;
            this.Items.Remove(itemToWear);
        }
    }
}
