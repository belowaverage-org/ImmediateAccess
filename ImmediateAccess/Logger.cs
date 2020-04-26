using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmediateAccess
{
    class Logger
    {
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
            Console.Write(DateTime.Now.ToString() + "> ");
            Console.ForegroundColor = Color;
            Console.WriteLine(Message);
            Console.ResetColor();
        }
    }
}
