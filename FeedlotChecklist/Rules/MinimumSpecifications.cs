using System.Collections.Generic;
using CheckIt.Contract;
using CheckIt.Rules;
using CheckIt.Services.DotNetInfo;
using CheckIt.Services.OSInfo;

namespace Feedlot.Checklist.Rules
{
    public class MinimumSpecificationForWebServer : MinimumSpecificationBase
    {
        protected override long GetMinimumRamValue()
        {
            return 4L * 1024 * 1024 * 1024;
        }

        protected override IEnumerable<OsVersion> ValidOperatingSystems()
        {
            return new List<OsVersion>{OsVersion.WinServer2012R2, OsVersion.WinServer2012,OsVersion.Win10, OsVersion.WinServer2016};
        }

        protected override bool Is64Bit()
        {
            return true;
        }

        protected override DotNetVersionInfo MinimumDotNetVersion()
        {
            return KnownDotNetVersions.V452;
        }

        public override string Name => "Feedlot Minimum Specifications";

        public override string Description => "Check to ensure that the server has the minimum required features.";

        protected override IEnumerable<IRuleResult> RunRule(IContext context)
        {
            return new List<IRuleResult>();
        }
    }
}