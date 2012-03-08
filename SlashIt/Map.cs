using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;

namespace SlashIt
{
    public class Map
    {

        //TODO move this to loading from config (maybe remove name) -- convert to Factory??????????
        public static Dictionary<int, TileDetail> availableTiles = new Dictionary<int, TileDetail>
        {
            {Constants.TypeIds.Door, new TileDetail {Description = "A big wooden door.  It's closed", DisplayCharacter = "+", Name = "Door"}},
            {Constants.TypeIds.Floor, new TileDetail {Description = "Empty floor", DisplayCharacter = ".", Name = "Floor"}},
            {Constants.TypeIds.OpenDoor, new TileDetail {Description = "An open door", DisplayCharacter = "`", Name = "OpenDoor"}},
            {Constants.TypeIds.Wall, new TileDetail {Description = "A brick wall", DisplayCharacter = "#", Name = "Wall"}},
        };

        public List<Tile> Tiles { get; set; }

        public bool Outdated { get; set; }

        public Map()
        {
            this.Outdated = true;
            this.Tiles = new List<Tile>();
           //this.LoadTiles();
        }

        public Tile GetTileForLocation(Location mapLocation)
        {
            var tileToMoveTo = this.Tiles
                .Where(m => m.Location.Left == mapLocation.Left && m.Location.Top == mapLocation.Top)
                .Single();
            return tileToMoveTo;
        }

        public LocalKeyInfo GetDirectionTowardsPlayer(Location playerLocation, Location mobileLocation)
        {
            if (playerLocation.Top < mobileLocation.Top)
            {
                return new LocalKeyInfo(ConsoleKey.UpArrow, false, false, false);
            }

            if (playerLocation.Top > mobileLocation.Top)
            {
                return new LocalKeyInfo(ConsoleKey.DownArrow, false, false, false);
            }

            if (playerLocation.Left < mobileLocation.Left)
            {
                return new LocalKeyInfo(ConsoleKey.LeftArrow, false, false, false);
            }

            if (playerLocation.Left > mobileLocation.Left)
            {
                return new LocalKeyInfo(ConsoleKey.RightArrow, false, false, false);
            }

            return null;
        }


