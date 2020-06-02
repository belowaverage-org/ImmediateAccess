using System;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Management;
using System.Net.Sockets;
using System.Net.Security;
using System.Net;
using System.IO;
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
            return await TestProbeExludeVPN();
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
        private static async Task<bool> TestProbeExludeVPN()
        {
            Logger.Info("Probe: Testing probe from all adapters excluding VPN...", ConsoleColor.Blue);
            CancellationTokenSource cts = new CancellationTokenSource();
            List<Task<bool>> taskList = new List<Task<bool>>();
            foreach(IPAddress ip in GetAllIPAddresses(false))
            {
                taskList.Add(TestProbe(ip, cts.Token));
            }
            Task<bool>[] tasks = taskList.ToArray();
            while (true)
            {
                await Task.WhenAny(tasks);
                bool allComplete = true;
                foreach (Task<bool> task in tasks)
                {
                    if(task.IsCompleted && task.Result)
                    {
                        Logger.Info("Probe: Online!", ConsoleColor.Blue);
                        cts.Cancel();
                        return true;
                    }
                    if(!task.IsCompleted)
                    {
                        allComplete = false;
                        continue;
                    }
                }
                if (allComplete) break;
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
                    socket.Connect(URI.Host, URI.Port);
                    if (Cancellation.IsCancellationRequested) return false;
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
                    if (Cancellation.IsCancellationRequested) return false;
                    Logger.Info(head + "Sending request...", ConsoleColor.DarkYellow);
                    StreamReader sr = new StreamReader(stream);
                    StreamWriter sw = new StreamWriter(stream);
                    sw.Write("GET / HTTP/1.1\r\nHost: " + URI.Host + "\r\nConnection: Close\r\n\r\n");
                    sw.Flush();
                    if (Cancellation.IsCancellationRequested) return false;
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
                    if (Cancellation.IsCancellationRequested) return false;
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
