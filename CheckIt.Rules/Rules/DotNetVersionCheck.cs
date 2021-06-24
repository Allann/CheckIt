using System.Collections.Generic;
using CheckIt.Contract;
using CheckIt.Services.DotNetInfo;

namespace CheckIt.Rules
{
    public class DotNetVersionCheck : RuleBase
    {
        public DotNetVersionCheck(DotNetVersionInfo minimumVersion)
        {
            MinimumVersion = minimumVersion;
        }
        public DotNetVersionInfo MinimumVersion { get; }

        public override string Name => ".NET Version";
        public override string Description => "Minimum .NET framework installed.";
        protected override IEnumerable<IRuleResult> RunRule(IContext context)
        {
            var latest = DotNetVersion.GetLatest();
            return latest.IsGreaterThanOrEqual(MinimumVersion)
                ? new IRuleResult[] { new RuleResult(this, true, $"Need {MinimumVersion.Name}, Actual {latest.Name}") }
                : new IRuleResult[] { new RuleResult(this, false, $".NET version required '{MinimumVersion.Name}', found '{latest.Name}'") };
        }
    }
}
