using System;
using System.Collections.Generic;
using CheckIt.Contract;

namespace CheckIt.Fixes
{
    public class InstallNode : IFix
    {
        public bool Manual => true;

        public IEnumerable<IInstruction> Instructions
        {
            get
            {
                var list = new List<IInstruction>
                {
                    new Instruction("Browse to https://nodejs.org/en/download/", false, true),
                    new Instruction("Click LTS and Windows Installer.", false, true),
                    new Instruction("Save and Run the installer.")
                };
                return list;
            }
        }

        public bool Perform(IRuleResult ruleResult)
        {
            throw new NotImplementedException();
        }

        public string PassResponse => "Installed nodejs successfully.";
        public string FailReponse => "Failed to install nodejs.";
    }
}