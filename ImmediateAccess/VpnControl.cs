using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;
using System.Threading;

namespace ImmediateAccess
{
    class VpnControl
    {
        public static RasDialProcess RasDialProcess = null;
        private static string RasDialExe = "rasdial.exe";
        public static string SelectedVPNProfile = "";
        /// <summary>
        /// This method connects to the selected VPN profile, if there is a failure, it will connect to any managed VPN profile
        /// in GPO defined order.
        /// </summary>
        /// <param name="CancellationToken">CancellationToken: Used to cancel this method if desired.</param>
        /// <returns>Task: Bool: Returns true if connected, and false if everything fails.</returns>
        public static async Task<bool> Connect(CancellationToken CancellationToken)
        {
            if (await IsConnected() != null) return true;
            Logger.Info("RasDial: Attempting to connect...", ConsoleColor.DarkCyan);
            await RasDial("\"" + SelectedVPNProfile + "\"");
            CancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(100);
            if (await IsConnected() != null) return true;
            foreach (string vpnProfile in (string[])PolicyReader.Policies["VpnProfileList"])
            {
                CancellationToken.ThrowIfCancellationRequested();
                SelectedVPNProfile = vpnProfile;
                await RasDial("\"" + SelectedVPNProfile + "\"");
                await Task.Delay(100);
                if (await IsConnected() != null) return true;
            }
            return false;
        }
        /// <summary>
        /// This method disconnects any managed VPN profile if one is currently connnected.
        /// </summary>
        /// <returns>Task: Bool: Always returns true.</returns>
        public static async Task<bool> Disconnect()
        {
            while (true)
            {
                string vpnProfile = await IsConnected();
                if (vpnProfile == null) return true;
                Logger.Info("RasDial: Disconnecting from VPN...", ConsoleColor.DarkCyan);
                await RasDial("\"" + vpnProfile + "\" /disconnect");
                await Task.Delay(100);
            }
        }
        /// <summary>
        /// This method checks if any managed VPN profile is connected.
        /// </summary>
        /// <returns>Task: String: Returns the VPN profile name if it is connected, otherwise null.</returns>
        public static async Task<string> IsConnected()
        {
            Logger.Info("Checking if connected to VPN already...");
            foreach (string vpnProfile in (string[])PolicyReader.Policies["VpnProfileList"])
            {
                ManagementObject mo = await VpnStatus.Get(vpnProfile);
                string status = (string)mo.GetPropertyValue("ConnectionStatus");
                if (status == "Connected")
                {
                    Logger.Info("Already connected to VPN.");
                    return vpnProfile;
                }
            }
            Logger.Info("Not connected to VPN.");
            return null;
        }
        /// <summary>
        /// This method runs the RasDial executable with the specified arguments.
        /// </summary>
        /// <param name="Arguments">String: a string of arguments to pass to RasDial.exe</param>
        /// <returns>Task: RasDialProcess: Returns the stopped / resulting RasDial process.</returns>
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
        /// <summary>
        /// This event fires whenever the RasDial process outputs data from the main console.
        /// </summary>
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
        /// <summary>
        /// This method returns a management object of the VPN client.
        /// </summary>
        /// <param name="VpnProfileName">String: The VPN profile to select in the query.</param>
        /// <returns>Task: ManagementObject: The MO of the VPN profile.</returns>
        public static Task<ManagementObject> Get(string VpnProfileName)
        {
            return Task.Run(() => {
                try
                {
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
                catch (Exception e)
                {
                    Logger.Error("Error reading VPN Profile via WMI: " + e.Message);
                    return null;
                }
            });
        }
    }
    /// <summary>
    /// An extension of the Process class to add an additional property.
    /// </summary>
    class RasDialProcess : Process
    {
        public bool SuccessFromRasDial = false;
    }
}