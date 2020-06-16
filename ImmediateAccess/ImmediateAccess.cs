using System;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Timers;
using System.Threading;
using Timer = System.Timers.Timer;

namespace ImmediateAccess
{
    class ImmediateAccess
    {

        private static bool IsCurrentlyEnsuring = false;
        private static bool IsNetworkAvailable = false;
        private static CancellationTokenSource EnsuranceCancel = new CancellationTokenSource();
        private static Timer NetEventCoolTimer = new Timer();
        public static async Task Start(string[] Paremeters)
        {
            NetEventCoolTimer.AutoReset = false;
            PolicyReader.ReadPolicies();
            NetEventCoolTimer.Interval = (int)PolicyReader.Policies["NetEventCooldownS"] * 1000;
            Logger.Info("Observing network state...");
            IsNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();
            await EnsureConnectionToIntranet();
            Logger.Info("Registering event listeners...");
            NetEventCoolTimer.Elapsed += NetEventCoolTimer_Elapsed;
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            Logger.Info("Service is ready.", ConsoleColor.Green);
        }
        private async static void NetEventCoolTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await EnsureConnectionToIntranet();
        }
        private static async Task EnsureConnectionToIntranet()
        {
            try
            {
                if (IsCurrentlyEnsuring || !IsNetworkAvailable) return;
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
            catch(OperationCanceledException)
            {
                IsCurrentlyEnsuring = false;
                _ = EnsureConnectionToIntranet();
            }
        }
        private static void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            Logger.Info("Network change detected: IP Address.");
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
        public static async Task Stop()
        {
            await VpnControl.Disconnect();
        }
    }
}