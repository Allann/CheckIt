using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CheckIt.Services.OSInfo.Interop;
using Microsoft.Win32;

namespace CheckIt.Services.OSInfo
{
    public static class OSVersionService
    {
        public enum ProcessorArchitecture
        {
            Unknown = 0,
            Bit32 = 1,
            Bit64 = 2,
            Itanium64 = 3
        }

        public enum SoftwareArchitecture
        {
            Unknown = 0,
            Bit32 = 1,
            Bit64 = 2
        }

        private static OsVersionInfo _current;

        public static OsVersionInfo GetCurrent()
        {
            if (_current != null) return _current;
            _current = Refresh();
            return _current;
        }

        private static OsVersionInfo Refresh()
        {
            int major = Environment.OSVersion.Version.Major;
            int minor = Environment.OSVersion.Version.Minor;
            int rev = 0;
            int build;
            string name = "";
            string edition = "";
            string servicePack = "";

            using (var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion"))
            {
                var buildNumber = key?.GetValue("CurrentBuildNumber").ToString();
                if (!int.TryParse(buildNumber, out build))
                    build = Environment.OSVersion.Version.Build;

                var productName = key?.GetValue("ProductName").ToString();
                var isWindows10 = !string.IsNullOrEmpty(productName) && productName.StartsWith("Windows 10", StringComparison.OrdinalIgnoreCase);

                if (isWindows10)
                {
                    var currentMajor = key.GetValue("CurrentMajorVersionNumber", 10).ToString();
                    int.TryParse(currentMajor, out major);
                    var currentMinor = key.GetValue("CurrentMinorVersionNumber", 10).ToString();
                    int.TryParse(currentMinor, out minor);
                }
                else
                {
                    var currentVersion = key?.GetValue("CurrentVersion").ToString();
                    if (!string.IsNullOrEmpty(currentVersion))
                    {
                        var splitVersion = currentVersion.Split('.');
                        major = int.Parse(splitVersion[0]);
                        minor = int.Parse(splitVersion[1]);
                    }
                }
                rev = Environment.OSVersion.Version.Revision;
            }

            var osVersionInfo = new OSVERSIONINFOEX { dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX)) };
            if (NativeMethods.GetVersionEx(ref osVersionInfo))
            {
                servicePack = osVersionInfo.szCSDVersion;
                var productType = osVersionInfo.wProductType;
                var suiteMask = osVersionInfo.wSuiteMask;
                edition = GetEdition(productType, suiteMask, osVersionInfo.wServicePackMajor, osVersionInfo.wServicePackMinor);
                name = GetName(major, minor, Environment.OSVersion.Platform, osVersionInfo.wProductType, servicePack);
            }

            var e = GetVersionEnum(name);
            return new OsVersionInfo(new Version(major, minor, build, rev), name, edition, servicePack, e);
        }

        private static OsVersion GetVersionEnum(string name)
        {
            if (name == "Windows 3.1") return OsVersion.Win32S;
            if (name == "Windows CE") return OsVersion.WinCE;
            if (name.StartsWith("Windows 95")) return OsVersion.Win95;
            if (name.StartsWith("Windows 98")) return OsVersion.Win98;
            if (name == "Windows Me") return OsVersion.WinME;
            if (name == "Windows NT 3.51") return OsVersion.WinNT351;
            if (name.StartsWith("Windows NT 4.0")) return OsVersion.WinNT4;
            if (name == "Windows 2000") return OsVersion.Win2000;
            if (name == "Windows XP") return OsVersion.WinXP;
            if (name == "Windows Server 2003") return OsVersion.Win2003;
            if (name == "Windows Vista") return OsVersion.Vista;
            if (name == "Windows Server 2008") return OsVersion.WinServer2008;
            if (name == "Windows 7") return OsVersion.Win7;
            if (name == "Windows Server 2008 R2") return OsVersion.WinServer2008R2;
            if (name == "Windows 8") return OsVersion.Win8;
            if (name == "Windows Server 2012") return OsVersion.WinServer2012;
            if (name == "Windows 8.1") return OsVersion.Win81;
            if (name == "Windows Server 2012 R2") return OsVersion.WinServer2012R2;
            if (name == "Windows 10") return OsVersion.Win10;
            if (name == "Windows Server 2016") return OsVersion.WinServer2016;

            return OsVersion.Unknown;
        }

