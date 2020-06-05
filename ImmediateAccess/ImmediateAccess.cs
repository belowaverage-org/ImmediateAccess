using System;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace ImmediateAccess
{
    class ImmediateAccess
    {
        private static bool IsCurrentlyEnsuring = false;
        private static bool IsNetworkAvailable = false;
        public static async Task Start(string[] Paremeters)
        {
            PolicyReader.ReadPolicies();
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
            if (await TestNetwork.IsProbeAvailable())
            {
                IsCurrentlyEnsuring = false;
                await VpnControl.Disconnect();
                return;
            }
            if (await VpnControl.IsConnected() != null) return;
            while (true)
            {
                if (!await TestNetwork.IsVpnServerAccessible())
                {
                    IsCurrentlyEnsuring = false;
                    return;
                }
                if (await VpnControl.Connect())
                {
                    IsCurrentlyEnsuring = false;
                    _ = EnsureConnectionToIntranet();
                    return;
                }
                Logger.Warning("Couldn't connect to VPN for some reason. Trying again in 5 seconds...");
                await Task.Delay(5000);
            }
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