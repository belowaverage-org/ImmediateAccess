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
        public static async Task<bool> IsProbeAvailable()
        {
            if (PolicyReader.Policies["InternalProbe"] == null) return false;
            return await TestProbeFrom(GetAllIPAddresses());
        }
        public static async Task<bool> SelectOnlineVpnProfile()
        {
            Logger.Info("Selecting optimal VPN profile...");
            foreach (string vpnProfile in (string[])PolicyReader.Policies["VpnProfileList"])
            {
                ManagementObject vpnStatus = await VpnStatus.Get(vpnProfile);
                Task<bool> ping = VpnServerPing((string)vpnStatus.GetPropertyValue("ServerAddress"));
                bool finished = ping.Wait((int)PolicyReader.Policies["VpnServerPingTimeoutMS"]);
                if (finished && await ping)
                {
                    Logger.Info("Selected: " + vpnProfile + ".");
                    VpnControl.SelectedVPNProfile = vpnProfile;
                    return true;
                }
            }
            return false;
        }
        private static async Task<bool> TestProbeFrom(IPAddress[] Bind)
        {
            Logger.Info("Probe: Testing probe from all adapters excluding VPN...", ConsoleColor.Blue);
            CancellationTokenSource cts = new CancellationTokenSource();
            List<Task<bool>> tasks = new List<Task<bool>>();
            foreach (IPAddress ip in Bind)
            {
                tasks.Add(TestProbe(ip, cts.Token));
            }
            while (tasks.Count != 0)
            {
                Task<bool> task = await Task.WhenAny(tasks);
                if (await task)
                {
                    Logger.Info("Probe: Success.", ConsoleColor.Blue);
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
            if (Cancellation.IsCancellationRequested) return Task.FromResult(false);
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
                    socket.ConnectAsync(URI.Host, URI.Port).Wait((int)PolicyReader.Policies["ProbeTimeoutS"] * 1000);
                    if (!socket.Connected)
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
        private static async Task<bool> VpnServerPing(string Host)
        {
            try
            {
                Logger.Info("Ping: \"" + Host + "\"...");
                Ping ping = new Ping();
                PingReply reply = await ping.SendPingAsync(Host, (int)PolicyReader.Policies["VpnServerPingTimeoutMS"]);
                if (reply.Status == IPStatus.Success)
                {
                    Logger.Info("Ping: Success.", ConsoleColor.Green);
                    return true;
                }
                Logger.Warning("Ping: Timeout.");
                return false;
            }
            catch (Exception e)
            {
                Logger.Warning("Ping: " + e.Message);
            }
            return false;
        }
        private static IPAddress[] GetAllIPAddresses(bool IncludeGpoVpnProfiles = false)
        {
            List<IPAddress> ipList = new List<IPAddress>();
            foreach (NetworkInterface iface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (!IncludeGpoVpnProfiles)
                {
                    List<string> profiles = new List<string>((string[])PolicyReader.Policies["VpnProfileList"]);
                    if (iface.NetworkInterfaceType == NetworkInterfaceType.Ppp && profiles.Contains(iface.Description)) break;
                }
                IPInterfaceProperties ipProps = iface.GetIPProperties();
                foreach (UnicastIPAddressInformation ip in ipProps.UnicastAddresses)
                {
                    ipList.Add(ip.Address);
                }
            }
            return ipList.ToArray();
        }
    }
}