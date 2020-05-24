using System;
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
            { "EventCooldownMS", 5000 },
            { "ProbeAttempts", 5 },
            { "ProbeIntervalMS", 500 },
            { "ProbeTimeoutMS", 1000 }
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
                try
                {
                    object value =  iak.GetValue(defaultPolicy.Key);
                    Logger.Info("GPO: " + defaultPolicy.Key + ": " + value + ".", ConsoleColor.Magenta);
                    if (value.GetType() == typeof(string[]))
                    {
                        foreach(string subValue in (string[])value)
                        {
                            Logger.Info("GPO:  - " + subValue, ConsoleColor.Magenta);
                        }
                    }
                    Policies.Add(defaultPolicy.Key, value);
                }
                catch (Exception)
                {
                    Logger.Info("GPO: Policy \"" + defaultPolicy.Key + "\" is not set, using default value: " + defaultPolicy.Value + ".", ConsoleColor.Magenta);
                    Policies.Add(defaultPolicy.Key, defaultPolicy.Value);
                }
            }
            if(iak != null) iak.Dispose();
            Logger.Info("GPO: Done!", ConsoleColor.Magenta);
        }
    }
}
