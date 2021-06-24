using System.Collections.Generic;
using System.IO;
using CheckIt.Contract;
using CheckIt.Fixes;

namespace CheckIt.Rules
{
    public class FolderCheck : RuleBase
    {
        private readonly DirectoryInfo _directory;

        public FolderCheck(DirectoryInfo directory)
        {
            _directory = directory;
        }

        public override string Name => "Directory";
        public override string Description => "Check for existance of a directory.";
        protected override IEnumerable<IRuleResult> RunRule(IContext context)
        {
            if (_directory.Exists)
                return new[] { new RuleResult(this, true, $"'{_directory.FullName}' found.") };

            var result = new List<IRuleResult>();
            var r = new RuleResult(this, false, $"Missing directory: {_directory.FullName}");
            r.AddFix(new CreateDirectoryFix(_directory.FullName));
            result.Add(r);

            return result;
        }
    }
}
