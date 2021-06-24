using System;
using System.Runtime.InteropServices;

namespace CheckIt.Services.OSInfo.Interop
{
    internal class NativeMethods
    {
        private NativeMethods()
        {
        }

        internal const int PRODUCT_UNDEFINED = 0x00000000;
        internal const int PRODUCT_ULTIMATE = 0x00000001;
        internal const int PRODUCT_HOME_BASIC = 0x00000002;
        internal const int PRODUCT_HOME_PREMIUM = 0x00000003;
        internal const int PRODUCT_ENTERPRISE = 0x00000004;
        internal const int PRODUCT_HOME_BASIC_N = 0x00000005;
        internal const int PRODUCT_BUSINESS = 0x00000006;
        internal const int PRODUCT_STANDARD_SERVER = 0x00000007;
        internal const int PRODUCT_DATACENTER_SERVER = 0x00000008;
        internal const int PRODUCT_SMALLBUSINESS_SERVER = 0x00000009;
        internal const int PRODUCT_ENTERPRISE_SERVER = 0x0000000A;
        internal const int PRODUCT_STARTER = 0x0000000B;
        internal const int PRODUCT_DATACENTER_SERVER_CORE = 0x0000000C;
        internal const int PRODUCT_STANDARD_SERVER_CORE = 0x0000000D;
        internal const int PRODUCT_ENTERPRISE_SERVER_CORE = 0x0000000E;
        internal const int PRODUCT_ENTERPRISE_SERVER_IA64 = 0x0000000F;
        internal const int PRODUCT_BUSINESS_N = 0x00000010;
        internal const int PRODUCT_WEB_SERVER = 0x00000011;
        internal const int PRODUCT_CLUSTER_SERVER = 0x00000012;
        internal const int PRODUCT_HOME_SERVER = 0x00000013;
        internal const int PRODUCT_STORAGE_EXPRESS_SERVER = 0x00000014;
        internal const int PRODUCT_STORAGE_STANDARD_SERVER = 0x00000015;
        internal const int PRODUCT_STORAGE_WORKGROUP_SERVER = 0x00000016;
        internal const int PRODUCT_STORAGE_ENTERPRISE_SERVER = 0x00000017;
        internal const int PRODUCT_SERVER_FOR_SMALLBUSINESS = 0x00000018;
        internal const int PRODUCT_SMALLBUSINESS_SERVER_PREMIUM = 0x00000019;
        internal const int PRODUCT_HOME_PREMIUM_N = 0x0000001A;
        internal const int PRODUCT_ENTERPRISE_N = 0x0000001B;
        internal const int PRODUCT_ULTIMATE_N = 0x0000001C;
        internal const int PRODUCT_WEB_SERVER_CORE = 0x0000001D;
        internal const int PRODUCT_MEDIUMBUSINESS_SERVER_MANAGEMENT = 0x0000001E;
        internal const int PRODUCT_MEDIUMBUSINESS_SERVER_SECURITY = 0x0000001F;
        internal const int PRODUCT_MEDIUMBUSINESS_SERVER_MESSAGING = 0x00000020;
        internal const int PRODUCT_SERVER_FOUNDATION = 0x00000021;
        internal const int PRODUCT_HOME_PREMIUM_SERVER = 0x00000022;
        internal const int PRODUCT_SERVER_FOR_SMALLBUSINESS_V = 0x00000023;
        internal const int PRODUCT_STANDARD_SERVER_V = 0x00000024;
        internal const int PRODUCT_DATACENTER_SERVER_V = 0x00000025;
        internal const int PRODUCT_ENTERPRISE_SERVER_V = 0x00000026;
        internal const int PRODUCT_DATACENTER_SERVER_CORE_V = 0x00000027;
        internal const int PRODUCT_STANDARD_SERVER_CORE_V = 0x00000028;
        internal const int PRODUCT_ENTERPRISE_SERVER_CORE_V = 0x00000029;
        internal const int PRODUCT_HYPERV = 0x0000002A;
        internal const int PRODUCT_STORAGE_EXPRESS_SERVER_CORE = 0x0000002B;
        internal const int PRODUCT_STORAGE_STANDARD_SERVER_CORE = 0x0000002C;
        internal const int PRODUCT_STORAGE_WORKGROUP_SERVER_CORE = 0x0000002D;
        internal const int PRODUCT_STORAGE_ENTERPRISE_SERVER_CORE = 0x0000002E;
        internal const int PRODUCT_STARTER_N = 0x0000002F;
        internal const int PRODUCT_PROFESSIONAL = 0x00000030;
        internal const int PRODUCT_PROFESSIONAL_N = 0x00000031;
        internal const int PRODUCT_SB_SOLUTION_SERVER = 0x00000032;
        internal const int PRODUCT_SERVER_FOR_SB_SOLUTIONS = 0x00000033;
        internal const int PRODUCT_STANDARD_SERVER_SOLUTIONS = 0x00000034;
        internal const int PRODUCT_STANDARD_SERVER_SOLUTIONS_CORE = 0x00000035;
        internal const int PRODUCT_SB_SOLUTION_SERVER_EM = 0x00000036;
        internal const int PRODUCT_SERVER_FOR_SB_SOLUTIONS_EM = 0x00000037;
        internal const int PRODUCT_SOLUTION_EMBEDDEDSERVER = 0x00000038;
        internal const int PRODUCT_SOLUTION_EMBEDDEDSERVER_CORE = 0x00000039;
        //internal const int ???? = 0x0000003A;
        internal const int PRODUCT_ESSENTIALBUSINESS_SERVER_MGMT = 0x0000003B;
        internal const int PRODUCT_ESSENTIALBUSINESS_SERVER_ADDL = 0x0000003C;
        internal const int PRODUCT_ESSENTIALBUSINESS_SERVER_MGMTSVC = 0x0000003D;
        internal const int PRODUCT_ESSENTIALBUSINESS_SERVER_ADDLSVC = 0x0000003E;
        internal const int PRODUCT_SMALLBUSINESS_SERVER_PREMIUM_CORE = 0x0000003F;
        internal const int PRODUCT_CLUSTER_SERVER_V = 0x00000040;
        internal const int PRODUCT_EMBEDDED = 0x00000041;
        internal const int PRODUCT_STARTER_E = 0x00000042;
        internal const int PRODUCT_HOME_BASIC_E = 0x00000043;
        internal const int PRODUCT_HOME_PREMIUM_E = 0x00000044;
        internal const int PRODUCT_PROFESSIONAL_E = 0x00000045;
        internal const int PRODUCT_ENTERPRISE_E = 0x00000046;
        internal const int PRODUCT_ULTIMATE_E = 0x00000047;
        //internal const int PRODUCT_UNLICENSED = 0xABCDABCD;

        internal const int VER_NT_WORKSTATION = 1;
        internal const int VER_NT_DOMAIN_CONTROLLER = 2;
        internal const int VER_NT_SERVER = 3;
        internal const int VER_SUITE_SMALLBUSINESS = 1;
        internal const int VER_SUITE_ENTERPRISE = 2;
        internal const int VER_SUITE_TERMINAL = 16;
        internal const int VER_SUITE_DATACENTER = 128;
        internal const int VER_SUITE_SINGLEUSERTS = 256;
        internal const int VER_SUITE_PERSONAL = 512;
        internal const int VER_SUITE_BLADE = 1024;

        [DllImport("Kernel32.dll")]
        internal static extern bool GetProductInfo(
            int osMajorVersion,
            int osMinorVersion,
            int spMajorVersion,
            int spMinorVersion,
            out int edition);

        [DllImport("kernel32.dll")]
        internal static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

        [DllImport("user32")]
        internal static extern int GetSystemMetrics(int nIndex);

        [DllImport("kernel32.dll")]
        internal static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll")]
        internal static extern void GetNativeSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr LoadLibrary(string libraryName);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr GetProcAddress(IntPtr hwnd, string procedureName);

    }
}