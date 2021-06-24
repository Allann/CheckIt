using CheckIt.Contract;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CheckIt
{
    public class FixResult : IFixResult
    {
        private IList<string> _instructions = new List<string>();

        public bool Success { get; set; }

        public string Message { get; set; }

        public void AddFailure(IInstruction instruction)
        {
            _instructions.Add(instruction.Text);
        }

        public IEnumerable<string> FailedInstructions => new ReadOnlyCollection<string>(_instructions);
    }
}
