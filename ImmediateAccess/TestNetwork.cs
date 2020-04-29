using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace ImmediateAccess
{
    class TestNetwork
    {
        private static Ping Ping = new Ping();
        public static async Task<bool> IsProbeAvailable()
        {
            bool result = false;
            int pingTimeout = 1000;
            string probeAddress = "ad.belowaverage.org";
            try
            {
                Logger.Info("Pinging probe...");
                PingReply reply = await Ping.SendPingAsync(probeAddress, pingTimeout);

                if (reply.Status == IPStatus.Success) result = true;
            }
            catch (Exception)
            {
                Logger.Warning("Probe failed to respond in a timely manner...");
                result = false;
            }
            if (result)
            {
                Logger.Info("\"" + probeAddress + "\" is online.", ConsoleColor.Green);
            }
            else
            {
                Logger.Warning("\"" + probeAddress + "\" is unavailable.");
            }
            return result;
        }
        public static async Task<bool> IsVpnServerAccessible()
        {
            bool result = false;
            int pingTimeout = 1000;
            string probeAddress = "ba-dz2.dz.belowaverage.org";
            try
            {
                Logger.Info("Pinging VPN server...");
                PingReply reply = await Ping.SendPingAsync(probeAddress, pingTimeout);

                if (reply.Status == IPStatus.Success) result = true;
            }
            catch (Exception)
            {
                Logger.Warning("VPN server failed to respond in a timely manner...");
                result = false;
            }
            if (result)
            {
                Logger.Info("\"" + probeAddress + "\" is online.", ConsoleColor.Green);
            }
            else
            {
                Logger.Warning("\"" + probeAddress + "\" is unavailable.");
            }
            return result;
        }
    }
}
