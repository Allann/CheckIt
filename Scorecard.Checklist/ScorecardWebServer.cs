using System.Collections.Generic;
using CheckIt;
using CheckIt.Contract;
using Scorecard.Checklist.Rules;

namespace Scorecard.Checklist
{
    public class ScorecardWebServer : ApplicationContextBase
    {
        public override string Name => "Scorecard";

        protected override void AddRules(IList<IRule> rules)
        {
            rules.Add(new ServerMinimumSpecifications());
        }
    }
}
