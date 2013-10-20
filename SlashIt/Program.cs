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
            Status.ClearInfo();
            Status.Info = "Start a New game, or Load an existing (S/L)?";
            Status.WriteToStatus();

            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
            var localKeyInfo = new LocalKeyInfo(consoleKeyInfo);

            XDocument doc;

            switch (localKeyInfo.Key)
            {
                case ConsoleKey.S:
                    doc = XDocument.Load(@".\new.sav");
                    break;

                case ConsoleKey.L:
                    doc = XDocument.Load(@".\game.sav");
                    break;

                default:
                    Program.Load();
                    return;
            }

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
                    var nonTurnAction = HandleInput();
                    if (nonTurnAction) continue;

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
                Status.WriteToStatusLine("Game over!");
            }

            Console.ReadKey(true);
        }

        static Random r = new Random();

        public static int RandomNumber(int max)
        {
            return r.Next(max);
        }


        private static bool HandleInput()
        {
            bool nonTurnAction = true;

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            var localKeyInfo = new LocalKeyInfo(keyInfo);

            Status.ClearInfo();
            Status.WriteToStatusLine("");

            nonTurnAction = game.CommandActivated(localKeyInfo);

            return nonTurnAction;
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
                    case "a":
                        key = ConsoleKey.A;
                        break;
                    case "e":
                        key = ConsoleKey.E;
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
                    case "i":
                        key = ConsoleKey.I;
                        break;
                    case "l":
                        key = ConsoleKey.L;
                        break;
                    case "o":
                        key = ConsoleKey.O;
                        break;
                    case "p":
                        key = ConsoleKey.P;
                        break;
                    case "w":
                        key = ConsoleKey.W;
                        break;
                    default:
                        continue;
                }


                switch (binding.Key)
                {
                    case "Attack":
                        command = new AttackCommand(map);
                        localKey = new LocalKeyInfo(key, false, false, false);
                        break;
                    case "DisplayEquipment":
                        command = new DisplayEquipmentCommand(map);
                        localKey = new LocalKeyInfo(key, false, false, false);
                        break;
                    case "Inventory":
                        command = new InventoryCommand(map);
                        localKey = new LocalKeyInfo(key, false, false, false);
                        break;
                    case "Look":
                        command = new LookCommand(map);
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
                    case "OpenClose":
                        command = new OpenCloseCommand(map);
                        localKey = new LocalKeyInfo(key, false, false, false);
                        break;
                    case "PickUp":
                        command = new PickUpCommand(map);
                        localKey = new LocalKeyInfo(key, false, false, false);
                        break;
                    case "Wear":
                        command = new WearCommand(map);
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
