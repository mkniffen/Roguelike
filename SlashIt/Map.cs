using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SlashIt
{
    public class Wall : Tile
    {
        public Wall()
        {
            Description = "A brick wall"; 
            DisplayCharacter = "#";
            Name = "Wall";
            UniqueId = Constants.UniqueIds.Wall;
        }
    }

    public class Floor : Tile
    {
        public Floor()
        {
            Description = "Empty floor";
            DisplayCharacter = " "; 
            Name = "Floor";
            UniqueId = Constants.UniqueIds.Floor;
        }
    }

    public class Door : Tile
    {
        public Door()
        {
            Description = "A big wooden door.  It's closed";
            DisplayCharacter = "+";
            Name = "Door";
            UniqueId = Constants.UniqueIds.Door;
        }
    }

    //new Tile { Description = "An open door", DisplayCharacter = "`", Name = "OpenDoor", UniqueId = Constants.UniqueIds.OpenDoor },

    public class OpenDoor : Tile
    {
        public OpenDoor()
        {
            Description = "An open door";
            DisplayCharacter = "`";
            Name = "OpenDoor";
            UniqueId = Constants.UniqueIds.OpenDoor;
        }
    }

    public class Map
    {
        [XmlIgnore]
        public List<IMapObject> MapObjects { get; set; }

        public Map()
        {
            this.MapObjects = new List<IMapObject>();
            this.LoadTiles();
        }




        public Tile GetTileForLocation(Location mapLocation)
        {
            var tileToMoveTo = (Tile)this.MapObjects
                .Where(m => m.Location.Left == mapLocation.Left && m.Location.Top == mapLocation.Top)
                .Single();
            return tileToMoveTo;
        }

        public Tile GetPlayerTile()
        {
            var mapTile = (Tile)this.MapObjects
                .Where(m => ((Tile)m).Player != null)
                .Single();
            return mapTile;
        }

        private void LoadTiles()
        {
            MapObjects.AddRange(
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
                    new Wall { Location = new Location(1,2) },
                    new Wall { Location = new Location(2,2) },
                    new Floor { Location = new Location(3,2), Player = new Player() },
                    new Floor { Location = new Location(4,2) },
                    new Floor { Location = new Location(5,2) },
                    new Door { Location = new Location(6,2) },
                    new Floor { Location = new Location(7,2) },
                    new Floor { Location = new Location(8,2) },
                    new Floor { Location = new Location(9,2) },
                    new Wall { Location = new Location(10,2) },
                    new Wall { Location = new Location(1,3) },
                    new Wall { Location = new Location(2,3) },
                    new Wall { Location = new Location(3,3) },
                    new Wall { Location = new Location(4,3) },
                    new Wall { Location = new Location(5,3) },
                    new Wall { Location = new Location(6,3) },
                    new Wall { Location = new Location(7,3) },
                    new Wall { Location = new Location(8,3) },
                    new Wall { Location = new Location(9,3) },
                    new Wall { Location = new Location(10,3) },
                    
                   
                });
        }
    }


    /*
    public class Map : IXmlSerializable
    {
        public Map()
        {
            MapOutdated = true;
            //MapFile = @".\game.sav";
        }

        //public string MapFile { get; set; }

        //TODO not sure about this syntax.  Gonna give it a try for now...
        public int this[int top, int left]
        {
            get { return this.map[top,left]; }
            set { this.map[top, left] = value; }
        }


        public bool MapOutdated { get; set; }

        public int GetUpperBound(int dimension)
        {
            return map.GetUpperBound(dimension);
        }

        private int[,] map = new int[10, 10];


        public Map Load(StreamReader saveFileStream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Map));
            return (Map)serializer.Deserialize(saveFileStream);
        }


        public void Save(TextWriter saveFileWriter)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Map));

            serializer.Serialize(saveFileWriter, this);
        }


        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }


        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();


            
            //TODO -- Will need to add reading and setting of the Height, Width attributes


            var isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();

            if (!isEmptyElement)
            {
                var definitionList = reader.ReadElementString("Definition").Split(',');
                int listPosition = 0;

                for (int top = 0; top <= map.GetUpperBound(0); top++)
                {
                    for (int left = 0; left <= map.GetUpperBound(1); left++)
                    {
                        map[top, left] = int.Parse(definitionList[listPosition]);
                        listPosition++;
                    }
                }
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("Height", (this.GetUpperBound(0)+1).ToString());
            writer.WriteAttributeString("Width", (this.GetUpperBound(1)+1).ToString());
            writer.WriteElementString("Definition", this.MapToString());
        }

        private string MapToString()
        {
            StringBuilder mapString = new StringBuilder();

            for (int top = 0; top <= this.GetUpperBound(0); top++)
            {
                for (int left = 0; left <= this.GetUpperBound(1); left++)
                {
                    mapString.Append(map[top, left]);
                    mapString.Append(",");
                }
            }

            return mapString.ToString();
        }
    }
     */
}
