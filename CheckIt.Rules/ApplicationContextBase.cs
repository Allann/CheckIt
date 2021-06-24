using System;
using System.Collections.Generic;
using CheckIt.Contract;

namespace CheckIt
{
    public abstract class ApplicationContextBase : IApplicationContext
    {
        public abstract string Name { get; }
        protected abstract void AddRules(IList<IRule> rules);

        public IContext Check()
        {
            var rules = new List<IRule>();
            AddRules(rules);

            var context = new Context();
            foreach (var rule in rules)
                rule.Check(context);

            return context;
        }

        protected string AskQuestion(string question, string defaultValue = null)
        {
            Console.Write(question);
            if (!string.IsNullOrWhiteSpace(defaultValue))
            {
                var fg = Console.ForegroundColor;
                Console.Write(" (");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(defaultValue);
                Console.ForegroundColor = fg;
                Console.Write(")");
            }
            Console.Write(": ");
            var answer = Console.ReadLine();
            if (string.IsNullOrEmpty(answer)) answer = defaultValue;
            return answer;
        }

    }
}
