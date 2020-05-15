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
        private static bool IsCurrentlyEnsuring = false;
        private static bool IsNetworkAvailable = false;
        public static async Task Start(string[] Paremeters)
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
            if (IsCurrentlyEnsuring || !IsNetworkAvailable) return;
            IsCurrentlyEnsuring = true;
            bool success = false;
            Logger.Info("Ensuring a connection to the intranet...");
            await VpnControl.Disconnect();
            success = await TestNetwork.IsProbeAvailable();
            if (success)
            {
                IsCurrentlyEnsuring = false;
                return;
            }
            success = await TestNetwork.IsVpnServerAccessible();
            if (!success)
            {
                IsCurrentlyEnsuring = false;
                return;
            }
            Logger.Warning("Intranet is not available! Mitigating...");
            while (true)
            {
                if(await VpnControl.Connect())
                {
                    break;
                }
                Logger.Warning("Couldn't connect to VPN for some reason. Trying again in 5 seconds...");
                await Task.Delay(5000);
            }
            Logger.Info("Waiting 5 seconds for cooldown...");
            await Task.Delay(5000);
            Logger.Info("Done.");
            IsCurrentlyEnsuring = false;
        }
        private static async void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            Logger.Info("Network change detected: IP Address.");
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
                /*
                if (VpnControl.RasDialProcess != null)
                {
                    Logger.Warning("RasDial should not be running, Killing...");
                    VpnControl.RasDialProcess.Kill();
                    Logger.Info("Killed.");
                }
                */
            }
        }
        public static async Task Stop()
        {
            //await VpnControl.Disconnect();
        }
    }
}