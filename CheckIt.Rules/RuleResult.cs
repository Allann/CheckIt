using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckIt.Contract;

namespace CheckIt
{
    public class RuleResult : IRuleResult
    {
        private readonly IList<IFix> _repairs = new List<IFix>();
        private readonly IList<IFixResult> _fixes = new List<IFixResult>();

        public RuleResult(IRule rule, bool success) : this(rule, success, null)
        { }

        public RuleResult(IRule rule, bool success, string message)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule), "Must provide a rule.");
            Rule = rule;
            Success = success;
            Message = message;
        }

        public IRule Rule { get; }
        public bool Success { get; private set; }
        public string Message { get; private set; }

        public IEnumerable<IFix> RepairInstructions => new ReadOnlyCollection<IFix>(_repairs);

        public void AddFix(IFix fix)
        {
            _repairs.Add(fix);
        }

        public IEnumerable<IFixResult> Fixes=>new ReadOnlyCollection<IFixResult>(_fixes);

        public void MarkAsFixed(IEnumerable<IFixResult> fixes)
        {
            Success = true;
            Message += "  Was fixed during check.";
            foreach (var result in fixes)
                _fixes.Add(result);
        }

        public void MarkAsFailed(IEnumerable<IFixResult> fixes)
        {
            Success = false;
            foreach (var result in fixes)
                _fixes.Add(result);
        }
    }
}
