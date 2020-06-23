using System;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Timers;
using System.Threading;
using Timer = System.Timers.Timer;
using System.Linq;

namespace ImmediateAccess
{
    class ImmediateAccess
    {
        public static bool IsDebugMode = true;
        private static bool IsCurrentlyEnsuring = false;
        private static bool IsNetworkAvailable = false;
        private static CancellationTokenSource EnsuranceCancel = new CancellationTokenSource();
        private static Timer NetEventCoolTimer = new Timer();
        private static Timer HealthCheckTimer = new Timer();
        public static void Start(string[] args)
        {
            IsDebugMode = args.Contains("/debug");
            Logger.Info("Observing network state...");
            IsNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();
            Logger.Info("Registering event listeners...");
            HealthCheckTimer.Elapsed += HealthCheckTimer_Elapsed;
            NetEventCoolTimer.Elapsed += NetEventCoolTimer_Elapsed;
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            Logger.Info("Service is ready.", ConsoleColor.Green);
            _ = EnsureConnectionToIntranet();
        }
        public static async Task Stop()
        {
            Logger.Info("Service is stopping...");
            HealthCheckTimer.Enabled = NetEventCoolTimer.Enabled = false;
            await VpnControl.Disconnect();
        }
        private static async Task EnsureConnectionToIntranet()
        {
            try
            {
                if (IsCurrentlyEnsuring || !IsNetworkAvailable || !IsServiceEnabled()) return;
                EnsuranceCancel = new CancellationTokenSource();
                IsCurrentlyEnsuring = true;
                if (await TestNetwork.IsProbeAvailable())
                {
                    IsCurrentlyEnsuring = false;
                    await VpnControl.Disconnect();
                    return;
                }
                if (await VpnControl.IsConnected() != null)
                {
                    IsCurrentlyEnsuring = false;
                    return;
                }
                EnsuranceCancel.Token.ThrowIfCancellationRequested();
                int attempts = (int)PolicyReader.Policies["VpnServerConnectAttempts"];
                while (attempts-- > 0)
                {
                    EnsuranceCancel.Token.ThrowIfCancellationRequested();
                    if (!await TestNetwork.SelectOnlineVpnProfile())
                    {
                        IsCurrentlyEnsuring = false;
                        return;
                    }
                    if (await VpnControl.Connect(EnsuranceCancel.Token))
                    {
                        IsCurrentlyEnsuring = false;
                        _ = EnsureConnectionToIntranet();
                        return;
                    }
                    if (attempts > 0)
                    {
                        Logger.Warning("Couldn't connect to VPN for some reason. Trying again in 5 seconds...");
                        await Task.Delay(5000);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                IsCurrentlyEnsuring = false;
                _ = EnsureConnectionToIntranet();
            }
        }
        private static void UpdatePolicy()
        {
            PolicyReader.ReadPolicies();
            HealthCheckTimer.Interval = (int)PolicyReader.Policies["HealthCheckIntervalS"] * 1000;
            HealthCheckTimer.Start();
            NetEventCoolTimer.Interval = (int)PolicyReader.Policies["NetEventCooldownS"] * 1000;
            NetEventCoolTimer.Stop();
        }
        private static bool IsServiceEnabled()
        {
            UpdatePolicy();
            string probeConf = (string)PolicyReader.Policies["InternalProbe"];
            string[] profiConf = (string[])PolicyReader.Policies["VpnProfileList"];
            if (
                probeConf == null ||
                profiConf == null ||
                probeConf == "" ||
                profiConf.Length == 0
            ) return false;
            return true;
        }
        private static void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            Logger.Info("Event: Network changed!");
            EnsuranceCancel.Cancel();
            NetEventCoolTimer.Stop();
            NetEventCoolTimer.Start();
        }
        private static void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            IsNetworkAvailable = e.IsAvailable;
            if (e.IsAvailable)
            {
                Logger.Info("Network is now available.", ConsoleColor.Green);
            }
            else
            {
                Logger.Warning("Network is no longer available.");
                EnsuranceCancel.Cancel();
            }
        }
        private static async void HealthCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Logger.Info("Health check timer lapsed!");
            await EnsureConnectionToIntranet();
        }
        private static async void NetEventCoolTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            NetEventCoolTimer.Stop();
            await EnsureConnectionToIntranet();
        }
    }
}