using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.Common;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Management;
using System.ServiceProcess;
using System.IO;

namespace ImmediateAccess
{
    class VpnControl
    {
        public static RasDialProcess RasDialProcess = null;
        //private static ServiceController RasMan = new ServiceController("RasMan");
        private static string RasDialExe = "rasdial.exe";
        private static string VpnProfileString = "Below Average AD - VPN";
        public static async Task<bool> Connect()
        {
            if (await IsConnected()) return true;
            Logger.Info("RasDial: Attempting to connect...", ConsoleColor.DarkCyan);
            await RasDial("\"" + VpnProfileString + "\"");
            await Task.Delay(1000);
            return await IsConnected();
        }
        public static async Task<bool> Disconnect()
        {
            if (!await IsConnected()) return true;
            Logger.Info("RasDial: Disconnecting from " + VpnProfileString + "...", ConsoleColor.DarkCyan);
            await RasDial("\"" + VpnProfileString + "\" /disconnect");
            await Task.Delay(1000);
            return !await IsConnected();
        }
        public static async Task<bool> IsConnected()
        {
            Logger.Info("RasDial: Checking if connected to VPN.", ConsoleColor.DarkCyan);
            RasDialProcess rasDial = await RasDial();
            return rasDial.VpnIsConnected;
        }
        private static Task<RasDialProcess> RasDial(string Arguments = "")
        {
            return Task.Run(() => {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = RasDialExe;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.Arguments = Arguments;
                RasDialProcess = new RasDialProcess();
                RasDialProcess.StartInfo = startInfo;
                RasDialProcess.EnableRaisingEvents = true;
                RasDialProcess.OutputDataReceived += RasDialProcess_OutputDataReceived;
                RasDialProcess.Start();
                RasDialProcess.BeginOutputReadLine();
                RasDialProcess.WaitForExit(10000);
                if (!RasDialProcess.HasExited)
                {
                    RasDialProcess.Kill();
                }
                if (RasDialProcess.SuccessFromRasDial)
                {
                    Logger.Info("RasDial: Success.", ConsoleColor.Green);
                }
                else
                {
                    Logger.Error("RasDial: Failure.");
                }
                return RasDialProcess;
            });
        }
        private static void RasDialProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            RasDialProcess process = (RasDialProcess)sender;
            if (e.Data == null) return;
            if (e.Data == VpnProfileString && process.StartInfo.Arguments == "")
            {
                process.VpnIsConnected = true;
            }
            if (e.Data == "Command completed successfully.")
            {
                process.SuccessFromRasDial = true;
                return;
            }
            Logger.Info("RasDial: " + e.Data, ConsoleColor.DarkCyan);
        }
    }
    class RasDialProcess : Process
    {
        public bool SuccessFromRasDial = false;
        public bool VpnIsConnected = false;
    }
}
