using System;
using Microsoft.Win32;

namespace CheckIt.Services.DotNetInfo
{
    public class DotNetVersion
    {
        public static DotNetVersionInfo GetLatest()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\";
            using (
                var ndpKey =
                    RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default)
                        .OpenSubKey(subkey + "v4\\Full\\"))
            {
                var value = (int?) ndpKey?.GetValue("Release");
                if (value.HasValue)
                    return DotNetVersionExtensions.GetVersionFromRelease(value.Value);
            }

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(subkey))
            {
                if (ndpKey == null) return DotNetVersionInfo.Unknown;

                foreach (var versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (!versionKeyName.StartsWith("v")) continue;

                    var versionKey = ndpKey.OpenSubKey(versionKeyName);
                    if (versionKey == null) continue;

                    var name = (string) versionKey.GetValue("Version", "");
                    var sp = versionKey.GetValue("SP", "").ToString();
                    var install = versionKey.GetValue("Install", "").ToString();
                    if (install == "") 
                        return DotNetVersionExtensions.GetVersionFromKey(versionKeyName ,name,null);
                    else
                    {
                        if (sp != "" && install == "1")
                        {
                            return DotNetVersionExtensions.GetVersionFromKey(versionKeyName, name, sp);
                        }
                    }
                    if (name != "")
                    {
                        continue;
                    }

                    foreach (var subKeyName in versionKey.GetSubKeyNames())
                    {
                        var subKey = versionKey.OpenSubKey(subKeyName);
                        if (subKey == null) continue;
                        name = (string) subKey.GetValue("Version", "");
                        if (name != "")
                            sp = subKey.GetValue("SP", "").ToString();
                        install = subKey.GetValue("Install", "").ToString();
                        if (install == "") //no install info, must be later.
                            Console.WriteLine(versionKeyName + "  " + name);
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                return DotNetVersionExtensions.GetVersionFromKey(subKeyName, name, sp);
                            }
                            else if (install == "1")
                            {
                                return DotNetVersionExtensions.GetVersionFromKey(subKeyName, name, null);
                            }
                        }
                    }
                }
            }
            return DotNetVersionInfo.Unknown;
        }
    }
}