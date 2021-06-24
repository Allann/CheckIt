using CheckIt.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckIt.Fixes
{
    public class InstallDotNetRuntime : IFix
    {
        public bool Manual => true;

        public IEnumerable<IInstruction> Instructions
        {
            get
            {
                var list = new List<IInstruction>
                {
                    new Instruction("Browse to http://dot.net", false, true),
                    new Instruction("Click Downloads.", false, true),
                    new Instruction("Click .NET Core.",false,true),
                    new Instruction("Select Current and SDK.", false, true),
                    new Instruction("Click x64 Windows installer"),
                };
                return list;
            }
        }

        public bool Perform(IRuleResult ruleResult)
        {
            throw new NotImplementedException();
        }

        public string PassResponse => "Installed .NET Core Runtime successfully.";
        public string FailReponse => "Failed to install .NET Core Runtime.";
    }
}
