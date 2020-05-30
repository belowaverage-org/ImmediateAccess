using System;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Management;
using System.Net.Sockets;
using System.Net.Security;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace ImmediateAccess
{
    class TestNetwork
    {
        private static Ping PingProvider = new Ping();
        public static async Task<bool> IsProbeAvailable()
        {
            if (PolicyReader.Policies["InternalProbe"] == null) return false;
            return await TestProbe();
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
        private static async Task<bool> TestProbe(bool AllowVPNTraversal = false)
        {
            if (AllowVPNTraversal)
            {
                return await HttpRequest(new Uri((string)PolicyReader.Policies["InternalProbe"]));
            }
            else
            {
                List<Task<bool>> tasks = new List<Task<bool>>();
                foreach(IPAddress ip in GetAllIPAddresses(false))
                {
                    tasks.Add(HttpRequest(new Uri((string)PolicyReader.Policies["InternalProbe"]), ip));
                }
                await Task.WhenAll(tasks.ToArray());
            }
            return true;
        }
        private static Task<bool> HttpRequest(Uri URI, IPAddress Bind = null)
        {
            if (URI.Scheme != Uri.UriSchemeHttp && URI.Scheme != Uri.UriSchemeHttps) return Task.FromResult(false);
            return Task.Run(() => {
                string head = "Probe " + Task.CurrentId + ": ";
                try
                {
                    Logger.Info(head + "Testing probe...", ConsoleColor.DarkYellow);
                    Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    if (Bind != null)
                    {
                        EndPoint ep = new IPEndPoint(Bind, 0);
                        socket.Bind(ep);
                    }
                    socket.Connect(URI.Host, URI.Port);
                    Logger.Info(head + "Connected! Negotiating...", ConsoleColor.DarkYellow);
                    Stream stream;
                    if (URI.Scheme == Uri.UriSchemeHttp)
                    {
                        Logger.Info(head + "Using HTTP.", ConsoleColor.DarkYellow);
                        stream = new NetworkStream(socket, true);
                    }
                    else
                    {
                        Logger.Info(head + "Using HTTPS.", ConsoleColor.DarkYellow);
                        NetworkStream ns = new NetworkStream(socket, true);
                        stream = new SslStream(ns, false);
                        ((SslStream)stream).AuthenticateAsClient(URI.Host);
                    }
                    Logger.Info(head + "Sending request...", ConsoleColor.DarkYellow);
                    StreamReader sr = new StreamReader(stream);
                    StreamWriter sw = new StreamWriter(stream);
                    sw.Write("GET / HTTP/1.1\r\nHost: " + URI.Host + "\r\nConnection: Close\r\n\r\n");
                    sw.Flush();
                    Logger.Info(head + "Waiting for response...", ConsoleColor.DarkYellow);
                    string response = sr.ReadToEnd();
                    foreach (string line in response.Split('\n'))
                    {
                        Logger.Info(head + " - " + line, ConsoleColor.DarkYellow);
                    }
                    Logger.Info(head + "Success!", ConsoleColor.DarkYellow);
                    return true;
                }
                catch (Exception e)
                {
                    Logger.Error(head + e.Message);
                    return false;
                }
            });
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
