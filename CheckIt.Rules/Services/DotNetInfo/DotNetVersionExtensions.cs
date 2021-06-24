using System;
using System.Linq;

namespace CheckIt.Services.DotNetInfo
{
    public static class DotNetVersionExtensions
    {
        internal static DotNetVersionInfo GetVersionFromRelease(int release)
        {
            foreach (var info in KnownDotNetVersions.Versions)
                if (release >= info.Release) return info;

            return DotNetVersionInfo.Unknown;
        }

        internal static DotNetVersionInfo GetVersionFromKey(string keyName, string name, string sp)
        {
            if (keyName == "v2.0.50727")
            {
                if (sp == "2")
                {
                    return KnownDotNetVersions.Versions.Single(v => v.Release == 1);
                }
                return KnownDotNetVersions.Versions.Single(v => v.Release == 0);
            }
            if (keyName == "v3.0")
            {
                return KnownDotNetVersions.Versions.Single(v => v.Release == 2);
            }
            if (keyName == "v3.0.30729")
            {
                return KnownDotNetVersions.Versions.Single(v => v.Release == 3);
            }
            if (keyName == "v3.5")
            {
                return KnownDotNetVersions.Versions.Single(v => v.Release == 4);
            }
            if (keyName == "v3.5.30729")
            {
                return KnownDotNetVersions.Versions.Single(v => v.Release == 5);
            }
            if (keyName == "v4")
            {
                if (name == "Client")
                    return KnownDotNetVersions.Versions.Single(v => v.Release == 6);
                if (name == "Full")
                {
                    return KnownDotNetVersions.Versions.Single(v => v.Release == 7);
                }
            }
            return DotNetVersionInfo.Unknown;
        }


        public static bool IsGreaterThanOrEqual(this DotNetVersionInfo info, DotNetVersionInfo other)
        {
            return ((IComparable<DotNetVersionInfo>) info).CompareTo(other) >= 0;
        }

        public static bool IsGreaterThan(this DotNetVersionInfo info, DotNetVersionInfo other)
        {
            return ((IComparable<DotNetVersionInfo>)info).CompareTo(other) > 0;
        }
    }
}
