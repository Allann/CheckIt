using System;
using System.Collections.Generic;
using System.IO;
using CheckIt.Contract;

namespace CheckIt.Fixes
{
    public class CreateDirectoryFix : IFix
    {
        private string _failException;

        public string Path { get; }

        public CreateDirectoryFix(string path)
        {
            Path = path;
        }

        public bool Manual => false;
        public bool Perform(IRuleResult result)
        {
            var directory = new DirectoryInfo(Path);
            if (directory.Exists) return true;

            try
            {
                directory.Create();
                return true;
            }
            catch (Exception e)
            {
                _failException = e.Message;
                return false;
            }
        }

        IEnumerable<IInstruction> IFix.Instructions => new[] {new Instruction($"Create directory: {Path}")};

        public string PassResponse => $"{Path} created.";
        public string FailReponse => $"Failed to create path {Path}. {_failException}";
    }
}
