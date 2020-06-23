﻿using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace ImmediateAccess
{
    class PolicyReader
    {
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
        public static Dictionary<string, object> Policies = new Dictionary<string, object>();
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
    }
}