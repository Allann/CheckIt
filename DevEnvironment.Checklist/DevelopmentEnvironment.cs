using CheckIt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckIt.Contract;
using DevEnvironment.Checklist.Rules;
using CheckIt.Rules;

namespace DevEnvironment.Checklist
{
    public class DevelopmentEnvironment : ApplicationContextBase
    {
        public override string Name => "development";

        protected override void AddRules(IList<IRule> rules)
        {
            rules.Add(new MinimumSpecificationEnvironment());
            rules.Add(new NodejsCheck());
            rules.Add(new NodeModuleCheck("yo"));
            rules.Add(new NodeModuleCheck("bower"));
            rules.Add(new DotNetSDKCheck());
        }
    }
}
