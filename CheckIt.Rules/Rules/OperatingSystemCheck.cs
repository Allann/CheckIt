using System;
using System.Collections.Generic;
using System.Linq;
using CheckIt.Contract;
using CheckIt.Services.OSInfo;

namespace CheckIt.Rules
{
    public class OperatingSystemCheck : RuleBase
    {
        private readonly IEnumerable<OsVersion> _validOperatingSystems;
        private readonly bool _is64Bit;

        public OperatingSystemCheck(IEnumerable<OsVersion> validOperatingSystems, bool is64Bit)
        {
            _validOperatingSystems = validOperatingSystems;
            _is64Bit = is64Bit;
        }

        public override string Name => "Operating System";
        public override string Description => "Check if the current Operating System is supported.";
        protected override IEnumerable<IRuleResult> RunRule(IContext context)
        {
            var current = OSVersionService.GetCurrent();

            if (_validOperatingSystems.Any(os => current.OSVersion == os))
            {
                var list = new List<IRuleResult> {new RuleResult(this, true, $"Matched {current}.")};

                var architecture = _is64Bit ? "64Bit" : "32Bit";
                if (Environment.Is64BitOperatingSystem == _is64Bit)
                {
                    list.Add(new RuleResult(this, true, $"Matches the required {architecture} architecture."));
                    return list;
                }
                list.Add(new RuleResult(this, false, $"Didn't match the required {architecture} architecture."));
                return list;
            }
            return new[] {  new RuleResult(this,false, $"{current} is not supported.")};
        }
    }
}
