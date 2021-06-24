using System.Collections.Generic;
using System.IO;
using CheckIt.Contract;
using CheckIt.Fixes;

namespace CheckIt.Rules
{
    public class NodejsCheck : RuleBase
    {
        public override string Name => "Node";
        public override string Description => "Check if Node is installed.";

        protected override IEnumerable<IRuleResult> RunRule(IContext context)
        {
            var directory = new DirectoryInfo($"c:\\program files\\nodejs");
            if (directory.Exists)
                return new IRuleResult[] { new RuleResult(this, true, "nodejs installed.") };

            var result = new List<IRuleResult>();
            var r = new RuleResult(this, false, $"nodejs missing.");
            r.AddFix(new InstallNode());
            result.Add(r);

            return result;
        }
    }
}