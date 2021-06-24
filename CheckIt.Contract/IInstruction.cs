namespace CheckIt.Contract
{
    public interface IInstruction
    {
        string Text { get; }
        bool ShouldSucceed { get; }
        bool IsInfoOnly { get; }
    }
}
