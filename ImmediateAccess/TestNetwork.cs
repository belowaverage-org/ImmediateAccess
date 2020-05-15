using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;

namespace ImmediateAccess
{
    class TestNetwork
    {
        private static HttpClient HttpProvider = new HttpClient();
        private static Ping PingProvider = new Ping();
        private static int pingCount = 5;
        private static int pingTimeout = 1000;
        private static int pingInterval = 500;
        public static async Task<bool> IsProbeAvailable()
        {
            return await TestProbe("https://ad.belowaverage.org");
        }
        public static async Task<bool> IsVpnServerAccessible()
        {
            return await Ping("ba-dz2.dz.belowaverage.org");
        }
        private static async Task<bool> TestProbe(string HostOrURI)
        {
            if(Uri.TryCreate(HostOrURI, UriKind.Absolute, out Uri ProbeURI))
            {
                return await HttpsRequest(HostOrURI);
            }
            else
            {
                return await Ping(HostOrURI);
            }
        }
        private static async Task<bool> HttpsRequest(string URI)
        {
            Logger.Info("Probing: \"" + URI + "\"...");
            try
            {
                HttpResponseMessage response = await HttpProvider.GetAsync(URI);
                if(response.IsSuccessStatusCode)
                {
                    Logger.Info("\"" + URI + "\" is online.", ConsoleColor.Green);
                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                if(e.InnerException != null) Logger.Error(e.InnerException.Message);
                return false;
            }
            return false;
        }
        private static async Task<bool> Ping(string Host)
        {
            bool result = false;
            while (pingCount-- > 0)
            {
                try
                {
                    Logger.Info("Pinging: \"" + Host + "\"...");
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
