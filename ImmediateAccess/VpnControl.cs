using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;

namespace ImmediateAccess
{
    class VpnControl
    {
        public static RasDialProcess RasDialProcess = null;
        //private static ServiceController RasMan = new ServiceController("RasMan");
        private static string RasDialExe = "rasdial.exe";
        private static string VpnProfileString = "NorwalkOH - VPN";
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
            Logger.Info("RasDial: Disconnecting from VPN...", ConsoleColor.DarkCyan);
            await RasDial("\"" + VpnProfileString + "\" /disconnect");
            await Task.Delay(1000);
            return !await IsConnected();
        }
        public static async Task<bool> IsConnected()
        {
            Logger.Info("Checking if connected to VPN...");
            ManagementObject mo = await VpnStatus.Get(VpnProfileString);
            string status = (string)mo.GetPropertyValue("ConnectionStatus");
            if(status == "Connected")
            {
                Logger.Info("Connected to VPN.");
                return true;
            }
            return false;
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
            if (e.Data == "Command completed successfully.")
            {
                process.SuccessFromRasDial = true;
                return;
            }
            Logger.Info("RasDial: " + e.Data, ConsoleColor.DarkCyan);
        }
    }
    class VpnStatus
    {
        public static Task<ManagementObject> Get(string VpnProfileName)
        {
            return Task.Run(() => {
                try
                {
                    Logger.Info("Gathering VPN Profile Information...");
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM PS_VpnConnection");
                    searcher.Scope.Path.ClassName = "PS_VpnConnection";
                    searcher.Scope.Path.NamespacePath = @"Root\Microsoft\Windows\RemoteAccess\Client";
                    ManagementObjectCollection profiles = searcher.Get();
                    ManagementObject vpnProfile = null;
                    foreach (ManagementObject profile in profiles)
                    {
                        if ((string)profile.GetPropertyValue("Name") == VpnProfileName)
                        {
                            vpnProfile = profile;
                            break;
                        }
                    }
                    return vpnProfile;
                }
                catch (Exception)
                {
                    Logger.Error("Error reading VPN Profile via WMI!");
                    return null;
                }
            });
        }
    }
    class RasDialProcess : Process
    {
        public bool SuccessFromRasDial = false;
    }
}
