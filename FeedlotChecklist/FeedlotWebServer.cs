using System.Collections.Generic;
using System.IO;
using CheckIt;
using CheckIt.Contract;
using CheckIt.Rules;
using Feedlot.Checklist.Rules;

namespace Feedlot.Checklist
{
    public class FeedlotWebServer : ApplicationContextBase
    {
        public override string Name => "Feedlot Web";
        
        protected override void AddRules(IList<IRule> rules)
        {
            rules.Add(new MinimumSpecificationForWebServer());
            rules.Add(new DotNetHostBundleCheck("1.1.2"));
            rules.Add(new DotNetSDKCheck("1.0.0-preview2-1-003177"));
            rules.Add(new DotNetCoreRuntimeCheck("1.1.0"));

            var path = AskQuestion("Installation path", "d:\\apps\\feedlot");
            if (!string.IsNullOrWhiteSpace(path))
                rules.Add(new FolderCheck(new DirectoryInfo(path)));

        }
    }
}
