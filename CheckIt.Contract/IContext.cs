using System.Collections.Generic;

namespace CheckIt.Contract
{
    public interface IContext
    {
        void Add(ResultLevel level, IRuleResult result);

        IEnumerable<IRuleResult> Information { get; }
        IEnumerable<IRuleResult> Warnings { get; }
        IEnumerable<IRuleResult> Failures { get; }

        bool HasWarnings();
        bool HasFailures();
        bool HasInformation();
    }
}