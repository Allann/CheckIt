using System.Collections.Generic;
using CheckIt.Services.DotNetInfo;
using CheckIt.Services.OSInfo;

namespace CheckIt.Rules
{
    public abstract class MinimumSpecificationBase : RuleBase
    {
        protected virtual long GetMinimumRamValue()
        {
            return 0;
        }

        protected abstract IEnumerable<OsVersion> ValidOperatingSystems();

        protected abstract bool Is64Bit();

        protected abstract DotNetVersionInfo MinimumDotNetVersion();
        
        public override void Initialise()
        {
            base.Initialise();

            AddRule(new OperatingSystemCheck(ValidOperatingSystems(), Is64Bit()));
            AddRule(new TotalRamCheck(GetMinimumRamValue()));
            AddRule(new DotNetVersionCheck(MinimumDotNetVersion()));
        }
    }
}
