using System;
using System.Collections.Generic;
using System.IO;
using CheckIt.Contract;
using CheckIt.Fixes;

namespace CheckIt.Rules
{
    public class NodeModuleCheck : RuleBase
    {
        public string ModuleName { get; }
        public NodeModuleCheck(string moduleName)
        {
            ModuleName = moduleName;
        }

        public override string Description => $"Check if npm module '{ModuleName}' is installed.";

        public override string Name => $"npm module '{ModuleName}'";

        protected override IEnumerable<IRuleResult> RunRule(IContext context)
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(folderPath, $"npm\\node_modules\\{ModuleName}");
            var directory = new DirectoryInfo(path);
            if (directory.Exists)
                return new IRuleResult[] { new RuleResult(this, true, $"npm module '{ModuleName}' installed.") };

            var result = new List<IRuleResult>();
            var r = new RuleResult(this, false, $"npm module '{ModuleName}' missing.");
            r.AddFix(new InstallNodeModule(ModuleName));
            result.Add(r);

            return result;
        }
    }
}