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
        private static bool IsNetworkAvailable = false;
        public static async void Start(string[] Paremeters)
        {
            Logger.Info("Testing probe availability...");
            await TestNetwork.IsProbeAvailable();
            Logger.Info("Registering event listeners...");
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            Logger.Info("Service is ready.", ConsoleColor.Green);
        }
        private static async void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            Logger.Info("Network address has changed.");
            if (IsNetworkAvailable)
            {
                await TestNetwork.IsProbeAvailable();
            }
        }
        private static async void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            IsNetworkAvailable = e.IsAvailable;
            if (e.IsAvailable)
            {
                Logger.Info("Network is now available.", ConsoleColor.Green);
                await TestNetwork.IsProbeAvailable();
            }
            else
            {
                Logger.Warning("Network is no longer available.");
            }
        }
        public static void Stop()
        {

        }
    }
}
