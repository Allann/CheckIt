using System;
using System.Collections.Generic;
using CheckIt.Contract;
using CheckIt.Services.SystemInfo;

namespace CheckIt.Rules
{
    public class TotalRamCheck : RuleBase
    {
        public TotalRamCheck(long minimumBytes)
        {
            MinimumBytes = minimumBytes;
        }
        public long MinimumBytes { get; }

        public override string Name => "Total Ram";
        public override string Description => "Minimum Ram requirements.";

        protected override IEnumerable<IRuleResult> RunRule(IContext context)
        {
            if (MinimumBytes <= 0)
                return new[] { new RuleResult(this, true, "Skipped minimum RAM check.") };

            var systemInfo = new SystemInformationService();
            var performanceData = systemInfo.PerformanceInformation;
            return performanceData.PhysicalTotalBytes < MinimumBytes ? 
                new[] { new RuleResult(this, false, $"Failed minimum RAM requirements. Expected: {SizeSuffix(MinimumBytes)}, Actual: {SizeSuffix(performanceData.PhysicalTotalBytes)}") } : 
                new[] { new RuleResult(this, true, $"Need {SizeSuffix(MinimumBytes)}, actual {SizeSuffix(performanceData.PhysicalTotalBytes)}.") };
        }

        private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        private static string SizeSuffix(long value)
        {
            if (value == 0) { return "0.0 bytes"; }

            var mag = (int)Math.Log(value, 1024);
            var adjustedSize = (decimal)value / (1L << (mag * 10));

            return $"{adjustedSize:n1} {SizeSuffixes[mag]}";
        }
    }
}