        /*
        1. Find the destination tile (where the player is). 
        2. Put the starting tile (where the monster is) on the OPEN list. It's starting cost is zero.
        3. While the OPEN list is not empty, and a path isn't found: 
            1. Get the tile from the OPEN list with the lowest movement cost. Let's call it the CURRENT tile.
            2. If this is the destination tile, the path has been found. Exit the loop now.
            3. Find the tiles to which you can immediately walk to from this tile. These would the tiles around this tile, which don't contain obstacles. Call these tiles "successors".
            4. For each successor: 
                1. Set the successor's parent to the CURRENT tile. 
                2. Set the successor's movement cost to the parent's movement cost, plus 1 (for diagonal movements, add more if it takes longer to go diagonally in your game).
                3. If the successor doesn't exist on either the OPEN list or the CLOSED list, add it to the OPEN list. Otherwise, if the successor's movement cost is lower than the movement cost of the same tile on one of the lists, delete the occurrences of the successor from the lists add the successor to the OPEN list Otherwise, if the successor's movement cost is higher than that of the same tile on one of the lists, get rid of the successor
            5. Delete the CURRENT tile from the OPEN list, and put it on the CLOSED list. 
        4. If the while loop has been ended because the OPEN list is empty, there is no path.
        5. If this is not the case, the last tile pulled from the OPEN list, and its parents, describe the shortest path (in reverse order - i.e. from the player to the monster - you should read the list of tiles back to front).
        */
        public Tile GetShortestDistanceDirectionToPlayer(Mobile mobile, Location playerLocation, Location mobileLocation)
        {
            //Set up lists for tracking tiles while pathfinding
            var openLocations = new List<PathLocation>();
            var closedLocations = new List<PathLocation>();
            List<PathLocation> successorLocations = null;
            PathLocation currentLocation = null;
            //Keep a PathLocation for the mobile.  It is needed at the end when walking the child parent relationship
            //It is used to filter out the location of the mobile, so we don't try to move to where the
            //mobile already is.
            PathLocation mobilePathLocation = new PathLocation(mobileLocation, 0, null);

            //Add the mobiles location as a starting point
            openLocations.Add(mobilePathLocation);

            //Continue to check for paths until we've looked at all possible tiles (tiles that the mobile can move into
            //from it's original location)
            while (openLocations.Count > 0)
            {
                //Get the tile with the least movement cost.  There may be several that have equal costs.
                //Just take the first one
                int minMovementCost = openLocations.Min(o => o.MovementCost);
                currentLocation = openLocations.Where(o => o.MovementCost == minMovementCost).First();

                //If we have found the players location with a tile that has the least associated movement
                //cost, then we are done.
                if (currentLocation.ThisLocation == playerLocation)
                {
                    break;
                }

                //Init the adject tiles scanning loops
                var topStart = currentLocation.ThisLocation.Top - 1 < 1 ? 1 : currentLocation.ThisLocation.Top - 1;
                var leftStart = currentLocation.ThisLocation.Left - 1 < 1 ? 1 : currentLocation.ThisLocation.Left - 1;
                //Make sure the successor list is empty.  It will be filled wiht tiles that can be moved to from,
                //the current location.
                successorLocations = new List<PathLocation>();

        //TODO a similar loop to this is used in Mobile.CanAttack.  See if can refactor

                //Go through each of the tiles adjacent to the current location and add it to the successor list
                //if the mobile can move to it (ie. it's not a wall or another mobile, etc...)
                for (int top = topStart; top <= currentLocation.ThisLocation.Top + 1; top++)
                {
                    for (int left = leftStart; left <= currentLocation.ThisLocation.Left + 1; left++)
                    {
                        var potentialTile = this.GetTileForLocation(new Location(left, top));
                        if (mobile.CanMoveOnPath(potentialTile))
                        {
                            //Add the adjacent tile to the successor list setting its movement cost
                            //and parent tile (the current location adjacenct to this one).
                            successorLocations.Add(new PathLocation(potentialTile.Location, currentLocation.MovementCost + 1, currentLocation));
                        }
                    }
                }

                //Go through the successors and determine what to do with them
                foreach (var successorLocation in successorLocations)
                {
                    //See if the successor location is already on the open or closed locations list
                    var onOpenLocations = openLocations.Where(o => o.ThisLocation == successorLocation.ThisLocation);
                    var onClosedLocations = closedLocations.Where(o => o.ThisLocation == successorLocation.ThisLocation);

                    //If the successor is NOT on either the closed or open list, then add it to open.  This means
                    //we need probably need to do more pathfinding from this successor.
                    if (onOpenLocations.Count() < 1 && onClosedLocations.Count() < 1)
                    {
                        openLocations.Add(successorLocation);
                    }
                        //If this successor has a lower movement cost than the same location with a higher movement cost,
                        //the remove it from both lists and add it to the open list.  This means we have found a shorter
                        //path (the lower movement cost) and so we will want to continue pathfinding from this successor.
                    else if ((onOpenLocations.Count() > 0 && successorLocation.MovementCost < onOpenLocations.Min(o => o.MovementCost)) ||
                        (onClosedLocations.Count() > 0 && successorLocation.MovementCost < onClosedLocations.Min(o => o.MovementCost)))
                    {
                        openLocations.RemoveAll(o => onOpenLocations.Contains(o));
                        closedLocations.RemoveAll(o => onClosedLocations.Contains(o));

                        openLocations.Add(successorLocation);
                    }
                        //If this successor has a greater movement cost, then throw it out.  This means this successor is
                        //through a longer path that we don't want to persue.
                    else if ((onOpenLocations.Count() > 0 && successorLocation.MovementCost > onOpenLocations.Min(o => o.MovementCost)) ||
                        (onClosedLocations.Count() > 0 && successorLocation.MovementCost > onClosedLocations.Min(o => o.MovementCost)))
                    {
                        ; //Do nothing (effectively get rid of the successor
                    }

                    //Note that we did nothing with successors that have == movement costs.  We want to make sure
                    //we check all the paths from it, so it will be skipped or removed at a later point.
                }

                //We are done with the current location at this point.  Move it to the closed list.
                openLocations.Remove(currentLocation);
                closedLocations.Add(currentLocation);
                currentLocation = null;
            }

            //If current location is null, there is no path to the player.
            if (currentLocation == null)
            {
                return null;
            }

            //If we get to this point with a current location, that location is the player.
            PathLocation locationToMoveTo = currentLocation.ParentPathLocation;


            //Now walk backwards from the player location through the parents to the mobile, but stop
            //at the tile before the mobile.
            while ( locationToMoveTo.ParentPathLocation != null && 
                locationToMoveTo.ParentPathLocation.ThisLocation != mobilePathLocation.ThisLocation)
            {
                locationToMoveTo = locationToMoveTo.ParentPathLocation;
            }

            //Return the tile adjancent to the mobile that we would like to move to.  this tile
            //is part of the shortest path at this point (of course the path can change later if the player moves).
            return this.GetTileForLocation(locationToMoveTo.ThisLocation);
        }


