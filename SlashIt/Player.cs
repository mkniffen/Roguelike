using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SlashIt
{
    public class Player : IMapObject
    {
        public Player()
        {
            Description = "This guy is a newb!!";
            DisplayCharacter = "@";
            Name = "Player";
            UniqueId = Constants.UniqueIds.Player;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayCharacter { get; set; }
        public int UniqueId { get; set; }

        //public TileType TileType { get; set; }
        public Location Location { get; set; }

        public bool CanMoveTo(IMapObject mapObject)
        {
            List<int> canMoveToTiles = new List<int>
            {
                Constants.UniqueIds.Floor,
                Constants.UniqueIds.OpenDoor
            };


            //See if this map object can make the requested move

            return canMoveToTiles.Contains(mapObject.UniqueId);
        }







        //public int Left { get; set; }
        //public int Top { get; set; }

        //public int LeftBeforeMove { get; set; }
        //public int TopBeforeMove { get; set; }

        //public int LeftMapPosition { get { return this.Left - Game.MapStartLeft; } }
        //public int TopMapPosition { get { return this.Top - Game.MapStartTop; } }

        //public int LeftMapPositionBeforeMove { get { return this.LeftBeforeMove - Game.MapStartLeft; } }
        //public int TopMapPositionBeforeMove { get { return this.TopBeforeMove - Game.MapStartTop; } }

        //public void DisallowMove()
        //{
        //    this.Left = this.LeftBeforeMove;
        //    this.Top = this.TopBeforeMove;
        //}

        //public void SetPosition(int left, int top)
        //{
        //    this.Left = left;
        //    this.Top = top;

        //    this.SetPositionBeforeMove();
        //}

        //public void SetPositionBeforeMove()
        //{
        //    this.LeftBeforeMove = this.Left;
        //    this.TopBeforeMove = this.Top;
        //}

        //public void Move(ConsoleKey consoleKey, Map map)
        //{
        //    switch (consoleKey)
        //    {
        //        case ConsoleKey.UpArrow:
        //            this.Top--;
        //            break;

        //        case ConsoleKey.DownArrow:
        //            this.Top++;
        //            break;

        //        case ConsoleKey.LeftArrow:
        //            this.Left--;
        //            break;

        //        case ConsoleKey.RightArrow:
        //            this.Left++;
        //            break;

        //        default:
        //            break;
        //    }
        //}

        
        

        public void Save(TextWriter saveFileWriter)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Player));

            serializer.Serialize(saveFileWriter, this);
        }

        public Player Load(StreamReader saveFileStream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Player));
            return (Player)serializer.Deserialize(saveFileStream);
        }
    }
}
