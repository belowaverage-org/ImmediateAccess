using System;
using System.Threading;

namespace ImmediateAccess
{
    class Logger
    {
        private static SemaphoreSlim ss = new SemaphoreSlim(1);
        /// <summary>
        /// This method will print an info line to the console.
        /// </summary>
        /// <param name="Message">String: The message to print.</param>
        public static void Info(string Message)
        {
            Info(Message, ConsoleColor.Gray);
        }
        /// <summary>
        /// This method will print a warning line to the console.
        /// </summary>
        /// <param name="Message">String: The warning to print.</param>
        public static void Warning(string Message)
        {
            Info(Message, ConsoleColor.Yellow);
        }
        /// <summary>
        /// This method will print an error line to the console.
        /// </summary>
        /// <param name="Message">String: The error to print.</param>
        public static void Error(string Message)
        {
            Info(Message, ConsoleColor.DarkRed);
        }
        /// <summary>
        /// This method will print an info line to the console.
        /// </summary>
        /// <param name="Message">String: The message to print, ConsoleColor: The color the print the line in.</param>
        public static void Info(string Message, ConsoleColor Color)
        {
            if (!ImmediateAccess.IsDebugMode) return;
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