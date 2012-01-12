using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SlashIt
{

    public class Game// : IXmlSerializable
    {
        const string SaveFile = @".\game.sav";
        Dictionary<LocalKeyInfo, ICommand> commands; 

        [XmlIgnore]
        public Map Map { get; set; }
        
        public const short MapStartLeft = 19;
        public const short MapStartTop = 0;

        public Game()
        {
            commands = new Dictionary<LocalKeyInfo, ICommand>();
        }

        public void SetCommand(LocalKeyInfo keyInfo, ICommand command)
        {
            commands.Add(keyInfo, command);
        }

        public void CommandActivated(LocalKeyInfo keyInfo)
        {
            commands[keyInfo].execute(keyInfo);
        }


        public void InitConsole()
        {
            Console.TreatControlCAsInput = true;

            Console.Clear();
            Console.SetWindowSize(80, 25);
            Console.BufferWidth = 80;
            Console.BufferHeight = 25;

            Console.CursorVisible = false;
            
            //this.Load();


            //TODO !!!!!!!!!! temp code to load up a monster...  change this!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
            //#region ChangeMe
            //var newNPC = new NonPlayerCharacter();
            //    Map[newNPC.Location.TopMapPosition, newNPC.Location.LeftMapPosition] = newNPC.UniqueId;
            //    MapObjects.Add(newNPC);
            //#endregion
            

            this.WriteConsole();

            Status.Message = "Height: " + Console.WindowHeight + " Width: " + Console.WindowWidth;
        }


        //TODO eventually optimize how and when things are drawn.  don't necessarily need to redraw all the time.
        //TODO maybe have use a second array that tracks what should be drawn on the grid.
        public void WriteConsole()
        {
            //if (Map.MapOutdated)
            //{
                Console.Clear();
                this.GenerateMap();  //TODO only do this when really needed
            //}

            

         //   Status.Message = "Map Left: " + character.LeftMapPosition + " :MapTop: " + character.TopMapPosition;

            Status.WriteToStatus();
        }


        internal void GenerateMap()
        {
            foreach (Tile tile in this.Map.Tiles)
            {
                Console.SetCursorPosition(tile.Location.Left + Game.MapStartLeft, tile.Location.Top + Game.MapStartTop);

                if (tile.Mobile != null)
                {
                    Console.Write(tile.Mobile.DisplayCharacter);
                    continue;
                }

                Console.Write(tile.DisplayCharacter);
            }

            //Map.MapOutdated = false;

        }


        //public System.Xml.Schema.XmlSchema GetSchema()
        //{
        //    return (null);
        //}

        //public void ReadXml(System.Xml.XmlReader reader)
        //{
        //    reader.MoveToContent();
        //    reader.ReadStartElement();

        //    XmlSerializer serializer;

        //    while (!reader.EOF)
        //    {
        //        switch (reader.Name)
        //        {
        //            case "Player":
        //                serializer = new XmlSerializer(typeof(Player));
        //                this.Player = (Player) serializer.Deserialize(reader);
        //                continue;
        //            case "Map":
        //                serializer = new XmlSerializer(typeof(Map));
        //                this.Map = (Map)serializer.Deserialize(reader);
        //                continue;
        //            //case "NonPlayerCharacters":
        //            //    reader.ReadStartElement();
        //            //    serializer = new XmlSerializer(typeof(IMapObject));
        //            //    var iMapObject = (IMapObject)serializer.Deserialize(reader);
        //            //    this.NonPlayerCharacters.Add(iMapObject);
        //            //    continue;
        //            default:
        //                break;
        //        }

        //        reader.ReadStartElement();
        //    }
        //}

        //public void WriteXml(System.Xml.XmlWriter writer)
        //{
        //    throw new NotImplementedException();
        //}
    }
}