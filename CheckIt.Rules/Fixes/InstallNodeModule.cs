using System;
using System.Collections.Generic;
using CheckIt.Contract;

namespace CheckIt.Fixes
{
    public class InstallNodeModule : IFix
    {
        public InstallNodeModule(string moduleName)
        {
            ModuleName = moduleName;
        }
        public bool Manual => true;

        public IEnumerable<IInstruction> Instructions
        {
            get
            {
                var list = new List<IInstruction>
                {
                    new Instruction("Open Command Prompt", false, true),
                    new Instruction($"type 'npm install -g {ModuleName}", false, true),
                    new Instruction("Installed")
                };
                return list;
            }
        }

        public bool Perform(IRuleResult ruleResult)
        {
            throw new NotImplementedException();
        }

        public string PassResponse => $"Installed node module '{ModuleName}' successfully.";
        public string FailReponse => $"Failed to install node module '{ModuleName}'.";

        public string ModuleName { get; }
    }
}