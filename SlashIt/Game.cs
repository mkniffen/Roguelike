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
            commands[keyInfo].execute();
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
            foreach (Tile tile in this.Map.MapObjects)
            {
                Console.SetCursorPosition(tile.Location.Left + Game.MapStartLeft, tile.Location.Top + Game.MapStartTop);

                if (tile.Player != null)
                {
                    Console.Write(tile.Player.DisplayCharacter);
                    continue;
                }

                Console.Write(tile.DisplayCharacter);
            }

            //Map.MapOutdated = false;

        }

        public void DoMoveMapPlayer(LocalKeyInfo keyInfo)
        {
            var mapTile = this.Map.GetPlayerTile();

            var mapLocation = new Location(mapTile.Location.Left, mapTile.Location.Top);

            var tileToMoveTo = this.Map.GetTileToMoveTo(keyInfo, mapLocation);

            if (mapTile.Player.CanMoveTo(tileToMoveTo))
            {
                tileToMoveTo.Player = mapTile.Player;
                tileToMoveTo.Player.Location = mapLocation;
                mapTile.Player = null;
            }
        }



        //TODO Move to command...
        public void DoLook()
        {
            //TODO -- will need to modify this so that all the items in the tile are put in the Info

            Status.Info = this.Map.GetPlayerTile().Description;
        }

        public bool Quit()
        {
            bool quit = true;

            Status.ClearInfo();
            Status.Info = "Really quit (Y or N)?";
            Status.WriteToStatus();

            var keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Y)
            {
                ;
            }
            else if (keyInfo.Key == ConsoleKey.N)
            {
                Status.ClearInfo();
                Status.Info = "Returning to game.";
                quit = false;
            }
            else
            {
                this.Quit();
            }

            return quit;
        }



        //TODO -- move the details to config
       


        public void Save()
        {
            Status.ClearInfo();
            Status.Info = "Game Saved.";

            using (TextWriter tw = new StreamWriter(SaveFile))
            {
                //TODO -- Move this up too (along with Load)???

                XmlSerializer serializer = new XmlSerializer(typeof(Game));
                serializer.Serialize(tw, this);

                tw.Close();
            }
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

        private void Load()
        {

            using (StreamReader saveFileStream = new StreamReader(SaveFile))
            {

                //TODO -- Not sure if this is the best place to do this.  Should I move this to a higher level (Program)??
                XmlSerializer serializer = new XmlSerializer(typeof(Game));
                var game = (Game)serializer.Deserialize(saveFileStream);

                this.Map = game.Map;
                //this.NonPlayerCharacters = game.NonPlayerCharacters;

                saveFileStream.Close();
            }


        }
    }
}