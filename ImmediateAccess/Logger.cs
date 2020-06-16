using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImmediateAccess
{
    class Logger
    {
        private static SemaphoreSlim ss = new SemaphoreSlim(1);
        public static void Info(string Message)
        {
            Info(Message, ConsoleColor.Gray);
        }
        public static void Warning(string Message)
        {
            Info(Message, ConsoleColor.Yellow);
        }
        public static void Error(string Message)
        {
            Info(Message, ConsoleColor.DarkRed);
        }
        public static void Info(string Message, ConsoleColor Color)
        {
            ss.Wait();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(DateTime.Now.ToString() + "> ");
            Console.ForegroundColor = Color;
            Console.WriteLine(Message);
            Console.ResetColor();
            ss.Release();
        }
    }
}
