using System;

namespace CheckIt.Services.DotNetInfo
{
    public static class KnownDotNetVersions
    {
        public static readonly DotNetVersionInfo[] Versions;

        static KnownDotNetVersions()
        {
            Versions = new[] {V462, V461, V46, V452, V451, V45, V40Full, V40Client, V35SP1, V35, V30SP2, V30, V20SP2, V20 };
        }

        public static DotNetVersionInfo V20 => new DotNetVersionInfo(0, 2, 0, 50727, ".NET Framework 2.0");
        public static DotNetVersionInfo V20SP2 => new DotNetVersionInfo(1, 2, 0, 50727, ".NET Framework 2.0 SP2");
        public static DotNetVersionInfo V30 => new DotNetVersionInfo(2, 3, 0, 0, ".NET Framework 3.0");
        public static DotNetVersionInfo V30SP2 => new DotNetVersionInfo(3, 3, 0, 30729, ".NET Framework 3.0 SP2");
        public static DotNetVersionInfo V35 => new DotNetVersionInfo(4, 3, 5, 0, ".NET Framework 3.5");
        public static DotNetVersionInfo V35SP1 => new DotNetVersionInfo(5, 3, 5, 30729, ".NET Framework 3.5 SP1");
        public static DotNetVersionInfo V40Client => new DotNetVersionInfo(6, 4, 0, 0, ".NET Framework 4.0 (Client)");
        public static DotNetVersionInfo V40Full => new DotNetVersionInfo(7, 4, 0, 0, ".NET Framework 4.0 (Full)");
        public static DotNetVersionInfo V45 => new DotNetVersionInfo(378389, 4, 5, 0, ".NET Framework 4.5");
        public static DotNetVersionInfo V451 => new DotNetVersionInfo(378675, 4, 5, 1, ".NET Framework 4.5.1");
        public static DotNetVersionInfo V452 => new DotNetVersionInfo(379893, 4, 5, 2, ".NET Framework 4.5.2");
        public static DotNetVersionInfo V46 => new DotNetVersionInfo(393295, 4, 6, 0, ".NET Framework 4.6");
        public static DotNetVersionInfo V461 => new DotNetVersionInfo(394254, 4, 6, 1, ".NET Framework 4.6.1");
        public static DotNetVersionInfo V462 => new DotNetVersionInfo(394802, 4, 6, 2, ".NET Framework 4.6.2");
    }

    public sealed class DotNetVersionInfo : IComparable, ICloneable, IComparable<DotNetVersionInfo>, IEquatable<DotNetVersionInfo>
    {
        public static DotNetVersionInfo Unknown = new DotNetVersionInfo(0, 0, 0, 0, "Unknown");

        public DotNetVersionInfo(int release, int major, int minor, int rev, string name)
        {
            Release = release;
            Major = major;
            Minor = minor;
            Rev = rev;
            Name = name;
        }

        public int Release { get; }
        public int Major { get; }
        public int Minor { get; }
        public int Rev { get; }
        public string Name { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return ((IEquatable<DotNetVersionInfo>)this).Equals((DotNetVersionInfo)obj);
        }

        public bool Equals(DotNetVersionInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Release == other.Release;
        }

        public override int GetHashCode()
        {
            return Release;
        }

        public static bool operator ==(DotNetVersionInfo left, DotNetVersionInfo right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DotNetVersionInfo left, DotNetVersionInfo right)
        {
            return !Equals(left, right);
        }

        int IComparable.CompareTo(object obj)
        {
            var info = obj as DotNetVersionInfo;
            if (info == null) throw new InvalidCastException("Unable to compare object to DotNetVersionInfo.");

            return ((IComparable<DotNetVersionInfo>)info).CompareTo(this);
        }

        object ICloneable.Clone()
        {
            return new DotNetVersionInfo(Release, Major, Minor, Rev, Name);
        }

        int IComparable<DotNetVersionInfo>.CompareTo(DotNetVersionInfo other)
        {
            return Release.CompareTo(other.Release);
        }

        bool IEquatable<DotNetVersionInfo>.Equals(DotNetVersionInfo other)
        {
            return Release == other?.Release;
        }
    }
}
