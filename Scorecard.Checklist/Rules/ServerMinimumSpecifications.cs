using System.Collections.Generic;
using CheckIt;
using CheckIt.Contract;
using CheckIt.Rules;
using CheckIt.Services.DotNetInfo;
using CheckIt.Services.OSInfo;

namespace Scorecard.Checklist.Rules
{
    public class ServerMinimumSpecifications : MinimumSpecificationBase
    {
        protected override long GetMinimumRamValue()
        {
            return 32 * Constants.Gb;
        }

        protected override IEnumerable<OsVersion> ValidOperatingSystems()
        {
            var list = new List<OsVersion>();
            list.AddRange(new[] { OsVersion.WinServer2012, OsVersion.WinServer2012R2 });
            return list;
        }

        protected override bool Is64Bit()
        {
            return true;
        }

        protected override DotNetVersionInfo MinimumDotNetVersion()
        {
            return KnownDotNetVersions.V461;
        }

        public override string Name => "Scorecard Specifications";
        public override string Description => "Check to ensure that the server has the minimum required features.";
        protected override IEnumerable<IRuleResult> RunRule(IContext context)
        {
            return new List<IRuleResult>();
        }
    }
}