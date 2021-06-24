using System;
using System.Runtime.InteropServices;

namespace CheckIt.Services.SystemInfo
{
    public class SystemInformationService
    {
        public PerformanceInfoData PerformanceInformation => PsApiWrapper.GetPerformanceInfo();
        
        private static class PsApiWrapper
        {
            [DllImport("psapi.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool GetPerformanceInfo([Out] out PsApiPerformanceInformation performanceInformation, [In] int size);

            [StructLayout(LayoutKind.Sequential)]
            private struct PsApiPerformanceInformation
            {
                public readonly int Size;
                public IntPtr CommitTotal;
                public IntPtr CommitLimit;
                public IntPtr CommitPeak;
                public IntPtr PhysicalTotal;
                public IntPtr PhysicalAvailable;
                public IntPtr SystemCache;
                public IntPtr KernelTotal;
                public IntPtr KernelPaged;
                public IntPtr KernelNonPaged;
                public IntPtr PageSize;
                public readonly int HandlesCount;
                public readonly int ProcessCount;
                public readonly int ThreadCount;
            }

            public static PerformanceInfoData GetPerformanceInfo()
            {
                PerformanceInfoData data = new PerformanceInfoData();
                PsApiPerformanceInformation perfInfo = new PsApiPerformanceInformation();
                if (GetPerformanceInfo(out perfInfo, Marshal.SizeOf(perfInfo)))
                {
                    // data in pages
                    data.CommitTotalPages = perfInfo.CommitTotal.ToInt64();
                    data.CommitLimitPages = perfInfo.CommitLimit.ToInt64();
                    data.CommitPeakPages = perfInfo.CommitPeak.ToInt64();

                    // data in bytes
                    var pageSize = perfInfo.PageSize.ToInt64();
                    data.PhysicalTotalBytes = perfInfo.PhysicalTotal.ToInt64() * pageSize;
                    data.PhysicalAvailableBytes = perfInfo.PhysicalAvailable.ToInt64() * pageSize;
                    data.SystemCacheBytes = perfInfo.SystemCache.ToInt64() * pageSize;
                    data.KernelTotalBytes = perfInfo.KernelTotal.ToInt64() * pageSize;
                    data.KernelPagedBytes = perfInfo.KernelPaged.ToInt64() * pageSize;
                    data.KernelNonPagedBytes = perfInfo.KernelNonPaged.ToInt64() * pageSize;
                    data.PageSizeBytes = pageSize;

                    // counters
                    data.HandlesCount = perfInfo.HandlesCount;
                    data.ProcessCount = perfInfo.ProcessCount;
                    data.ThreadCount = perfInfo.ThreadCount;
                }
                return data;
            }
        }
    }
}
