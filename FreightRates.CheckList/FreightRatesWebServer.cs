using System;
using System.Collections.Generic;
using System.IO;
using CheckIt;
using CheckIt.Contract;
using CheckIt.Rules;
using FreightRates.Checklist.Rules;

namespace FreightRates.Checklist
{
    public class FreightRatesWebServer : ApplicationContextBase
    {
        public override string Name => "FreightRates";

        protected override void AddRules(IList<IRule> rules)
        {
            rules.Add(new MinimumSpecificationForWebServer());
            rules.Add(new DotNetHostBundleCheck());

            var path = AskQuestion("Installation path", "d:\\apps\\freightrates");
            if (!string.IsNullOrWhiteSpace(path))
                rules.Add(new FolderCheck(new DirectoryInfo(path)));
        }
    }
}
