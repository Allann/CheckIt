using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckIt
{
    public class ArgumentProcesser
    {
        public ArgumentProcesser(string[] args)
        {
            Parse(args);
        }

        public bool CanProcess()
        {
            return !RequireHelp;
        }

        private void Parse(string[] args)
        {
            if (!args.Any())
            {
                WriteHelpMessage();
                return;
            }

            RequireHelp = false;
            foreach (var arg in args)
            {
                if (arg.StartsWith("-") || arg.StartsWith("/"))
                {
                    var option = arg.Substring(1);
                    if (string.Equals(option, "help", StringComparison.InvariantCultureIgnoreCase) || string.Equals(option, "h", StringComparison.InvariantCultureIgnoreCase))
                    {
                        RequireHelp = true;
                    }
                    else if (string.Equals(option, "list", StringComparison.InvariantCultureIgnoreCase) || string.Equals(option, "l", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ListAvailable = true;
                    }
                    else if (string.Equals(option, "verbose", StringComparison.InvariantCultureIgnoreCase) || string.Equals(option, "v", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Verbose = true;
                    }
                }
                else
                {
                    ChecklistName = arg;
                }
            }

            if (string.IsNullOrWhiteSpace(ChecklistName) && !ListAvailable)
            {
                RequireHelp = true;
            }
            if (RequireHelp)
                WriteHelpMessage();
        }


        private void DisplayError(string error)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(error);

            Console.ResetColor();
        }

        public string ChecklistName { get; private set; }
        public bool RequireHelp { get; private set; } = true;
        public bool ListAvailable { get; private set; }
        public bool Verbose { get; private set; }

        private void WriteHelpMessage()
        {
            Console.WriteLine("Help:");
            Console.WriteLine("    CheckIt application -verbose -help -list");
            Console.WriteLine("");
            Console.WriteLine("Where:");
            Console.WriteLine("    application = the application context checklist to run (use -list first to see valid contexts).");
            Console.WriteLine("    verbose     = will output all tests to the console");
            Console.WriteLine("    help        = this help message");
            Console.WriteLine("    list        = list the available checklists");
            Console.WriteLine("");
            Console.WriteLine("Examples:");
            Console.WriteLine("    CheckIt MyWebsite");
            Console.WriteLine("    CheckIt \"My Website\"");
            Console.WriteLine("    CheckIt MyApplication -verbose");
            Console.WriteLine("    CheckIt -help");
            Console.WriteLine("    CheckIt -list");
        }

        public void WriteMissingChecklistMessage(IEnumerable<string> availableChecklists)
        {
            var checklistNames = availableChecklists as IList<string> ?? availableChecklists?.ToList();

            if (checklistNames == null || !checklistNames.Any())
            {
                DisplayError("Unable to find any context checklists in the current directory");
            }
            else
            {
                Console.WriteLine($"The context '{ChecklistName}' was not found.");
                Console.WriteLine("Available context checklists include:");
                foreach (var checklistName in checklistNames)
                {
                    Console.WriteLine($" o {checklistName}");
                }
            }
        }

        public void WriteAvailableChecklistMessage(IEnumerable<string> availableChecklists)
        {
            var checklistNames = availableChecklists as IList<string> ?? availableChecklists?.ToList();

            if (checklistNames == null || !checklistNames.Any())
            {
                DisplayError("Unable to find any context checklists in the current directory");
            }
            else
            {
                Console.WriteLine("Available context checklists:");
                foreach (var checklistName in checklistNames)
                {
                    Console.WriteLine($" o {checklistName}");
                }
            }
        }
    }
}
