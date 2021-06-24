using System;
using System.Collections.Generic;
using CheckIt.Contract;

namespace CheckIt.Fixes
{
    public class InstallHostBundle: IFix
    {
        public bool Manual => true;

        public IEnumerable<IInstruction> Instructions
        {
            get
            {
                var list = new List<IInstruction>
                {
                    new Instruction("Browse to https://docs.microsoft.com/en-us/aspnet/core/publishing/iis", false, true),
                    new Instruction("Follow instructions to ensure IIS is installed correctly.", false, true),
                    new Instruction("Download from https://aka.ms/dotnetcore_windowshosting_1_1_0"),
                    new Instruction("Install"),
                    new Instruction("Restart Server (net stop was /y is not enough)")
                };
                return list;
            }
        }

        public bool Perform(IRuleResult ruleResult)
        {
            throw new NotImplementedException();
        }

        public string PassResponse => "Installed Hosting Bundle successfully.";
        public string FailReponse => "Failed to install Hosting Bundle.";
    }
}
