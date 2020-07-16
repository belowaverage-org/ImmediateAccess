﻿using System;
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
        /// <summary>
        /// This method is the second entry point to Immediate Access. It sets up the timers, and invokes an initial connect check.
        /// </summary>
        /// <param name="args">String[]: An array of parameters. Currently, only "/debug" is supported.</param>
        public static void Start(string[] args)
        {
            IsDebugMode = args.Contains("/debug");
            Pipe.Setup();
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
        /// <summary>
        /// This method fires when the Immediate Access service is stopping, before allowing the service to stop
        /// this method will first disconnect the managed VPN profiles.
        /// </summary>
        /// <returns>Task: To allow this method to run asyncronously.</returns>
        public static async Task Stop()
        {
            Logger.Info("Service is stopping...");
            HealthCheckTimer.Enabled = NetEventCoolTimer.Enabled = false;
            await VpnControl.Disconnect();
        }
        /// <summary>
        /// This method contains the main logic for the Immediate Access service. In a nutshell, this method tests if the probe is available, and if it is not,
        /// selects and then connects to a VPN profile.
        /// </summary>
        /// <returns>Task: to enable threading of this method to allow for canceling.</returns>
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
        /// <summary>
        /// This method invokes the GPO reader, and then sets Immediate Access's internal timers.
        /// </summary>
        private static void UpdatePolicy()
        {
            PolicyReader.ReadPolicies();
            HealthCheckTimer.Interval = (int)PolicyReader.Policies["HealthCheckIntervalS"] * 1000;
            HealthCheckTimer.Start();
            NetEventCoolTimer.Interval = (int)PolicyReader.Policies["NetEventCooldownS"] * 1000;
            NetEventCoolTimer.Stop();
        }
        /// <summary>
        /// This method determines if the GPO to enable Immediate Access is defined.
        /// </summary>
        /// <returns>Boolean: True if service should be enabled, and false if not.</returns>
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
        /// <summary>
        /// This event fires whenever an IP address changes on any system network adapter.
        /// </summary>
        private static void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            Logger.Info("Event: Network changed!");
            EnsuranceCancel.Cancel();
            NetEventCoolTimer.Stop();
            NetEventCoolTimer.Start();
        }
        /// <summary>
        /// This event fires whenever all network adapters loose network connectivity (link) and fires when any network adapter re-gains a link.
        /// </summary>
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
        /// <summary>
        /// This event fires on an interval with the purpose of invoking the EnsureConnectionToIntranet() method
        /// as a catch-all if other events failed to detect the system in an offline state (away from probe / not on VPN).
        /// </summary>
        private static async void HealthCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Logger.Info("Health check timer lapsed!");
            await EnsureConnectionToIntranet();
        }
        /// <summary>
        /// This event fires whenever the Net Event Cooldown Timer lapses. The net event cooldown timer prevents multiple
        /// IP address change events from firing the EnsureConnectionToIntranet() in rapid succession.
        /// </summary>
        private static async void NetEventCoolTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            NetEventCoolTimer.Stop();
            await EnsureConnectionToIntranet();
        }
    }
}