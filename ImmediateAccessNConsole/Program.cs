using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ImmediateAccessNConsole
{
    class Program
    {
        private static TcpClient nConsole;
        private static StreamReader nConsoleReader;
        private static ConsoleColor color;
        static void Main(string[] args)
        {
            if (args.Length == 2 && args[0] == "WatchPID")
            {
                int pid;
                if (int.TryParse(args[1], out pid))
                {
                    Task.Run(() =>
                    {
                        Process watch = Process.GetProcessById(pid);
                        watch.EnableRaisingEvents = true;
                        watch.WaitForExit();
                        Process.GetCurrentProcess().Kill();
                    });
                }
            }
            Console.WriteLine(Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title + ": v" + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            SetupNConsole();
            Thread.Sleep(-1);
        }
        private static void SetupNConsole()
        {
            Console.WriteLine("Connecting to the Immediate Access service...");
            Task.Run(() => {
                try
                {
                    nConsole = new TcpClient("127.0.0.1", 7362);
                    nConsoleReader = new StreamReader(nConsole.GetStream());
                    _ = mConsoleReadLoop();
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.Write(
                        "Failed to connect to the Immediate Access service's console.\r\n\r\n" +
                        "Check that the Immediate Access service is actually running.\r\n\r\n" +
                        "Retrying in 5 seconds"
                    );
                    WaitOutLoud(5);
                    Console.Clear();
                    SetupNConsole();
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
                        Console.Clear();
                        Console.Write(
                           "Connection to Immediate Access service lost!\r\n\r\n" +
                           "Attempting to re-connect in 5 seconds"
                       );
                        WaitOutLoud(5);
                        Console.Clear();
                        SetupNConsole();
                        return;
                    }
                }
            });
        }
        private static void UpdateLogRTF(string log)
        {
            string[] logParts = log.Split(new string[] { "<#", "#>" }, StringSplitOptions.None);
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
        private static void WaitOutLoud(int seconds)
        {
            for(int count = 0; count < seconds; count++)
            {
                Thread.Sleep(1000);
                Console.Write(".");
            }
            Console.Write("\r\n");
        }
        private static Dictionary<string, ConsoleColor> ConsoleColorConversion = new Dictionary<string, ConsoleColor>()
        {
            { "Black", ConsoleColor.Black },
            { "Blue", ConsoleColor.Blue },
            { "Cyan", ConsoleColor.Cyan },
            { "DarkBlue", ConsoleColor.DarkBlue },
            { "DarkCyan", ConsoleColor.DarkCyan },
            { "DarkGray", ConsoleColor.DarkGray },
            { "DarkGreen", ConsoleColor.DarkGreen },
            { "DarkMagenta", ConsoleColor.DarkMagenta },
            { "DarkRed", ConsoleColor.DarkRed },
            { "DarkYellow", ConsoleColor.DarkYellow },
            { "Gray", ConsoleColor.Gray },
            { "Green", ConsoleColor.Green },
            { "Magenta", ConsoleColor.Magenta },
            { "Red", ConsoleColor.Red },
            { "White", ConsoleColor.White },
            { "Yellow", ConsoleColor.Yellow }
        };
    }
}
