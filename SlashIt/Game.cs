using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SlashIt
{

    public class Game
    {
        const string SaveFile = @".\game.sav";

        public Game()
        {
            Map = new Map();
            NonPlayerCharacters = new List<NonPlayerCharacter>();
        }

        public Character Character { get; set; }

        public List<NonPlayerCharacter> NonPlayerCharacters { get; set; }

        public Map Map { get; set; }
        public const short MapStartLeft = 20;
        public const short MapStartTop = 1;

        public void InitConsole()
        {
            Console.TreatControlCAsInput = true;

            Console.Clear();
            Console.SetWindowSize(80, 25);
            Console.BufferWidth = 80;
            Console.BufferHeight = 25;

            Console.CursorVisible = false;

            Character = new Character();
            Character.SetPosition(22, 2);  //TODO Const

            this.Load();

            if (NonPlayerCharacters.Count < 1)
            {
                var newNPC = new NonPlayerCharacter();
                Map[newNPC.Location.TopMapPosition, newNPC.Location.LeftMapPosition] = newNPC.UniqueId;
                NonPlayerCharacters.Add(newNPC);
            }

            this.WriteConsole();

            Status.Message = "Height: " + Console.WindowHeight + " Width: " + Console.WindowWidth;
        }


        //TODO eventually optimize how and when things are drawn.  don't necessarily need to redraw all the time.
        //TODO maybe have use a second array that tracks what should be drawn on the grid.
        public void WriteConsole()
        {
            if (Map.MapOutdated)
            {
                Console.Clear();
                this.GenerateMap();  //TODO only do this when really needed
            }

            Console.SetCursorPosition(Character.LeftBeforeMove, Character.TopBeforeMove);
            Console.Write(CellCharacter(Character.LeftMapPositionBeforeMove, Character.TopMapPositionBeforeMove));
            Console.SetCursorPosition(Character.Left, Character.Top);
            Console.Write("@");
            Character.SetPositionBeforeMove();

         //   Status.Message = "Map Left: " + character.LeftMapPosition + " :MapTop: " + character.TopMapPosition;

            Status.WriteToStatus();
        }


        internal void GenerateMap()
        {
            int mapStartLeft = Game.MapStartLeft;
            int mapStartTop = Game.MapStartTop;

            Console.SetCursorPosition(mapStartLeft, mapStartTop);

            for (int top = 0; top <= this.Map.GetUpperBound(0); top++)
            {
                for (int left = 0; left <= this.Map.GetUpperBound(1); left++)
                {
                    Console.Write(CellCharacter(left, top));
                }

                Console.SetCursorPosition(mapStartLeft, ++mapStartTop);

            }

            Map.MapOutdated = false;

        }

        private string CellCharacter(int left, int top)
        {
           //TODO -- Can stop using the switch.  Instead use linq on the id returned from the map matrix.  Just
            //                                   make all map objects use the same base class (or interface) that has
            //                                   a uniqueId that can be easily looked up.  Should be able to apply this technique
            //                                   throughout.  Give it a try and see how you like it.

                    switch (this.Map[top, left])
                    {
                        case (0):
                            //Hallway
                            return " ";
                        case (1):
                            //Wall
                            return "#";
                        case (2):
                            //Closed Door
                            return "+";
                        case (3):
                            //Open Door
                            return "`";
                        case (111):
                            return NonPlayerCharacters.Where(n => n.UniqueId == 111).Single().DisplayCharacter;
                        default:
                            return " ";
                    }
        }

        public void CheckBounds()
        {
            //TODO -- Improve this
            if (Map[Character.TopMapPosition, Character.LeftMapPosition] != 0 && Map[Character.TopMapPosition, Character.LeftMapPosition] != 3)
            {
                Character.DisallowMove();
            }
        }

        //TODO -- Refactor!!!!!!!!!!
        public void DoLook()
        {
            switch (Map[Character.TopMapPosition, Character.LeftMapPosition])
            {
                case (0):
                    Status.Info = "You see empty floor";
                    break;
                case (2):
                    Status.Info = "A big wooden door.  It's closed.";
                    break;
                case (3):
                    Status.Info = "An open door";
                    break;
                default:
                    break;
            }
        }



        public void DoOpenClose()
        {
            Status.ClearInfo();
            Status.Info = "Which direction?";
            Status.WriteToStatus();

            var keyInfo = Console.ReadKey(true);

            //TODO -- Taking key input exists in more than one place now, REFACTOR

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                Status.ClearInfo();
                return;
            }

            var mapLocation = Character.Move(keyInfo.Key);

            switch (Map[mapLocation.Top, mapLocation.Left])
            {
                case (2):
                    //Replace closed door with open door in map matrix
                    Map[mapLocation.Top, mapLocation.Left] = 3;
                    Map.MapOutdated = true;
                    Status.ClearInfo();
                    break;
                case (3):
                    //Replace open door with closed door in map matrix
                    Map[mapLocation.Top, mapLocation.Left] = 2;
                    Map.MapOutdated = true;
                    Status.ClearInfo();
                    break;
                default:
                    Status.Info = "That can't be opened or closed.  Press any key to continue.";
                    Status.WriteToStatus();

                    Console.ReadKey(true);
                    DoOpenClose();
                    break;
            }
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

        private void Load()
        {

            using (StreamReader saveFileStream = new StreamReader(SaveFile))
            {

        //TODO -- Not sure if this is the best place to do this.  Should I move this to a higher level (Program)??
                XmlSerializer serializer = new XmlSerializer(typeof(Game));
                var game = (Game)serializer.Deserialize(saveFileStream);

                this.Map = game.Map;
                this.Character = game.Character;
                //this.NonPlayerCharacters = game.NonPlayerCharacters;

                saveFileStream.Close();
            }
        }


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
    }
}