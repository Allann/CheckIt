using CheckIt.Contract;

namespace CheckIt
{
    public class Instruction : IInstruction
    {
        public Instruction(string text, bool shouldSucceed = true, bool isInfoOnly = false)
        {
            Text = text;
            ShouldSucceed = shouldSucceed;
            IsInfoOnly = isInfoOnly;
        }
        public string Text { get; }
        public bool ShouldSucceed { get; }
        public bool IsInfoOnly { get; }
    }
}
