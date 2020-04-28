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
        private static bool AlreadyTestingProbe = false;
        private static Ping Ping = new Ping();
        public static async Task<bool> IsProbeAvailable() {
            bool result = false;
            if (AlreadyTestingProbe) return false;
            AlreadyTestingProbe = true;
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
            AlreadyTestingProbe = false;
            return result;
        }
    }
}
