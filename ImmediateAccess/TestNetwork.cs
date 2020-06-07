using System;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Management;
using System.Net.Sockets;
using System.Net.Security;
using System.Net;
using System.Collections.Generic;
using System.Threading;

namespace ImmediateAccess
{
    class TestNetwork
    {
        private static Ping PingProvider = new Ping();
        public static async Task<bool> IsProbeAvailable()
        {
            if (PolicyReader.Policies["InternalProbe"] == null) return false;
            return await TestProbeFrom(GetAllIPAddresses());
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
        private static async Task<bool> TestProbeFrom(IPAddress[] Bind)
        {
            Logger.Info("Probe: Testing probe from all adapters excluding VPN...", ConsoleColor.Blue);
            CancellationTokenSource cts = new CancellationTokenSource(); //MAX TIME SPENT CAN GO HERE!
            List<Task<bool>> tasks = new List<Task<bool>>();
            foreach(IPAddress ip in Bind)
            {
                tasks.Add(TestProbe(ip, cts.Token));
            }
            while (tasks.Count != 0)
            {
                Task<bool> task = await Task.WhenAny(tasks);
                if(await task)
                {
                    Logger.Info("Probe: Online!", ConsoleColor.Blue);
                    cts.Cancel();
                    return true;
                }
                else
                {
                    tasks.Remove(task);
                }
            }
            Logger.Info("Probe: Probe not available!", ConsoleColor.Blue);
            return false;
        }
        private static async Task<bool> TestProbe(IPAddress Bind = null, CancellationToken Cancellation = new CancellationToken())
        {
            return await HttpRequest(new Uri((string)PolicyReader.Policies["InternalProbe"]), Bind, Cancellation);
        }
        private static Task<bool> HttpRequest(Uri URI, IPAddress Bind = null, CancellationToken Cancellation = new CancellationToken())
        {
            if(Cancellation.IsCancellationRequested) return Task.FromResult(false);
            if (URI.Scheme != Uri.UriSchemeHttp && URI.Scheme != Uri.UriSchemeHttps) return Task.FromResult(false);
            return Task.Run(() => {
                string head = "Probe " + Task.CurrentId + ": ";
                try
                {
                    if (Cancellation.IsCancellationRequested) return false;
                    Logger.Info(head + "Testing probe on: " + Bind.ToString() + "...", ConsoleColor.DarkYellow);
                    Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    if (Bind != null)
                    {
                        EndPoint ep = new IPEndPoint(Bind, 0);
                        socket.Bind(ep);
                    }
                    socket.ConnectAsync(URI.Host, URI.Port).Wait((int)PolicyReader.Policies["ProbeTimeoutMS"]);
                    if(!socket.Connected)
                    {
                        return false;
                    }
                    if (Cancellation.IsCancellationRequested) return false;
                    Logger.Info(head + "Connected! Checking certificate...", ConsoleColor.DarkYellow);
                    NetworkStream ns = new NetworkStream(socket, true);
                    SslStream ss = new SslStream(ns, false);
                    ss.AuthenticateAsClient(URI.Host);
                    if (Cancellation.IsCancellationRequested) return false;
                    Logger.Info(head + "Success!", ConsoleColor.DarkYellow);
                    return true;
                }
                catch (Exception e)
                {
                    if (Cancellation.IsCancellationRequested) return false;
                    if (e.InnerException != null)
                    {
                        Logger.Error(head + e.InnerException.Message);
                    }
                    else
                    {
                        Logger.Error(head + e.Message);
                    }
                    return false;
                }
            });
        }
        private static async Task<bool> Ping(string Host)
        {
            int pingCount = 1; // (int)PolicyReader.Policies["ProbeAttempts"];
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
        private static IPAddress[] GetAllIPAddresses(bool IncludeGpoVpnProfiles = false)
        {
            List<IPAddress> ipList = new List<IPAddress>();
            foreach(NetworkInterface iface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if(!IncludeGpoVpnProfiles)
                {
                    List<string> profiles = new List<string>((string[])PolicyReader.Policies["VpnProfileList"]);
                    if (iface.NetworkInterfaceType == NetworkInterfaceType.Ppp && profiles.Contains(iface.Description)) break;
                }
                IPInterfaceProperties ipProps = iface.GetIPProperties();
                foreach(UnicastIPAddressInformation ip in ipProps.UnicastAddresses)
                {
                    ipList.Add(ip.Address);
                }
            }
            return ipList.ToArray();
        }
    }
}
