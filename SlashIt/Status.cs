using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{

    public class Status
    {
        //TODO: maybe change this to be inited on prog start so that it can vary more easily 
        private static string clearLine = "                                                                               ";

        private const int MessagePositionDefaultLeft = 0;
        private const int MessagePositionDefaultTop = 24;

        private const int InfoPositionDefaultLeft = 0;
        private const int InfoPositionDefaultTop = 0;

        public static string Message { get; set; }
        public static string Info { get; set; }

        public static int MessagePositionLeft { get; set; }
        public static int MessagePositionTop { get; set; }

        public static int InfoPositionLeft { get; set; }
        public static int InfoPositionTop { get; set; }

        public Status()
        {
            MessagePositionLeft = Status.MessagePositionDefaultLeft;
            MessagePositionTop = Status.MessagePositionDefaultTop;

            InfoPositionLeft = Status.InfoPositionDefaultLeft;
            InfoPositionTop = Status.InfoPositionDefaultTop;
        }

        public static void WriteToStatus()
        {
            var originalCursonPositionLeft = Console.CursorLeft;
            var originalCursonPositionTop = Console.CursorTop;

            Console.SetCursorPosition(InfoPositionDefaultLeft, InfoPositionDefaultTop);
            Console.Write(clearLine);
            Console.SetCursorPosition(InfoPositionDefaultLeft, InfoPositionDefaultTop);
            Console.Write(Info);

            Console.SetCursorPosition(MessagePositionDefaultLeft, MessagePositionDefaultTop);
            Console.Write(clearLine);
            Console.SetCursorPosition(MessagePositionDefaultLeft, MessagePositionDefaultTop);
            Console.Write(Message);

            Console.SetCursorPosition(originalCursonPositionLeft, originalCursonPositionTop);
        }

        public static void WriteToStatusLine(string status)
        {
            Message = status;
            WriteToStatus();
        }


        public static void ClearInfo()
        {
            var originalCursonPositionLeft = Console.CursorLeft;
            var originalCursonPositionTop = Console.CursorTop;

            Console.SetCursorPosition(InfoPositionDefaultLeft, InfoPositionDefaultTop);
            Console.Write(clearLine);
            Info = "";

            Console.SetCursorPosition(originalCursonPositionLeft, originalCursonPositionTop);
        }
    }
}