        private static string GetEdition(byte productType, short suiteMask, short spMajor, short spMinor)
        {
            var major = Environment.OSVersion.Version.Major;
            var minor = Environment.OSVersion.Version.Minor;

            switch (major)
            {
                case 4:
                    switch (productType)
                    {
                        case NativeMethods.VER_NT_WORKSTATION:
                            return "Workstation";
                        case NativeMethods.VER_NT_SERVER:
                            return (suiteMask & NativeMethods.VER_SUITE_ENTERPRISE) != 0 ? "Enterprise Server" : "Standard Server";
                    }
                    break;
                case 5:
                    switch (productType)
                    {
                        case NativeMethods.VER_NT_WORKSTATION:
                            return (suiteMask & NativeMethods.VER_SUITE_PERSONAL) != 0
                                ? "Home"
                                : (NativeMethods.GetSystemMetrics(86) == 0 ? "Professional" : "Tablet Edition");
                        case NativeMethods.VER_NT_SERVER:
                            return minor == 0
                                ? ((suiteMask & NativeMethods.VER_SUITE_DATACENTER) != 0
                                    ? "Datacenter Server"
                                    : ((suiteMask & NativeMethods.VER_SUITE_ENTERPRISE) != 0 ? "Advanced Server" : "Server"))
                                : ((suiteMask & NativeMethods.VER_SUITE_DATACENTER) != 0
                                    ? "Datacenter"
                                    : ((suiteMask & NativeMethods.VER_SUITE_ENTERPRISE) != 0
                                        ? "Enterprise"
                                        : ((suiteMask & NativeMethods.VER_SUITE_BLADE) != 0 ? "Web Edition" : "Standard")));
                    }
                    break;
                case 6:
                    int ed;
                    if (!NativeMethods.GetProductInfo(major, minor, spMajor, spMinor, out ed)) return string.Empty;

                    switch (ed)
                    {
                        case NativeMethods.PRODUCT_BUSINESS:
                            return "Business";
                        case NativeMethods.PRODUCT_BUSINESS_N:
                            return "Business N";
                        case NativeMethods.PRODUCT_CLUSTER_SERVER:
                            return "HPC Edition";
                        case NativeMethods.PRODUCT_CLUSTER_SERVER_V:
                            return "HPC Edition without Hyper-V";
                        case NativeMethods.PRODUCT_DATACENTER_SERVER:
                            return "Datacenter Server";
                        case NativeMethods.PRODUCT_DATACENTER_SERVER_CORE:
                            return "Datacenter Server (core installation)";
                        case NativeMethods.PRODUCT_DATACENTER_SERVER_V:
                            return "Datacenter Server without Hyper-V";
                        case NativeMethods.PRODUCT_DATACENTER_SERVER_CORE_V:
                            return "Datacenter Server without Hyper-V (core installation)";
                        case NativeMethods.PRODUCT_EMBEDDED:
                            return "Embedded";
                        case NativeMethods.PRODUCT_ENTERPRISE:
                            return "Enterprise";
                        case NativeMethods.PRODUCT_ENTERPRISE_N:
                            return "Enterprise N";
                        case NativeMethods.PRODUCT_ENTERPRISE_E:
                            return "Enterprise E";
                        case NativeMethods.PRODUCT_ENTERPRISE_SERVER:
                            return "Enterprise Server";
                        case NativeMethods.PRODUCT_ENTERPRISE_SERVER_CORE:
                            return "Enterprise Server (core installation)";
                        case NativeMethods.PRODUCT_ENTERPRISE_SERVER_CORE_V:
                            return "Enterprise Server without Hyper-V (core installation)";
                        case NativeMethods.PRODUCT_ENTERPRISE_SERVER_IA64:
                            return "Enterprise Server for Itanium-based Systems";
                        case NativeMethods.PRODUCT_ENTERPRISE_SERVER_V:
                            return "Enterprise Server without Hyper-V";
                        case NativeMethods.PRODUCT_ESSENTIALBUSINESS_SERVER_MGMT:
                            return "Essential Business Server MGMT";
                        case NativeMethods.PRODUCT_ESSENTIALBUSINESS_SERVER_ADDL:
                            return "Essential Business Server ADDL";
                        case NativeMethods.PRODUCT_ESSENTIALBUSINESS_SERVER_MGMTSVC:
                            return "Essential Business Server MGMTSVC";
                        case NativeMethods.PRODUCT_ESSENTIALBUSINESS_SERVER_ADDLSVC:
                            return "Essential Business Server ADDLSVC";
                        case NativeMethods.PRODUCT_HOME_BASIC:
                            return "Home Basic";
                        case NativeMethods.PRODUCT_HOME_BASIC_N:
                            return "Home Basic N";
                        case NativeMethods.PRODUCT_HOME_BASIC_E:
                            return "Home Basic E";
                        case NativeMethods.PRODUCT_HOME_PREMIUM:
                            return "Home Premium";
                        case NativeMethods.PRODUCT_HOME_PREMIUM_N:
                            return "Home Premium N";
                        case NativeMethods.PRODUCT_HOME_PREMIUM_E:
                            return "Home Premium E";
                        case NativeMethods.PRODUCT_HOME_PREMIUM_SERVER:
                            return "Home Premium Server";
                        case NativeMethods.PRODUCT_HYPERV:
                            return "Microsoft Hyper-V Server";
                        case NativeMethods.PRODUCT_MEDIUMBUSINESS_SERVER_MANAGEMENT:
                            return "Windows Essential Business Management Server";
                        case NativeMethods.PRODUCT_MEDIUMBUSINESS_SERVER_MESSAGING:
                            return "Windows Essential Business Messaging Server";
                        case NativeMethods.PRODUCT_MEDIUMBUSINESS_SERVER_SECURITY:
                            return "Windows Essential Business Security Server";
                        case NativeMethods.PRODUCT_PROFESSIONAL:
                            return "Professional";
                        case NativeMethods.PRODUCT_PROFESSIONAL_N:
                            return "Professional N";
                        case NativeMethods.PRODUCT_PROFESSIONAL_E:
                            return "Professional E";
                        case NativeMethods.PRODUCT_SB_SOLUTION_SERVER:
                            return "SB Solution Server";
                        case NativeMethods.PRODUCT_SB_SOLUTION_SERVER_EM:
                            return "SB Solution Server EM";
                        case NativeMethods.PRODUCT_SERVER_FOR_SB_SOLUTIONS:
                            return "Server for SB Solutions";
                        case NativeMethods.PRODUCT_SERVER_FOR_SB_SOLUTIONS_EM:
                            return "Server for SB Solutions EM";
                        case NativeMethods.PRODUCT_SERVER_FOR_SMALLBUSINESS:
                            return "Windows Essential Server Solutions";
                        case NativeMethods.PRODUCT_SERVER_FOR_SMALLBUSINESS_V:
                            return "Windows Essential Server Solutions without Hyper-V";
                        case NativeMethods.PRODUCT_SERVER_FOUNDATION:
                            return "Server Foundation";
                        case NativeMethods.PRODUCT_SMALLBUSINESS_SERVER:
                            return "Windows Small Business Server";
                        case NativeMethods.PRODUCT_SMALLBUSINESS_SERVER_PREMIUM:
                            return "Windows Small Business Server Premium";
                        case NativeMethods.PRODUCT_SMALLBUSINESS_SERVER_PREMIUM_CORE:
                            return "Windows Small Business Server Premium (core installation)";
                        case NativeMethods.PRODUCT_SOLUTION_EMBEDDEDSERVER:
                            return "Solution Embedded Server";
                        case NativeMethods.PRODUCT_SOLUTION_EMBEDDEDSERVER_CORE:
                            return "Solution Embedded Server (core installation)";
                        case NativeMethods.PRODUCT_STANDARD_SERVER:
                            return "Standard Server";
                        case NativeMethods.PRODUCT_STANDARD_SERVER_CORE:
                            return "Standard Server (core installation)";
                        case NativeMethods.PRODUCT_STANDARD_SERVER_SOLUTIONS:
                            return "Standard Server Solutions";
                        case NativeMethods.PRODUCT_STANDARD_SERVER_SOLUTIONS_CORE:
                            return "Standard Server Solutions (core installation)";
                        case NativeMethods.PRODUCT_STANDARD_SERVER_CORE_V:
                            return "Standard Server without Hyper-V (core installation)";
                        case NativeMethods.PRODUCT_STANDARD_SERVER_V:
                            return "Standard Server without Hyper-V";
                        case NativeMethods.PRODUCT_STARTER:
                            return "Starter";
                        case NativeMethods.PRODUCT_STARTER_N:
                            return "Starter N";
                        case NativeMethods.PRODUCT_STARTER_E:
                            return "Starter E";
                        case NativeMethods.PRODUCT_STORAGE_ENTERPRISE_SERVER:
                            return "Enterprise Storage Server";
                        case NativeMethods.PRODUCT_STORAGE_ENTERPRISE_SERVER_CORE:
                            return "Enterprise Storage Server (core installation)";
                        case NativeMethods.PRODUCT_STORAGE_EXPRESS_SERVER:
                            return "Express Storage Server";
                        case NativeMethods.PRODUCT_STORAGE_EXPRESS_SERVER_CORE:
                            return "Express Storage Server (core installation)";
                        case NativeMethods.PRODUCT_STORAGE_STANDARD_SERVER:
                            return "Standard Storage Server";
                        case NativeMethods.PRODUCT_STORAGE_STANDARD_SERVER_CORE:
                            return "Standard Storage Server (core installation)";
                        case NativeMethods.PRODUCT_STORAGE_WORKGROUP_SERVER:
                            return "Workgroup Storage Server";
                        case NativeMethods.PRODUCT_STORAGE_WORKGROUP_SERVER_CORE:
                            return "Workgroup Storage Server (core installation)";
                        case NativeMethods.PRODUCT_UNDEFINED:
                            return "Unknown product";
                        case NativeMethods.PRODUCT_ULTIMATE:
                            return "Ultimate";
                        case NativeMethods.PRODUCT_ULTIMATE_N:
                            return "Ultimate N";
                        case NativeMethods.PRODUCT_ULTIMATE_E:
                            return "Ultimate E";
                        case NativeMethods.PRODUCT_WEB_SERVER:
                            return "Web Server";
                        case NativeMethods.PRODUCT_WEB_SERVER_CORE:
                            return "Web Server (core installation)";
                    }
                    break;
            }
            return string.Empty;
        }

