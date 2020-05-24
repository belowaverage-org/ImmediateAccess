using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Management;

namespace ImmediateAccess
{
    class TestNetwork
    {
        private static Ping PingProvider = new Ping();
        public static async Task<bool> IsProbeAvailable()
        {
            if (PolicyReader.Policies["InternalProbe"] == null) return false;
            return await TestProbe((string)PolicyReader.Policies["InternalProbe"]);
        }
        public static async Task<bool> IsVpnServerAccessible()
        {
            foreach(string vpnProfile in (string[])PolicyReader.Policies["VpnProfileList"])
            {
                ManagementObject vpnStatus = await VpnStatus.Get(vpnProfile);
                if (await Ping((string)vpnStatus.GetPropertyValue("ServerAddress")))
                {
                    VpnControl.SelectedVPNProfile = vpnProfile;
                    return true;
                }
            }
            return false;
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
            HttpClient HttpProvider = new HttpClient();
            int pingCount = (int)PolicyReader.Policies["ProbeAttempts"];
            HttpProvider.Timeout = TimeSpan.FromMilliseconds((int)PolicyReader.Policies["ProbeTimeoutMS"]);
            while (pingCount-- > 0)
            {
                try
                {
                    Logger.Info("Probing: \"" + URI + "\"...");
                    HttpResponseMessage response = await HttpProvider.GetAsync(URI);
                    if (response.IsSuccessStatusCode)
                    {
                        Logger.Info("\"" + URI + "\" is online.", ConsoleColor.Green);
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                    if (e.InnerException != null) Logger.Error(e.InnerException.Message);
                }
                await Task.Delay((int)PolicyReader.Policies["ProbeIntervalMS"]);
            }
            HttpProvider.Dispose();
            return false;
        }
        private static async Task<bool> Ping(string Host)
        {
            int pingCount = (int)PolicyReader.Policies["ProbeAttempts"];
            while (pingCount-- > 0)
            {
                try
                {
                    Logger.Info("Pinging: \"" + Host + "\"...");
                    PingReply reply = await PingProvider.SendPingAsync(Host, (int)PolicyReader.Policies["ProbeTimeoutMS"]);
                    if (reply.Status == IPStatus.Success)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    Logger.Warning("Probe failed to respond in a timely manner...");
                }
                await Task.Delay((int)PolicyReader.Policies["ProbeIntervalMS"]);
            }
            return false;
        }
    }
}
