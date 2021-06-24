using System.Diagnostics.CodeAnalysis;

namespace CheckIt.Services.OSInfo
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum OsVersion
    {
        Unknown,
        Win32S,
        WinCE,
        Win95,
        Win98,
        WinME,
        WinNT351,
        WinNT4,
        Win2000,
        WinXP,
        Win2003,
        WinXPx64,
        Vista,
        WinServer2008,
        WinServer2008R2,
        Win7,
        WinServer2012,
        Win8,
        WinServer2012R2,
        Win81,
        Win10,
        WinServer2016,
    }
}