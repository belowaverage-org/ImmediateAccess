using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;

namespace ImmediateAccess
{
    class VpnControl
    {
        public static RasDialProcess RasDialProcess = null;
        private static string RasDialExe = "rasdial.exe";
        public static string SelectedVPNProfile = "";
        public static async Task<bool> Connect()
        {
            if (await IsConnected() != null) return true;
            Logger.Info("RasDial: Attempting to connect...", ConsoleColor.DarkCyan);
            await RasDial("\"" + SelectedVPNProfile + "\"");
            await Task.Delay(1000);
            if(await IsConnected() != null) return true;
            foreach (string vpnProfile in (string[])PolicyReader.Policies["VpnProfileList"])
            {
                SelectedVPNProfile = vpnProfile;
                await RasDial("\"" + SelectedVPNProfile + "\"");
                await Task.Delay(1000);
                if (await IsConnected() != null) return true;
            }
            return false;
        }
        public static async Task<bool> Disconnect()
        {
            while (true)
            {
                string vpnProfile = await IsConnected();
                if (vpnProfile == null) return true;
                Logger.Info("RasDial: Disconnecting from VPN...", ConsoleColor.DarkCyan);
                await RasDial("\"" + vpnProfile + "\" /disconnect");
                await Task.Delay(1000);
            }
        }
        public static async Task<string> IsConnected()
        {
            Logger.Info("Checking if connected to VPN...");
            foreach (string vpnProfile in (string[])PolicyReader.Policies["VpnProfileList"])
            {
                ManagementObject mo = await VpnStatus.Get(vpnProfile);
                string status = (string)mo.GetPropertyValue("ConnectionStatus");
                if (status == "Connected")
                {
                    Logger.Info("Connected to VPN.");
                    return vpnProfile;
                }
            }
            return null;
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