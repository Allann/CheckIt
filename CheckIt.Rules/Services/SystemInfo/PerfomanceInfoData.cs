namespace CheckIt.Services.SystemInfo
{
    public class PerformanceInfoData
    {
        public long CommitTotalPages;
        public long CommitLimitPages;
        public long CommitPeakPages;
        public long PhysicalTotalBytes;
        public long PhysicalAvailableBytes;
        public long SystemCacheBytes;
        public long KernelTotalBytes;
        public long KernelPagedBytes;
        public long KernelNonPagedBytes;
        public long PageSizeBytes;
        public int HandlesCount;
        public int ProcessCount;
        public int ThreadCount;
    }
}