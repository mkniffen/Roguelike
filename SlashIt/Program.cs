using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SlashIt
{
    public class Program
    {
        static Game game = new Game();
        static Map map = new Map();

        public static bool Quit { get; set; }

        public static void Save()
        {
            Status.ClearInfo();
            Status.Info = "Game Saved.";

            XDocument doc = new XDocument(
                new XElement("SlashIt",
                    new XElement("Map", map.Save())
                    ));

            doc.Save(@".\game.sav");
        }

        private static void Load()
        {
            XDocument doc = XDocument.Load(@".\game.sav");
            map.Load(doc.Descendants("Tiles"));
        }

        static void Main(string[] args)
        {
            InitCommands();
            Load();

            game.Map = map;
            game.InitConsole();

            while (!Quit && !game.PlayerIsDead)
            {
                game.WriteConsole();
                if (game.PlayerCanAct())
                {
                    HandleInput();
                    map.GetPlayer().TimeBucket = 0;
                }
                game.PerformNonPlayerCharacterAction();
                game.AdvanceTime();
            }

            if (game.PlayerIsDead)
            {
                Status.WriteToStatusLine("You are Dead!!!!!!!!!!!");
            }
            else
            {
                Console.WriteLine("Game over!");
            }

            Console.ReadKey();
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
            Status.WriteToStatusLine("");

            game.CommandActivated(localKeyInfo);
        }

        
        private static void InitCommands()
        {
            //TODO -- eventually move the selection of key bindings to the ui in game


            //TODO Is there a better way??

            var bindings = ReadConfig("KeyBindings");

            ICommand command = null;
            ConsoleKey key = ConsoleKey.A;
            LocalKeyInfo localKey = null;

            foreach (var binding in bindings)
            {
             
                switch (binding.Value)
                {
                    case "o":
                        key = ConsoleKey.O;
                        break;
                    case "UpArrow":
                        key = ConsoleKey.UpArrow;
                        break;
                    case "DownArrow":
                        key = ConsoleKey.DownArrow;
                        break;
                    case "RightArrow":
                        key = ConsoleKey.RightArrow;
                        break;
                    case "LeftArrow":
                        key = ConsoleKey.LeftArrow;
                        break;
                    case "l":
                        key = ConsoleKey.L;
                        break;
                    case "a":
                        key = ConsoleKey.A;
                        break;
                    default:
                        continue;
                }


                switch (binding.Key)
                {
                    case "OpenClose":
                        command = new OpenCloseCommand(map);
                        localKey = new LocalKeyInfo(key, false, false, false);
                        break;
                    case "MoveUp":
                        command = new MoveMapPlayerCommand(map);
                        localKey = new LocalKeyInfo(key, false, false, false);
                        break;
                    case "MoveDown":
                        command = new MoveMapPlayerCommand(map);
                        localKey = new LocalKeyInfo(key, false, false, false);
                        break;
                    case "MoveRight":
                        command = new MoveMapPlayerCommand(map);
                        localKey = new LocalKeyInfo(key, false, false, false);
                        break;
                    case "MoveLeft":
                        command = new MoveMapPlayerCommand(map);
                        localKey = new LocalKeyInfo(key, false, false, false);
                        break;
                    case "Look":
                        command = new LookCommand(map);
                        localKey = new LocalKeyInfo(key, false, false, false);
                        break;
                    case "Attack":
                        command = new AttackCommand(map);
                        localKey = new LocalKeyInfo(key, false, false, false);
                        break;
                    default:
                        break;
                }

                game.SetCommand(localKey, command);
            }

            //var lookCommand = new LookCommand(map);
            //lk = new LocalKeyInfo(ConsoleKey.L, false, false, false);
            //game.SetCommand(lk, lookCommand);

            #region Non Bindable Keys

            var quitCommand = new QuitCommand(map);
            var lk = new LocalKeyInfo(ConsoleKey.Q, false, false, false);
            game.SetCommand(lk, quitCommand);

            #endregion
        }

        private static Dictionary<string, string> ReadConfig(string p)
        {
            XDocument doc = XDocument.Load(@".\Config\Config.xml");

            return doc.Descendants("KeyBindings")
                .SelectMany(kb => kb.Elements())
                .Select(e => new { e.Name, e.Value })
                .ToDictionary(x => x.Name.ToString(), x => x.Value.ToString());
        }
    }
}
