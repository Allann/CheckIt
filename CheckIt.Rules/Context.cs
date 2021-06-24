using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CheckIt.Contract;

namespace CheckIt
{
    public class Context : IContext
    {
        private readonly IList<IRuleResult> _information = new List<IRuleResult>();
        private readonly IList<IRuleResult> _warnings = new List<IRuleResult>();
        private readonly IList<IRuleResult> _failures = new List<IRuleResult>();

        public void Add(ResultLevel level, IRuleResult result)
        {
            switch (level)
            {
                case ResultLevel.Success:
                    _information.Add(result);
                    break;
                case ResultLevel.Failure:
                    if (result.Rule.Mandatory)
                        _failures.Add(result);
                    else
                        _warnings.Add(result);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        public IEnumerable<IRuleResult> Information => new ReadOnlyCollection<IRuleResult>(_information);
        public IEnumerable<IRuleResult> Warnings => new ReadOnlyCollection<IRuleResult>(_warnings);
        public IEnumerable<IRuleResult> Failures => new ReadOnlyCollection<IRuleResult>(_failures);

        public bool HasWarnings()
        {
            return _warnings.Any();
        }

        public bool HasFailures()
        {
            return _failures.Any();
        }

        public bool HasInformation()
        {
            return _information.Any();
        }
    }
}