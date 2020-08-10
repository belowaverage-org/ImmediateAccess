using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImmediateAccessNConsole
{
    class Program
    {
        private static TcpClient nConsole;
        private static StreamReader nConsoleReader;
        static void Main(string[] args)
        {
            SetupNConsole();
            Console.ReadLine();
        }
        private static void SetupNConsole()
        {
            Task.Run(() => {
                try
                {
                    nConsole = new TcpClient("127.0.0.1", 7362);
                    nConsoleReader = new StreamReader(nConsole.GetStream());
                    _ = mConsoleReadLoop();
                }
                catch (Exception)
                {
                    
                }
            });
        }
        private static Task mConsoleReadLoop()
        {
            return Task.Run(() => {
                while (true)
                {
                    try
                    {
                        UpdateLogRTF(nConsoleReader.ReadLine() + "\r\n");
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            });
        }
        private static void UpdateLogRTF(string log)
        {
            ConsoleColor color = ConsoleColor.White;
            string[] logParts = log.Split(new string[] { "<", ">" }, StringSplitOptions.None);
            foreach (string logPart in logParts)
            {
                if (ConsoleColorConversion.ContainsKey(logPart))
                {
                    color = ConsoleColorConversion[logPart];
                }
                else
                {
                    UpdateLogColor(logPart, color);
                }
            }
        }
        private static void UpdateLogColor(string log, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(log);
        }
        private static Dictionary<string, ConsoleColor> ConsoleColorConversion = new Dictionary<string, ConsoleColor>()
        {
            { "#Black#", ConsoleColor.Black },
            { "#Blue#", ConsoleColor.Blue },
            { "#Cyan#", ConsoleColor.Cyan },
            { "#DarkBlue#", ConsoleColor.DarkBlue },
            { "#DarkCyan#", ConsoleColor.DarkCyan },
            { "#DarkGray#", ConsoleColor.DarkGray },
            { "#DarkGreen#", ConsoleColor.DarkGreen },
            { "#DarkMagenta#", ConsoleColor.DarkMagenta },
            { "#DarkRed#", ConsoleColor.DarkRed },
            { "#DarkYellow#", ConsoleColor.DarkYellow },
            { "#Gray#", ConsoleColor.Gray },
            { "#Green#", ConsoleColor.Green },
            { "#Magenta#", ConsoleColor.Magenta },
            { "#Red#", ConsoleColor.Red },
            { "#White#", ConsoleColor.White },
            { "#Yellow#", ConsoleColor.Yellow }
        };
    }
}
