namespace CheckIt.Contract
{
    public interface IApplicationContext
    {
        string Name { get; }

        IContext Check();
    }
}