        private static string GetName(int major, int minor, PlatformID platform, byte productType, string csdVersion)
        {
            switch (platform)
            {
                case PlatformID.Win32S:
                    return "Windows 3.1";
                case PlatformID.WinCE:
                    return "Windows CE";
                case PlatformID.Win32Windows:
                    {
                        if (major == 4)
                        {
                            switch (minor)
                            {
                                case 0:
                                    return csdVersion == "B" || csdVersion == "C" ? "Windows 95 OSR2" : "Windows 95";
                                case 10:
                                    return csdVersion == "A" ? "Windows 98 Second Edition" : "Windows 98";
                                case 90:
                                    return "Windows Me";
                            }
                        }
                        break;
                    }
                case PlatformID.Win32NT:
                    {
                        switch (major)
                        {
                            case 3:
                                return "Windows NT 3.51";
                            case 4:
                                switch (productType)
                                {
                                    case 1:
                                        return "Windows NT 4.0";
                                    case 3:
                                        return "Windows NT 4.0 Server";
                                }
                                break;
                            case 5:
                                switch (minor)
                                {
                                    case 0:
                                        return "Windows 2000";
                                    case 1:
                                        return "Windows XP";
                                    case 2:
                                        return "Windows Server 2003";
                                }
                                break;
                            case 6:
                                switch (minor)
                                {
                                    case 0:
                                        switch (productType)
                                        {
                                            case 1:
                                                return "Windows Vista";
                                            case 3:
                                                return "Windows Server 2008";
                                        }
                                        break;

                                    case 1:
                                        switch (productType)
                                        {
                                            case 1:
                                                return "Windows 7";
                                            case 3:
                                                return "Windows Server 2008 R2";
                                        }
                                        break;
                                    case 2:
                                        switch (productType)
                                        {
                                            case 1:
                                                return "Windows 8";
                                            case 3:
                                                return "Windows Server 2012";
                                        }
                                        break;
                                    case 3:
                                        switch (productType)
                                        {
                                            case 1:
                                                return "Windows 8.1";
                                            case 3:
                                                return "Windows Server 2012 R2";
                                        }
                                        break;
                                }
                                break;
                            case 10:
                                switch (minor)
                                {
                                    case 0:
                                        switch (productType)
                                        {
                                            case 1:
                                                return "Windows 10";
                                            case 3:
                                                return "Windows Server 2016";
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                    }
            }
            return string.Empty;
        }

        public static SoftwareArchitecture ProgramBits
        {
            get
            {
                SoftwareArchitecture pbits;
                switch (IntPtr.Size * 8)
                {
                    case 64:
                        pbits = SoftwareArchitecture.Bit64;
                        break;

                    case 32:
                        pbits = SoftwareArchitecture.Bit32;
                        break;

                    default:
                        pbits = SoftwareArchitecture.Unknown;
                        break;
                }
                return pbits;
            }
        }

        public static SoftwareArchitecture OSBits
        {
            get
            {
                SoftwareArchitecture osbits;

                switch (IntPtr.Size * 8)
                {
                    case 64:
                        osbits = SoftwareArchitecture.Bit64;
                        break;

                    case 32:
                        osbits = Is32BitProcessOn64BitProcessor()
                            ? SoftwareArchitecture.Bit64
                            : SoftwareArchitecture.Bit32;
                        break;

                    default:
                        osbits = SoftwareArchitecture.Unknown;
                        break;
                }

                return osbits;
            }
        }

        public static ProcessorArchitecture ProcessorBits
        {
            get
            {
                var pbits = ProcessorArchitecture.Unknown;

                try
                {
                    var systemInfo = new SYSTEM_INFO();
                    NativeMethods.GetNativeSystemInfo(ref systemInfo);

                    switch (systemInfo.uProcessorInfo.wProcessorArchitecture)
                    {
                        case 9: // PROCESSOR_ARCHITECTURE_AMD64
                            pbits = ProcessorArchitecture.Bit64;
                            break;
                        case 6: // PROCESSOR_ARCHITECTURE_IA64
                            pbits = ProcessorArchitecture.Itanium64;
                            break;
                        case 0: // PROCESSOR_ARCHITECTURE_INTEL
                            pbits = ProcessorArchitecture.Bit32;
                            break;
                        default: // PROCESSOR_ARCHITECTURE_UNKNOWN
                            pbits = ProcessorArchitecture.Unknown;
                            break;
                    }
                }
                catch
                {
                    // Ignore        
                }

                return pbits;
            }
        }

        private static IsWow64ProcessDelegate GetIsWow64ProcessDelegate()
        {
            var handle = NativeMethods.LoadLibrary("kernel32");

            if (handle != IntPtr.Zero)
            {
                var fnPtr = NativeMethods.GetProcAddress(handle, "IsWow64Process");

                if (fnPtr != IntPtr.Zero)
                    return
                        (IsWow64ProcessDelegate)
                        Marshal.GetDelegateForFunctionPointer(fnPtr, typeof(IsWow64ProcessDelegate));
            }

            return null;
        }

        private static bool Is32BitProcessOn64BitProcessor()
        {
            var fnDelegate = GetIsWow64ProcessDelegate();

            if (fnDelegate == null)
                return false;

            bool isWow64;
            var retVal = fnDelegate.Invoke(Process.GetCurrentProcess().Handle, out isWow64);

            if (retVal == false)
                return false;

            return isWow64;
        }


        private delegate bool IsWow64ProcessDelegate([In] IntPtr handle, [Out] out bool isWow64Process);
    }
}