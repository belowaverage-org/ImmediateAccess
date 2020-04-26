using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ImmediateAccess
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                if (args.Contains("/debug"))
                {
                    //Main main = new Main();
                    Console.ReadLine();
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