using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CheckIt.Contract;

namespace CheckIt.Rules
{
    public abstract class RuleBase : IRule
    {
        private bool _initialised;
        private readonly IList<IRule> _childRules = new List<IRule>();

        public abstract string Name { get; }
        public abstract string Description { get; }

        public void Check(IContext context)
        {
            if (!_initialised)
                Initialise();

            var results = RunRule(context).ToList();

            foreach (var ruleResult in results)
            {
                if (ruleResult.Success)
                {
                    context.Add(ResultLevel.Success, ruleResult);
                }
                else
                {
                    var fixes = ruleResult.RepairInstructions.Select(fix => RunFix(ruleResult, fix)).ToList();
                    if (!fixes.Any())
                        context.Add(ResultLevel.Failure, ruleResult);
                    else if (fixes.All(f => f.Success))
                    {
                        ruleResult.MarkAsFixed(fixes);
                        context.Add(ResultLevel.Success, ruleResult);
                    }
                    else
                    {
                        ruleResult.MarkAsFailed(fixes);
                        context.Add(ResultLevel.Failure, ruleResult);
                    }
                }
            }

            foreach (var rule in _childRules)
                rule.Check(context);
        }

        protected virtual IFixResult RunFix(IRuleResult result, IFix fix)
        {
            var passed = true;
            var fixResult = new FixResult();

            if (fix.Manual)
            {
                var idx = 1;
                foreach (var instruction in fix.Instructions)
                {
                    Console.Write($"{idx++}. {instruction.Text}");
                    if (instruction.IsInfoOnly)
                    {
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.Write(" (Success y/N)");
                        var key = Console.ReadKey();
                        var answer = key.Key == ConsoleKey.Y;
                        Console.WriteLine();
                        passed &= answer;

                        if (!answer)
                        {
                            fixResult.AddFailure(instruction);
                        }
                    }
                }
            }
            else
                passed = fix.Perform(result);

            fixResult.Success = passed;
            fixResult.Message = passed ? fix.PassResponse : fix.FailReponse;

            return fixResult;
        }

        protected abstract IEnumerable<IRuleResult> RunRule(IContext context);

        public virtual bool Mandatory => true;
        public IReadOnlyCollection<IRule> DependantOn => new ReadOnlyCollection<IRule>(_childRules);

        protected void AddRule(IRule rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            _childRules.Add(rule);
        }

        public virtual void Initialise()
        {
            _initialised = true;
        }

        public bool IsInitialised()
        {
            return _initialised;
        }
    }
}