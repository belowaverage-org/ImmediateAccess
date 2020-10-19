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
    public class TestNetwork
    {
        /// <summary>
        /// This method checks if an Internal Probe is defined and then tests the probe.
        /// </summary>
        /// <param name="IncludeGpoVpnProfiles">Bool: Should this be executed from network adapters associated with GPO defined VPN profiles?</param>
        /// <returns>Task: Bool: Returns true if the probe is available.</returns>
        public static async Task<bool> IsProbeAvailable(bool IncludeGpoVpnProfiles = false)
        {
            if (PolicyReader.Policies["InternalProbe"] == null) return false;
            return await TestProbeFrom(GetAllIPAddresses(IncludeGpoVpnProfiles));
        }
        /// <summary>
        /// This method pings all GPO defined VPN profiles, and selects -- in GPO defined order -- the first profile to respond to a ping.
        /// </summary>
        /// <returns>Task: Bool: True if any profile responds, false if all profiles are offline.</returns>
        public static async Task<bool> SelectOnlineVpnProfile()
        {
            Logger.Info("Selecting optimal VPN profile...");
            foreach (string vpnProfile in (string[])PolicyReader.Policies["VpnProfileList"])
            {
                ManagementObject vpnStatus = await VpnStatus.Get(vpnProfile);
                if (vpnStatus == null) continue;
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
        /// <summary>
        /// This method runs multiple TestProbe methods (tasks) at once and if any return true the rest will be cancled and true will be returned
        /// by this method. Otherwise, this method waits for a GPO defined time before canceling all other tasks.
        /// </summary>
        /// <param name="Bind">IPAddress: The bind IPAddress.</param>
        /// <returns>Task: Bool: The result of the first TestProbe method to finish true, or false after a timeout.</returns>
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
        /// <summary>
        /// This method calls the HttpRequest method with the Internal Probe defined from GPO.
        /// </summary>
        /// <param name="Bind">IPAddress: The bind IPAddress.</param>
        /// <param name="Cancellation">CancellationToken: The token used to stop this method safely.</param>
        /// <returns>Task: Bool: Returns true if successful.</returns>
        private static async Task<bool> TestProbe(IPAddress Bind = null, CancellationToken Cancellation = new CancellationToken())
        {
            if (!Uri.TryCreate((string)PolicyReader.Policies["InternalProbe"], UriKind.Absolute, out Uri URI)) return false;
            return await HttpRequest(URI, Bind, Cancellation);
        }
        /// <summary>
        /// This method sends an HTTP(S) request to the internal probe, and if the certificates are trusted and up to snuff, returns true.
        /// </summary>
        /// <param name="URI">Uri: The URI to send the request.</param>
        /// <param name="Bind">IPAddress: The bind IPAddress.</param>
        /// <param name="Cancellation">CancellationToken: The token used to stop this method safely.</param>
        /// <returns>Task: Bool: Returns true if successful.</returns>
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
        /// <summary>
        /// This method pings the VPN servers and returns the result of the ping.
        /// </summary>
        /// <param name="Host">String: the host to ping.</param>
        /// <returns>Task: Bool: If true, the ping was successful. </returns>
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
        /// <summary>
        /// This method retrieves all network adapter IP addresses from the system. (Optionally including the GPO defined VPN profiles)
        /// </summary>
        /// <param name="IncludeGpoVpnProfiles">Bool: Should the retrieved IPs contain the VPN profile IP addresses?</param>
        /// <returns>IPAddress[]: An array of IPAddress objects.</returns>
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