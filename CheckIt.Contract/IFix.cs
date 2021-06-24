using System.Collections.Generic;

namespace CheckIt.Contract
{
    public interface IFix
    {
        bool Manual { get; }
        IEnumerable<IInstruction> Instructions { get; }
        bool Perform(IRuleResult ruleResult);

        string PassResponse { get; }
        string FailReponse { get; }
    }
}
