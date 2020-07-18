using System;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;

namespace ImmediateAccess
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Logger.Info(Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title + ": v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(), ConsoleColor.Yellow);
            Logger.Info("Service: Starting...");
            if (args.Length != 0)
            {
                if (args.Contains("/debug"))
                {
                    ImmediateAccess.Start(args);
                    Console.Read();
                    ImmediateAccess.Stop().Wait();
                    return;
                }
            }
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Main()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}