        public void ToggleDoor(Tile tileToUse, bool isOpen)
        {
            this.Tiles.Remove(tileToUse);

            if (isOpen)
            {
                this.Tiles.Add(new Tile { Location = tileToUse.Location, TypeId = Constants.TypeIds.Door });
            }
            else
            {
                this.Tiles.Add(new Tile { Location = tileToUse.Location, TypeId = Constants.TypeIds.OpenDoor });
            }
        }


        public LocalKeyInfo GetDirectionRandom()
        {
            var direction = Program.RandomNumber(5);

            switch (direction)
            {
                case 0:
                    return null;
                case 1:
                    return new LocalKeyInfo(ConsoleKey.UpArrow, false, false, false);
                case 2:
                    return new LocalKeyInfo(ConsoleKey.DownArrow, false, false, false);
                case 3:
                    return new LocalKeyInfo(ConsoleKey.LeftArrow, false, false, false);
                case 4:
                    return new LocalKeyInfo(ConsoleKey.RightArrow, false, false, false);
                default:
                    return null;
            }
        }

        public Tile GetPlayerTile()
        {
            var mapTile = this.Tiles
                .Where(m => m.Mobile != null && m.Mobile.TypeId == Constants.TypeIds.Player)
                .Single();

            return mapTile;
        }

        public List<Tile> GetNonPlayerTiles()
        {
            var mapTiles = this.Tiles
                .Where(m => m.Mobile != null && m.Mobile is INonPlayerCharacter)
                .ToList();

            return mapTiles;
        }

        public Tile GetTileInDirection(LocalKeyInfo keyInfo, Location mapLocation)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    mapLocation.Top--;
                    break;

                case ConsoleKey.DownArrow:
                    mapLocation.Top++;
                    break;

                case ConsoleKey.LeftArrow:
                    mapLocation.Left--;
                    break;

                case ConsoleKey.RightArrow:
                    mapLocation.Left++;
                    break;

                default:
                    break;
            }

