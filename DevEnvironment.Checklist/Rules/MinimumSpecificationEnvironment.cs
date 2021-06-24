using CheckIt;
using CheckIt.Contract;
using CheckIt.Rules;
using CheckIt.Services.DotNetInfo;
using CheckIt.Services.OSInfo;
using System.Collections.Generic;

namespace DevEnvironment.Checklist.Rules
{
    public class MinimumSpecificationEnvironment : MinimumSpecificationBase
    {
        protected override long GetMinimumRamValue()
        {
            return 11 * Constants.Gb;
        }

        protected override bool Is64Bit()
        {
            return true;
        }

        protected override DotNetVersionInfo MinimumDotNetVersion()
        {
            return KnownDotNetVersions.V461;
        }

        protected override IEnumerable<OsVersion> ValidOperatingSystems()
        {
            return new[] { OsVersion.Win10, OsVersion.Win7, OsVersion.Win8, OsVersion.Win81 };
        }

        public override string Name => "Development Environment";
        public override string Description => "Check if the PC has minimum required features for a development environment.";

        protected override IEnumerable<IRuleResult> RunRule(IContext context)
        {
            return new List<IRuleResult>();
        }
    }
}
