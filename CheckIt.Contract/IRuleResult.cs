using System;
using System.Collections.Generic;

namespace CheckIt.Contract
{
    public interface IRuleResult
    {
        IRule Rule { get; }
        bool Success { get; }
        string Message { get; }
        IEnumerable<IFix> RepairInstructions { get; }

        void AddFix(IFix fix);

        void MarkAsFixed(IEnumerable<IFixResult> fixes);
        void MarkAsFailed(IEnumerable<IFixResult> fixes);
        IEnumerable<IFixResult> Fixes { get; }
    }
}