            return this.GetTileForLocation(mapLocation);
        }


        public Tile MoveMobile(LocalKeyInfo keyInfo, Tile mobileMapTile)
        {
            var mapLocation = new Location(mobileMapTile.Location.Left, mobileMapTile.Location.Top);

            var tileToMoveTo = this.GetTileInDirection(keyInfo, mapLocation);

            return this.MoveMobile(mobileMapTile, tileToMoveTo);
        }

        public Tile MoveMobile(Tile mobileMapTile, Tile tileToMoveTo)
        {
            if (mobileMapTile.Mobile.CanMoveTo(tileToMoveTo))
            {
                tileToMoveTo.Mobile = mobileMapTile.Mobile;
                mobileMapTile.Mobile = null;
                this.Outdated = true;

                return tileToMoveTo;
            }

            return null;
        }

        private void LoadTiles()
        {
            Tiles.AddRange(
                new List<Tile> 
                { 
                    new Tile { Location = new Location(1,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(2,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(3,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(4,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(5,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(6,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(7,1) , TypeId = Constants.TypeIds.Wall},
                    new Tile { Location = new Location(8,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(9,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(10,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(11,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(12,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(13,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(14,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(15,1), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(1,2), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(2,2), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(3,2), Mobile = new Mobile {TypeId = Constants.TypeIds.Player, DisplayCharacter = "@", Description =  "This guy is a newb!!", HitMessage = "The player ", Name = "Player", HitPoints = 30, TransitionTable = null, CurrentTransition = null }, TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(4,2), TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(5,2) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(6,2), TypeId = Constants.TypeIds.Door },
                    new Tile { Location = new Location(7,2) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(8,2) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(9,2), Mobile = new Mobile {TypeId = Constants.TypeIds.Bob, DisplayCharacter = "B", Description =  "So plain it just bores you to death!", HitMessage = "Bob ", Name = "Bob", HitPoints = 20, TransitionTable = new BobTransitionTable(), CurrentTransition = (int)Transition.Rest} , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(10,2), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(11,2), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(12,2), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(13,2), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(14,2), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(15,2), TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(1,3) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(2,3) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(3,3) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(4,3) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(5,3) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(6,3) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(7,3) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(8,3) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(9,3) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(10,3) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(11,3) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(12,3) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(13,3) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(14,3) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(15,3) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(1,4) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(2,4) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(3,4) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(4,4) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(5,4) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(6,4) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(7,4) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(8,4) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(9,4) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(10,4) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(11,4) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(12,4) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(13,4) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(14,4) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(15,4) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(1,5) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(2,5) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(3,5) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(4,5) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(5,5) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(6,5) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(7,5) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(8,5) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(9,5) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(10,5) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(11,5) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(12,5) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(13,5) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(14,5) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(15,5) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(1,6) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(2,6) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(3,6) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(4,6) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(5,6) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(6,6) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(7,6) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(8,6) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(9,6) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(10,6) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(11,6) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(12,6) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(13,6) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(14,6) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(15,6) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(1,7) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(2,7) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(3,7) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(4,7) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(5,7) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(6,7) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(7,7) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(8,7) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(9,7) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(10,7) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(11,7) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(12,7) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(13,7) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(14,7) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(15,7) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(1,8) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(2,8) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(3,8) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(4,8) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(5,8) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(6,8) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(7,8) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(8,8) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(9,8) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(10,8) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(11,8) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(12,8) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(13,8) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(14,8) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(15,8) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(1,9) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(2,9) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(3,9) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(4,9) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(5,9) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(6,9) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(7,9) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(8,9) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(9,9), Mobile = new Mobile {TypeId = Constants.TypeIds.Rat, DisplayCharacter = "r", Description = "A simple rat that wants to EAT you!", HitMessage = "the Rat ", Name = "Rat" , HitPoints = 10, TransitionTable = null, CurrentTransition = (int)Transition.Rest} , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(10,9) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(11,9) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(12,9) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(13,9) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(14,9) , TypeId = Constants.TypeIds.Floor },
                    new Tile { Location = new Location(15,9) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(1,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(2,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(3,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(4,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(5,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(6,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(7,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(8,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(9,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(10,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(11,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(12,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(13,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(14,10) , TypeId = Constants.TypeIds.Wall },
                    new Tile { Location = new Location(15,10) , TypeId = Constants.TypeIds.Wall },
                    
                   
                });
        }

        public List<Mobile> GetMobiles()
        {
            return this.Tiles.Where(t => t.Mobile != null).Select(t => t.Mobile).ToList();
        }

        public Mobile GetPlayer()
        {
            return this.GetPlayerTile().Mobile;
        }

        public XElement Save()
        {
            return new XElement("Tiles",
                Tiles.Select(t => 
                    new XElement("Tile", 
                        new XElement("Location",
                            new XElement("Top", t.Location.Top),
                            new XElement("Left", t.Location.Left)),
                        new XElement(t.Mobile == null ? new XElement("Mobile", "") : t.Mobile.Save()),  //TODO need to code save on player and mobile
                        new XElement(t.Item == null ? new XElement("Item","") : t.Item.Save()),
                        new XElement("TypeId", t.TypeId)
                            ))
                );
        }

        public void Load(IEnumerable<XElement> tilesToLoad)
        {
            Mobile mobile = null;
            Item item = null;

            foreach (XElement tile in tilesToLoad.Descendants("Tile"))
            {
                var top = Int32.Parse(tile.Element("Location").Element("Top").Value);
                var left = Int32.Parse(tile.Element("Location").Element("Left").Value);
                var tileTypeId = Int32.Parse(tile.Element("TypeId").Value);

                mobile = null;
                if (!string.IsNullOrEmpty(tile.Element("Mobile").Value))
                {
                    mobile = Mobile.GetMobileById((Int32.Parse(tile.Element("Mobile").Element("TypeId").Value)));
                    mobile.Load(tile.Element("Mobile"));
                }

                item = null;
                if (!string.IsNullOrEmpty(tile.Element("Item").Value))
                {
                    item = Item.GetItemById((Int32.Parse(tile.Element("Item").Element("ItemId").Value)));
                    item.Load(tile.Element("Item"));
                }

                Tiles.Add(new Tile {Location = new Location(left, top), TypeId = tileTypeId, Mobile = mobile, Item = item });
            }
        }
    }
}
