using System;
using System.Diagnostics;
using System.Management;
using System.ServiceProcess;

namespace RasManRepair
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("RasManRepair: Searching for RasMan process ID...");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(new ManagementScope("root\\cimv2"), new SelectQuery("SELECT * FROM Win32_Service WHERE Name = 'RasMan'"));
            UInt32 processId = 0;
            foreach (ManagementObject service in searcher.Get())
            {
                processId = (UInt32)service.Properties["ProcessId"].Value;
            }
            Console.WriteLine("RasManRepair: Killing RasMan (" + processId + ")...");
            Process rasManProcess = Process.GetProcessById(int.Parse(processId.ToString()));
            rasManProcess.Kill();
            rasManProcess.WaitForExit(10000);
            Console.WriteLine("RasManRepair: Starting RasMan...");
            ServiceController rasManService = new ServiceController("RasMan");
            rasManService.Start();
            Console.WriteLine("RasManRepair: Done.");
        }
    }
}