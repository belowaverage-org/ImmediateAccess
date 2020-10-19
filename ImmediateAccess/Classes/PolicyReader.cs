using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace ImmediateAccess
{
    public class PolicyReader
    {
        /// <summary>
        /// A dictionary containing default GPO policy settings. Immediate Access will fall-back
        /// to these settings if it fails to find defined settings in GPO.
        /// </summary>
        private static Dictionary<string, object> DefaultPolicies = new Dictionary<string, object>()
        {
            { "InternalProbe", null },
            { "VpnProfileList", null },
            { "ProbeTimeoutS", 10 },
            { "NetEventCooldownS", 3 },
            { "HealthCheckIntervalS", 300 },
            { "VpnServerPingTimeoutMS", 1500 },
            { "VpnServerConnectAttempts", 3 }
        };
        /// <summary>
        /// A dictionary containing default / defined GPO policy settings depending on weather or not the policies were ever defined.
        /// </summary>
        public static Dictionary<string, object> Policies = new Dictionary<string, object>();
        /// <summary>
        /// This method merges the default policies and configured policies in GPO into the "Policies" dictionary.
        /// </summary>
        public static void ReadPolicies()
        {
            Logger.Info("GPO: Reading Policies...", ConsoleColor.Magenta);
            RegistryKey iak = null;
            Policies.Clear();
            try
            {
                iak = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Below Average\Immediate Access");
            }
            catch (Exception)
            {
                Logger.Warning("GPO: The Immediate Access GPO is not applied to this PC!");
                return;
            }
            foreach (KeyValuePair<string, object> defaultPolicy in DefaultPolicies)
            {
                object value = null;
                if (iak != null) value = iak.GetValue(defaultPolicy.Key);
                if (value == null)
                {
                    Logger.Info("GPO: " + defaultPolicy.Key + ": " + defaultPolicy.Value + ". (Not Set)", ConsoleColor.Magenta);
                    Policies.Add(defaultPolicy.Key, defaultPolicy.Value);
                    continue;
                }
                Logger.Info("GPO: " + defaultPolicy.Key + ": " + value + ".", ConsoleColor.Magenta);
                if (value.GetType() == typeof(string[]))
                {
                    foreach (string subValue in (string[])value)
                    {
                        Logger.Info("GPO:  - " + subValue, ConsoleColor.Magenta);
                    }
                }
                Policies.Add(defaultPolicy.Key, value);
            }
            if (iak != null) iak.Dispose();
            Logger.Info("GPO: Done!", ConsoleColor.Magenta);
        }
        /// <summary>
        /// This method determines if the GPO to enable Immediate Access is defined.
        /// </summary>
        /// <returns>Boolean: True if service should be enabled, and false if not.</returns>
        public static bool IsServiceEnabled()
        {
            string probeConf = (string)Policies["InternalProbe"];
            string[] profiConf = (string[])Policies["VpnProfileList"];
            if (
                probeConf == null ||
                !Uri.TryCreate(probeConf, UriKind.Absolute, out Uri _) ||
                profiConf == null ||
                profiConf.Length == 0
            ) return false;
            return true;
        }
    }
}