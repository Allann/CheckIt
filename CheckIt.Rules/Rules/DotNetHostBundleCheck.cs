using System.Collections.Generic;
using System.IO;
using CheckIt.Contract;
using CheckIt.Fixes;

namespace CheckIt.Rules
{
    public class DotNetHostBundleCheck : RuleBase
    {
        public string NETCoreVersion { get; }

        public DotNetHostBundleCheck(string netCoreVersion = "1.1.2")
        {
            NETCoreVersion = netCoreVersion;
        }

        public override string Name => ".Net Host Bundle";
        public override string Description => "Check if the .Net Core Hosting Bundle is installed.";
        protected override IEnumerable<IRuleResult> RunRule(IContext context)
        {
            var directory = new DirectoryInfo($"c:\\program files\\dotnet\\host\\fxr\\{NETCoreVersion}");
            if (directory.Exists)
                return new IRuleResult[] { new RuleResult(this, true, "Host bundle installed.") };

            var result = new List<IRuleResult>();
            var r = new RuleResult(this, false, $"{NETCoreVersion} Host bundle missing.");
            r.AddFix(new InstallHostBundle());
            result.Add(r);

            return result;
        }
    }
}
