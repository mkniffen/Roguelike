using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SlashIt
{
    public class Map
    {
        [XmlIgnore]
        public List<Tile> Tiles { get; set; }

        public bool Outdated { get; set; }

        public Map()
        {
            this.Outdated = true;
            this.Tiles = new List<Tile>();
            this.LoadTiles();
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

            //TODO  COmment this !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


            var openLocations = new List<PathLocation>();
            var closedLocations = new List<PathLocation>();
            List<PathLocation> successorLocations = null;
            PathLocation currentLocation = null;
            //Keep a PathLocation for the mobile.  It is needed at the end when walking the child parent relationship
            //It is used to filter out the location of the mobile, so we don't try to move to where the
            //mobile already is.
            PathLocation mobilePathLocation = new PathLocation(mobileLocation, 0, null);

            openLocations.Add(mobilePathLocation);

            while (openLocations.Count > 0)
            {
                            //TODO MAKE sure MIN is working as expected
                int minMovementCost = openLocations.Min(o => o.MovementCost);
                currentLocation = openLocations.Where(o => o.MovementCost == minMovementCost).First();

                if (currentLocation.ThisLocation == playerLocation)
                {
                    break;
                }

                var topStart = currentLocation.ThisLocation.Top - 1 < 1 ? 1 : currentLocation.ThisLocation.Top - 1;
                var leftStart = currentLocation.ThisLocation.Left - 1 < 1 ? 1 : currentLocation.ThisLocation.Left - 1;
                successorLocations = new List<PathLocation>();

                for (int top = topStart; top <= currentLocation.ThisLocation.Top + 1; top++)
                {
                    for (int left = leftStart; left <= currentLocation.ThisLocation.Left + 1; left++)
                    {
                        var potentialTile = this.GetTileForLocation(new Location(left, top));
                        if (mobile.CanMoveOnPath(potentialTile))
                        {
                            successorLocations.Add(new PathLocation(potentialTile.Location, currentLocation.MovementCost + 1, currentLocation));
                        }
                    }
                }

                foreach (var successorLocation in successorLocations)
                {
                    var onOpenLocations = openLocations.Where(o => o.ThisLocation == successorLocation.ThisLocation);
                    var onClosedLocations = closedLocations.Where(o => o.ThisLocation == successorLocation.ThisLocation);

                    if (onOpenLocations.Count() < 1 && onClosedLocations.Count() < 1)
                    {
                        openLocations.Add(successorLocation);
                    }
                    else if ((onOpenLocations.Count() > 0 && successorLocation.MovementCost < onOpenLocations.Min(o => o.MovementCost)) ||
                        (onClosedLocations.Count() > 0 && successorLocation.MovementCost < onClosedLocations.Min(o => o.MovementCost)))
                    {
                        openLocations.RemoveAll(o => onOpenLocations.Contains(o));
                        closedLocations.RemoveAll(o => onClosedLocations.Contains(o));

                        openLocations.Add(successorLocation);
                    }                                     //TODO MAKE sure MIN is working as expected
                    else if ((onOpenLocations.Count() > 0 && successorLocation.MovementCost > onOpenLocations.Min(o => o.MovementCost)) ||
                        (onClosedLocations.Count() > 0 && successorLocation.MovementCost > onClosedLocations.Min(o => o.MovementCost)))
                    {
                        ; //Do nothing (effectively get rid of the successor
                    }
                }

                openLocations.Remove(currentLocation);
                closedLocations.Add(currentLocation);
                currentLocation = null;
            }

            if (currentLocation == null)
            {
                return null;
            }

            PathLocation locationToMoveTo = currentLocation.ParentPathLocation;

            while ( locationToMoveTo.ParentPathLocation != null && 
                locationToMoveTo.ParentPathLocation.ThisLocation != mobilePathLocation.ThisLocation)
            {
                locationToMoveTo = locationToMoveTo.ParentPathLocation;
            }

            return this.GetTileForLocation(locationToMoveTo.ThisLocation);
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
                .Where(m => m.Mobile != null && m.Mobile is Player)
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
                    new Wall { Location = new Location(1,1) },
                    new Wall { Location = new Location(2,1) },
                    new Wall { Location = new Location(3,1) },
                    new Wall { Location = new Location(4,1) },
                    new Wall { Location = new Location(5,1) },
                    new Wall { Location = new Location(6,1) },
                    new Wall { Location = new Location(7,1) },
                    new Wall { Location = new Location(8,1) },
                    new Wall { Location = new Location(9,1) },
                    new Wall { Location = new Location(10,1) },
                    new Wall { Location = new Location(11,1) },
                    new Wall { Location = new Location(12,1) },
                    new Wall { Location = new Location(13,1) },
                    new Wall { Location = new Location(14,1) },
                    new Wall { Location = new Location(15,1) },
                    new Wall { Location = new Location(1,2) },
                    new Wall { Location = new Location(2,2) },
                    new Floor { Location = new Location(3,2), Mobile = new Player() },
                    new Floor { Location = new Location(4,2) },
                    new Floor { Location = new Location(5,2) },
                    new Door { Location = new Location(6,2) },
                    new Floor { Location = new Location(7,2) },
                    new Floor { Location = new Location(8,2) },
                    new Floor { Location = new Location(9,2), Mobile = new Bob() },
                    new Wall { Location = new Location(10,2) },
                    new Wall { Location = new Location(11,2) },
                    new Wall { Location = new Location(12,2) },
                    new Wall { Location = new Location(13,2) },
                    new Wall { Location = new Location(14,2) },
                    new Wall { Location = new Location(15,2) },
                    new Wall { Location = new Location(1,3) },
                    new Wall { Location = new Location(2,3) },
                    new Floor { Location = new Location(3,3) },
                    new Wall { Location = new Location(4,3) },
                    new Wall { Location = new Location(5,3) },
                    new Wall { Location = new Location(6,3) },
                    new Floor { Location = new Location(7,3) },
                    new Floor { Location = new Location(8,3) },
                    new Floor { Location = new Location(9,3) },
                    new Floor { Location = new Location(10,3) },
                    new Floor { Location = new Location(11,3) },
                    new Wall { Location = new Location(12,3) },
                    new Wall { Location = new Location(13,3) },
                    new Wall { Location = new Location(14,3) },
                    new Wall { Location = new Location(15,3) },
                    new Wall { Location = new Location(1,4) },
                    new Wall { Location = new Location(2,4) },
                    new Floor { Location = new Location(3,4) },
                    new Wall { Location = new Location(4,4) },
                    new Wall { Location = new Location(5,4) },
                    new Wall { Location = new Location(6,4) },
                    new Wall { Location = new Location(7,4) },
                    new Wall { Location = new Location(8,4) },
                    new Wall { Location = new Location(9,4) },
                    new Wall { Location = new Location(10,4) },
                    new Floor { Location = new Location(11,4) },
                    new Wall { Location = new Location(12,4) },
                    new Wall { Location = new Location(13,4) },
                    new Wall { Location = new Location(14,4) },
                    new Wall { Location = new Location(15,4) },
                    new Wall { Location = new Location(1,5) },
                    new Wall { Location = new Location(2,5) },
                    new Floor { Location = new Location(3,5) },
                    new Floor { Location = new Location(4,5) },
                    new Floor { Location = new Location(5,5) },
                    new Floor { Location = new Location(6,5) },
                    new Wall { Location = new Location(7,5) },
                    new Wall { Location = new Location(8,5) },
                    new Wall { Location = new Location(9,5) },
                    new Wall { Location = new Location(10,5) },
                    new Floor { Location = new Location(11,5) },
                    new Wall { Location = new Location(12,5) },
                    new Wall { Location = new Location(13,5) },
                    new Wall { Location = new Location(14,5) },
                    new Wall { Location = new Location(15,5) },
                    new Wall { Location = new Location(1,6) },
                    new Wall { Location = new Location(2,6) },
                    new Floor { Location = new Location(3,6) },
                    new Wall { Location = new Location(4,6) },
                    new Wall { Location = new Location(5,6) },
                    new Wall { Location = new Location(6,6) },
                    new Wall { Location = new Location(7,6) },
                    new Floor { Location = new Location(8,6) },
                    new Floor { Location = new Location(9,6) },
                    new Wall { Location = new Location(10,6) },
                    new Floor { Location = new Location(11,6) },
                    new Wall { Location = new Location(12,6) },
                    new Wall { Location = new Location(13,6) },
                    new Wall { Location = new Location(14,6) },
                    new Wall { Location = new Location(15,6) },
                    new Wall { Location = new Location(1,7) },
                    new Wall { Location = new Location(2,7) },
                    new Floor { Location = new Location(3,7) },
                    new Wall { Location = new Location(4,7) },
                    new Wall { Location = new Location(5,7) },
                    new Wall { Location = new Location(6,7) },
                    new Wall { Location = new Location(7,7) },
                    new Wall { Location = new Location(8,7) },
                    new Floor { Location = new Location(9,7) },
                    new Floor { Location = new Location(10,7) },
                    new Floor { Location = new Location(11,7) },
                    new Wall { Location = new Location(12,7) },
                    new Wall { Location = new Location(13,7) },
                    new Wall { Location = new Location(14,7) },
                    new Wall { Location = new Location(15,7) },
                    new Wall { Location = new Location(1,8) },
                    new Wall { Location = new Location(2,8) },
                    new Floor { Location = new Location(3,8) },
                    new Floor { Location = new Location(4,8) },
                    new Floor { Location = new Location(5,8) },
                    new Wall { Location = new Location(6,8) },
                    new Wall { Location = new Location(7,8) },
                    new Wall { Location = new Location(8,8) },
                    new Floor { Location = new Location(9,8) },
                    new Wall { Location = new Location(10,8) },
                    new Wall { Location = new Location(11,8) },
                    new Wall { Location = new Location(12,8) },
                    new Wall { Location = new Location(13,8) },
                    new Wall { Location = new Location(14,8) },
                    new Wall { Location = new Location(15,8) },
                    new Wall { Location = new Location(1,9) },
                    new Wall { Location = new Location(2,9) },
                    new Floor { Location = new Location(3,9) },
                    new Floor { Location = new Location(4,9) },
                    new Floor { Location = new Location(5,9) },
                    new Wall { Location = new Location(6,9) },
                    new Wall { Location = new Location(7,9) },
                    new Wall { Location = new Location(8,9) },
                    new Floor { Location = new Location(9,9), Mobile = new Rat() },
                    new Floor { Location = new Location(10,9) },
                    new Floor { Location = new Location(11,9) },
                    new Floor { Location = new Location(12,9) },
                    new Floor { Location = new Location(13,9) },
                    new Floor { Location = new Location(14,9) },
                    new Wall { Location = new Location(15,9) },
                    new Wall { Location = new Location(1,10) },
                    new Wall { Location = new Location(2,10) },
                    new Wall { Location = new Location(3,10) },
                    new Wall { Location = new Location(4,10) },
                    new Wall { Location = new Location(5,10) },
                    new Wall { Location = new Location(6,10) },
                    new Wall { Location = new Location(7,10) },
                    new Wall { Location = new Location(8,10) },
                    new Wall { Location = new Location(9,10) },
                    new Wall { Location = new Location(10,10) },
                    new Wall { Location = new Location(11,10) },
                    new Wall { Location = new Location(12,10) },
                    new Wall { Location = new Location(13,10) },
                    new Wall { Location = new Location(14,10) },
                    new Wall { Location = new Location(15,10) },
                    
                   
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
    }

    public class PathLocation
    {
        public PathLocation(Location currentLocation, int movementCost, PathLocation parentPathLocation)
        {
            this.ThisLocation = currentLocation;
            this.ParentPathLocation = parentPathLocation;
            this.MovementCost = movementCost;
        }

        public Location ThisLocation { get; set; }
        public PathLocation ParentPathLocation { get; set; }
        public int MovementCost { get; set; }
    }
 
}
