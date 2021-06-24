using System.Collections.Generic;
using System.IO;
using CheckIt.Contract;
using CheckIt.Fixes;

namespace CheckIt.Rules
{
    public class DotNetCoreRuntimeCheck : RuleBase
    {
        public DotNetCoreRuntimeCheck(string netCoreVersion = "1.1.0")
        {
            NETCoreVersion = netCoreVersion;
        }

        public string NETCoreVersion { get; }

        public override string Name => ".Net Core Runtime";
        public override string Description => "Check if .Net Core Runtime is installed.";

        protected override IEnumerable<IRuleResult> RunRule(IContext context)
        {
            var directory = new DirectoryInfo($"c:\\program files\\dotnet\\shared\\Microsoft.NETCore.App\\{NETCoreVersion}");
            if (directory.Exists)
                return new IRuleResult[] { new RuleResult(this, true, "dotnet installed.") };

            var result = new List<IRuleResult>();
            var r = new RuleResult(this, false, $"{NETCoreVersion} Runtime missing.");
            r.AddFix(new InstallDotNetRuntime());
            result.Add(r);

            return result;
        }
    }
}