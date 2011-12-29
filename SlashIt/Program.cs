using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    class Program
    {
        static bool quit = false;
        static Game game;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

            game = new Game();
            game.InitConsole();

            MainLoop();

        }

        private static void MainLoop()
        {
            while (!quit)
            {
                game.WriteConsole();
                HandleInput();
                game.CheckBounds();
            }

            Console.WriteLine("Game over!");
        }

        /// <summary>
        /// Event handler for ^C key press
        /// </summary>
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            // Unfortunately, due to a bug in .NET Framework v4.0.30319 you can't debug this 
            // because Visual Studio 2010 gives a "No Source Available" error. 
            // http://connect.microsoft.com/VisualStudio/feedback/details/524889/debugging-c-console-application-that-handles-console-cancelkeypress-is-broken-in-net-4-0

            quit = true;
            e.Cancel = true; // Set this to true to keep the process from quitting immediately
        }

        private static void HandleInput()
        {

            var keyInfo = Console.ReadKey(true);
            //  Status.Message = "Key: " + keyInfo.Key;

            Status.ClearInfo();

//TODO -- Look at a command pattern or maybe a factory.
            switch (keyInfo.Key)
            {
                case ConsoleKey.C:
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.LeftArrow:
                case ConsoleKey.RightArrow:
                case ConsoleKey.UpArrow:
                    game.character.Move(keyInfo.Key, game.Map);
                    break;
                case ConsoleKey.L:
                    game.DoLook();
                    break;
                case ConsoleKey.O:
                    game.DoOpenClose();
                    break;
                case ConsoleKey.Q:
                    quit = true;
                    break;

                #region Unused Console Keys

                case ConsoleKey.A:
                    break;
                case ConsoleKey.Add:
                    break;
                case ConsoleKey.Applications:
                    break;
                case ConsoleKey.Attention:
                    break;
                case ConsoleKey.B:
                    break;
                case ConsoleKey.Backspace:
                    break;
                case ConsoleKey.BrowserBack:
                    break;
                case ConsoleKey.BrowserFavorites:
                    break;
                case ConsoleKey.BrowserForward:
                    break;
                case ConsoleKey.BrowserHome:
                    break;
                case ConsoleKey.BrowserRefresh:
                    break;
                case ConsoleKey.BrowserSearch:
                    break;
                case ConsoleKey.BrowserStop:
                    break;
                case ConsoleKey.Clear:
                    break;
                case ConsoleKey.CrSel:
                    break;
                case ConsoleKey.D:
                    break;
                case ConsoleKey.D0:
                    break;
                case ConsoleKey.D1:
                    break;
                case ConsoleKey.D2:
                    break;
                case ConsoleKey.D3:
                    break;
                case ConsoleKey.D4:
                    break;
                case ConsoleKey.D5:
                    break;
                case ConsoleKey.D6:
                    break;
                case ConsoleKey.D7:
                    break;
                case ConsoleKey.D8:
                    break;
                case ConsoleKey.D9:
                    break;
                case ConsoleKey.Decimal:
                    break;
                case ConsoleKey.Delete:
                    break;
                case ConsoleKey.Divide:
                    break;
                case ConsoleKey.E:
                    break;
                case ConsoleKey.End:
                    break;
                case ConsoleKey.Enter:
                    break;
                case ConsoleKey.EraseEndOfFile:
                    break;
                case ConsoleKey.Escape:
                    break;
                case ConsoleKey.ExSel:
                    break;
                case ConsoleKey.Execute:
                    break;
                case ConsoleKey.F:
                    break;
                case ConsoleKey.F1:
                    break;
                case ConsoleKey.F10:
                    break;
                case ConsoleKey.F11:
                    break;
                case ConsoleKey.F12:
                    break;
                case ConsoleKey.F13:
                    break;
                case ConsoleKey.F14:
                    break;
                case ConsoleKey.F15:
                    break;
                case ConsoleKey.F16:
                    break;
                case ConsoleKey.F17:
                    break;
                case ConsoleKey.F18:
                    break;
                case ConsoleKey.F19:
                    break;
                case ConsoleKey.F2:
                    break;
                case ConsoleKey.F20:
                    break;
                case ConsoleKey.F21:
                    break;
                case ConsoleKey.F22:
                    break;
                case ConsoleKey.F23:
                    break;
                case ConsoleKey.F24:
                    break;
                case ConsoleKey.F3:
                    break;
                case ConsoleKey.F4:
                    break;
                case ConsoleKey.F5:
                    break;
                case ConsoleKey.F6:
                    break;
                case ConsoleKey.F7:
                    break;
                case ConsoleKey.F8:
                    break;
                case ConsoleKey.F9:
                    break;
                case ConsoleKey.G:
                    break;
                case ConsoleKey.H:
                    break;
                case ConsoleKey.Help:
                    break;
                case ConsoleKey.Home:
                    break;
                case ConsoleKey.I:
                    break;
                case ConsoleKey.Insert:
                    break;
                case ConsoleKey.J:
                    break;
                case ConsoleKey.K:
                    break;
                case ConsoleKey.LaunchApp1:
                    break;
                case ConsoleKey.LaunchApp2:
                    break;
                case ConsoleKey.LaunchMail:
                    break;
                case ConsoleKey.LaunchMediaSelect:
                    break;
                case ConsoleKey.LeftWindows:
                    break;
                case ConsoleKey.M:
                    break;
                case ConsoleKey.MediaNext:
                    break;
                case ConsoleKey.MediaPlay:
                    break;
                case ConsoleKey.MediaPrevious:
                    break;
                case ConsoleKey.MediaStop:
                    break;
                case ConsoleKey.Multiply:
                    break;
                case ConsoleKey.N:
                    break;
                case ConsoleKey.NoName:
                    break;
                case ConsoleKey.NumPad0:
                    break;
                case ConsoleKey.NumPad1:
                    break;
                case ConsoleKey.NumPad2:
                    break;
                case ConsoleKey.NumPad3:
                    break;
                case ConsoleKey.NumPad4:
                    break;
                case ConsoleKey.NumPad5:
                    break;
                case ConsoleKey.NumPad6:
                    break;
                case ConsoleKey.NumPad7:
                    break;
                case ConsoleKey.NumPad8:
                    break;
                case ConsoleKey.NumPad9:
                    break;
                case ConsoleKey.Oem1:
                    break;
                case ConsoleKey.Oem102:
                    break;
                case ConsoleKey.Oem2:
                    break;
                case ConsoleKey.Oem3:
                    break;
                case ConsoleKey.Oem4:
                    break;
                case ConsoleKey.Oem5:
                    break;
                case ConsoleKey.Oem6:
                    break;
                case ConsoleKey.Oem7:
                    break;
                case ConsoleKey.Oem8:
                    break;
                case ConsoleKey.OemClear:
                    break;
                case ConsoleKey.OemComma:
                    break;
                case ConsoleKey.OemMinus:
                    break;
                case ConsoleKey.OemPeriod:
                    break;
                case ConsoleKey.OemPlus:
                    break;
                case ConsoleKey.P:
                    break;
                case ConsoleKey.Pa1:
                    break;
                case ConsoleKey.Packet:
                    break;
                case ConsoleKey.PageDown:
                    break;
                case ConsoleKey.PageUp:
                    break;
                case ConsoleKey.Pause:
                    break;
                case ConsoleKey.Play:
                    break;
                case ConsoleKey.Print:
                    break;
                case ConsoleKey.PrintScreen:
                    break;
                case ConsoleKey.Process:
                    break;
                case ConsoleKey.R:
                    break;
                case ConsoleKey.RightWindows:
                    break;
                case ConsoleKey.S:
                    break;
                case ConsoleKey.Select:
                    break;
                case ConsoleKey.Separator:
                    break;
                case ConsoleKey.Sleep:
                    break;
                case ConsoleKey.Spacebar:
                    break;
                case ConsoleKey.Subtract:
                    break;
                case ConsoleKey.T:
                    break;
                case ConsoleKey.Tab:
                    break;
                case ConsoleKey.U:
                    break;
                case ConsoleKey.V:
                    break;
                case ConsoleKey.VolumeDown:
                    break;
                case ConsoleKey.VolumeMute:
                    break;
                case ConsoleKey.VolumeUp:
                    break;
                case ConsoleKey.W:
                    break;
                case ConsoleKey.X:
                    break;
                case ConsoleKey.Y:
                    break;
                case ConsoleKey.Z:
                    break;
                case ConsoleKey.Zoom:
                    break;

                #endregion

                default:
                    break;
            };
        }
    }
}
