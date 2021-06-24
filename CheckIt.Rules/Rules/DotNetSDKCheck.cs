using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckIt.Contract;
using System.IO;
using CheckIt.Fixes;
using System.Diagnostics;

namespace CheckIt.Rules
{
    public class DotNetSDKCheck : RuleBase
    {
        public DotNetSDKCheck(string netCoreVersion = "1.0.0-preview2-1-003177")
        {
            NETCoreVersion = netCoreVersion;
        }

        public string NETCoreVersion { get; }

        public override string Name => ".Net Core SDK";
        public override string Description => "Check if .Net Core SDK is installed.";

        protected override IEnumerable<IRuleResult> RunRule(IContext context)
        {
            var directory = new DirectoryInfo($"c:\\program files\\dotnet\\sdk\\{NETCoreVersion}");
            if (directory.Exists)
                return new IRuleResult[] { new RuleResult(this, true, "dotnet installed.") };

            var result = new List<IRuleResult>();
            var r = new RuleResult(this, false, $"{NETCoreVersion} SDK missing.");
            r.AddFix(new InstallDotNetRuntime());
            result.Add(r);

            return result;
        }
    }
}
