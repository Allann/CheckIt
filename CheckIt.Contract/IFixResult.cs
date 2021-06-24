using System.Collections.Generic;

namespace CheckIt.Contract
{
    public interface IFixResult
    {
        bool Success { get; }
        string Message { get; }
        IEnumerable<string> FailedInstructions { get; }
        void AddFailure(IInstruction instruction);
    }
}
