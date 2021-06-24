using System.Collections.Generic;

namespace CheckIt.Contract
{
    public interface IRule
    {
        string Name { get; }
        string Description { get; }
        bool Mandatory { get; }
        IReadOnlyCollection<IRule> DependantOn { get; }

        bool IsInitialised();
        void Check(IContext context);
    }
}