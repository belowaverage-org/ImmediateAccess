using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.ServiceProcess;

namespace ImmediateAccess
{
    class Installer
    {
        public static void Install()
        {
            Logger.Info("Copying service registry template to disk...");
            File.WriteAllText("service.reg", Properties.Resources.service);
            string binPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Logger.Info("Registering service...");
            Process.Start("sc.exe", "create ImmediateAccess binPath=\"" + binPath + "\" start=auto DisplayName=\"Immediate Access\"").WaitForExit();
            Logger.Info("Merging registry...");
            Process.Start("regedit.exe", "/S service.reg").WaitForExit();
            Logger.Info("Cleanup...");
            File.Delete("service.reg");
            Logger.Info("Starting service...");
            Process.Start("sc.exe", "start ImmediateAccess").WaitForExit();
            Logger.Info("Done!");
        }
        public static void Uninstall()
        {
            Logger.Info("Stopping service...");
            Process.Start("sc.exe", "stop ImmediateAccess").WaitForExit();
            Logger.Info("Removing service...");
            Process.Start("sc.exe", "delete ImmediateAccess").WaitForExit();
            Logger.Info("Done!");
        }
    }
}
