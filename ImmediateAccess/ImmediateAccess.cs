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
        public static void Start(string[] Paremeters)
        {
            Logger.Info("Initializing...");
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
        }

        private static void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            Logger.Info("Network address has changed.");
        }

        private static void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                Logger.Info("Network is now available.", ConsoleColor.Green);
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
