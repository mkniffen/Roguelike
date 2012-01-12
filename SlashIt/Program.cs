using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    public class Program
    {
        static Game game = new Game();
        static Map map = new Map();

        public static bool Quit { get; set; }

        public static void Save()
        {
            //Status.ClearInfo();
            //Status.Info = "Game Saved.";

            //using (TextWriter tw = new StreamWriter(SaveFile))
            //{
            //    //TODO -- Move this up too (along with Load)???

            //    XmlSerializer serializer = new XmlSerializer(typeof(Game));
            //    serializer.Serialize(tw, this);

            //    tw.Close();
            //}
        }

        private void Load()
        {

            //using (StreamReader saveFileStream = new StreamReader(SaveFile))
            //{

            //    //TODO -- Not sure if this is the best place to do this.  Should I move this to a higher level (Program)??
            //    XmlSerializer serializer = new XmlSerializer(typeof(Game));
            //    var game = (Game)serializer.Deserialize(saveFileStream);

            //    this.Map = game.Map;
            //    //this.NonPlayerCharacters = game.NonPlayerCharacters;

            //    saveFileStream.Close();
            //}


        }

        static void Main(string[] args)
        {
            InitCommands();
            game.Map = map;
            game.InitConsole();

            while (!Quit)
            {
                game.WriteConsole();
                HandleInput();
                game.MoveNonPlayerCharacters();
            }

            Console.WriteLine("Game over!");
        }
        static Random r = new Random();

        public static int RandomNumber(int max)
        {
            return r.Next(max);
        }


        private static void HandleInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            var localKeyInfo = new LocalKeyInfo(keyInfo);

            Status.ClearInfo();

            game.CommandActivated(localKeyInfo);
        }

        //TODO -- eventually refactor so the commands are set up using a config file with bindable keys
        private static void InitCommands()
        {
            var openCloseCommand = new OpenCloseCommand(map);
            var lk = new LocalKeyInfo(ConsoleKey.O, false, false, false);
            game.SetCommand(lk, openCloseCommand);

            var movePlayerCommand = new MoveMapPlayerCommand(map);
            lk = new LocalKeyInfo(ConsoleKey.DownArrow, false, false, false);
            game.SetCommand(lk, movePlayerCommand);

            movePlayerCommand = new MoveMapPlayerCommand(map);
            lk = new LocalKeyInfo(ConsoleKey.LeftArrow, false, false, false);
            game.SetCommand(lk, movePlayerCommand);

            movePlayerCommand = new MoveMapPlayerCommand(map);
            lk = new LocalKeyInfo(ConsoleKey.RightArrow, false, false, false);
            game.SetCommand(lk, movePlayerCommand);

            movePlayerCommand = new MoveMapPlayerCommand(map);
            lk = new LocalKeyInfo(ConsoleKey.UpArrow, false, false, false);
            game.SetCommand(lk, movePlayerCommand);

            var lookCommand = new LookCommand(map);
            lk = new LocalKeyInfo(ConsoleKey.L, false, false, false);
            game.SetCommand(lk, lookCommand);

            var quitCommand = new QuitCommand(map);
            lk = new LocalKeyInfo(ConsoleKey.Q, false, false, false);
            game.SetCommand(lk, quitCommand);
        }
    }
}
