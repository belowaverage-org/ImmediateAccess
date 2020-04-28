using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Net.NetworkInformation;

namespace ImmediateAccess
{
    class ImmediateAccess
    {
        public static bool CaptureEvents = true;
        private static bool IsNetworkAvailable = false;
        public static async void Start(string[] Paremeters)
        {
            Logger.Info("Observing network state...");
            IsNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();
            await EnsureConnectionToIntranet();
            Logger.Info("Registering event listeners...");
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            Logger.Info("Service is ready.", ConsoleColor.Green);
        }
        private static async Task EnsureConnectionToIntranet()
        {
            Logger.Info("Ensuring a connection to the intranet.");
            if (!IsNetworkAvailable) 
            {
                Logger.Warning("No network available.");
                return;
            }
            Logger.Info("Disabling network events...");
            CaptureEvents = false;
            Logger.Info("Disconnecting to test probe...");
            await VpnControl.Disconnect();
            Logger.Info("Pinging intranet probe...");
            bool success = await TestNetwork.IsProbeAvailable();
            if (success)
            {
                Logger.Info("Already connected to intranet!", ConsoleColor.Green);
                CaptureEvents = true;
                Logger.Info("Enabling network events...");
                return;
            }
            Logger.Warning("Intranet is not available! Mitigating...");
            await VpnControl.Connect();
            Logger.Info("Waiting 5 seconds for cooldown...");
            await Task.Delay(5000);
            Logger.Info("Enabling network events...");
            CaptureEvents = true;
        }
        private static async void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            Logger.Info("Network change detected: IP Address.");
            if (!CaptureEvents) return;
            await EnsureConnectionToIntranet();
        }
        private static void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            IsNetworkAvailable = e.IsAvailable;
            if (e.IsAvailable)
            {
                Logger.Info("Network is now available.", ConsoleColor.Green);
            }
            else
            {
                Logger.Warning("Network is no longer available.");
                if (VpnControl.RasDialProcess != null)
                {
                    Logger.Warning("RasDial should not be running, Killing...");
                    VpnControl.RasDialProcess.Kill();
                    Logger.Info("Killed.");
                }
            }
        }
        public static void Stop()
        {

        }
    }
}