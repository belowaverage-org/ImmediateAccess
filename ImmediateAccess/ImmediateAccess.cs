using System;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Timers;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using Timer = System.Timers.Timer;

namespace ImmediateAccess
{
    class ImmediateAccess
    {

        private static bool IsCurrentlyEnsuring = false;
        private static bool IsNetworkAvailable = false;
        private static CancellationTokenSource EnsuranceCancel = new CancellationTokenSource();
        private static Timer NetEventCoolTimer = new Timer(3000); //CREATE POLICY FOR THIS BAD BOY
        public static async Task Start(string[] Paremeters)
        {
            NetEventCoolTimer.AutoReset = false;
            PolicyReader.ReadPolicies();
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
                while (true)
                {
                    EnsuranceCancel.Token.ThrowIfCancellationRequested();
                    if (!await TestNetwork.IsVpnServerAccessible())
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
                    Logger.Warning("Couldn't connect to VPN for some reason. Trying again in 5 seconds...");
                    await Task.Delay(5000);
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
                /*
                if (VpnControl.RasDialProcess != null)
                {
                    Logger.Warning("RasDial should not be running, Killing...");
                    VpnControl.RasDialProcess.Kill();
                    Logger.Info("Killed.");
                }
                */
            }
        }
        public static async Task Stop()
        {
            //await VpnControl.Disconnect();
        }
    }
}