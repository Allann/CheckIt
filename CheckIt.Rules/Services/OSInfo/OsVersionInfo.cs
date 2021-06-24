using System;
using CheckIt.Services.OSInfo.Utils;

namespace CheckIt.Services.OSInfo
{
    public sealed class OsVersionInfo : IComparable, ICloneable, IComparable<OsVersionInfo>, IEquatable<OsVersionInfo>
    {
        public Version Version { get; }
        public string Name { get; }
        public string Edition { get; }
        public string ServicePack { get; }
        public OsVersion OSVersion { get; }

        internal OsVersionInfo(Version version, string name, string edition, string servicePack, OsVersion osVersion)
        {
            Version = version;
            Name = name;
            Edition = edition;
            ServicePack = servicePack;
            OSVersion = osVersion;
        }

        public object Clone()
        {
            return new OsVersionInfo(Version, Name, Edition, ServicePack, OSVersion);
        }

        public int CompareTo(object o)
        {
            return CompareTo(o as OsVersionInfo);
        }

        public int CompareTo(OsVersionInfo other)
        {
            return Version.CompareTo(other.Version);
        }

        public bool Equals(OsVersionInfo other)
        {
            return other != null && other.Version == Version;
        }

        public override bool Equals(object o)
        {
            OsVersionInfo p = o as OsVersionInfo;
            return p != null && Equals(p);
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.HashAll(Version);
        }

        public static bool operator ==(OsVersionInfo o, OsVersionInfo p)
        {
            return Equals(o, p);
        }

        public static bool operator >(OsVersionInfo o, OsVersionInfo p)
        {
            if (o == null)
                return p == null;

            return o.CompareTo(p) > 0;
        }

        public static bool operator >=(OsVersionInfo o, OsVersionInfo p)
        {
            if (o == null)
                return p == null;

            return o.CompareTo(p) >= 0;
        }

        public static bool operator !=(OsVersionInfo o, OsVersionInfo p)
        {
            return !Equals(o, p);
        }

        public static bool operator <(OsVersionInfo o, OsVersionInfo p)
        {
            if (o == null)
                return p == null;

            return o.CompareTo(p) < 0;
        }

        public static bool operator <=(OsVersionInfo o, OsVersionInfo p)
        {
            if (o == null)
                return p == null;

            return o.CompareTo(p) <= 0;
        }

        public override string ToString()
        {
            return $"{Name} {Edition}";
        }
    }
}