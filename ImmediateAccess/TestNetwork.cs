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
        private static Ping PingProvider = new Ping();
        public static async Task<bool> IsProbeAvailable()
        {
            return await Ping("ad.belowaverage.org");
        }
        public static async Task<bool> IsVpnServerAccessible()
        {
            return await Ping("ba-dz2.dz.belowaverage.org");
        }
        private static async Task<bool> Ping(string Host)
        {
            bool result = false;
            int pingCount = 5;
            int pingTimeout = 1000;
            int pingInterval = 500;
            while (pingCount-- > 0)
            {
                try
                {
                    Logger.Info("Pinging: " + Host + "...");
                    PingReply reply = await PingProvider.SendPingAsync(Host, pingTimeout);
                    if (reply.Status == IPStatus.Success)
                    {
                        result = true;
                        break;
                    }
                }
                catch (Exception)
                {
                    Logger.Warning("Probe failed to respond in a timely manner...");
                    result = false;
                }
                await Task.Delay(pingInterval);
            }
            if (result)
            {
                Logger.Info("\"" + Host + "\" is online.", ConsoleColor.Green);
            }
            else
            {
                Logger.Warning("\"" + Host + "\" is unavailable.");
            }
            return result;
        }
    }
}
