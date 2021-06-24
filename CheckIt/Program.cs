using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CheckIt.Contract;

namespace CheckIt
{
    class Program
    {
        static void Main(string[] args)
        {
            var processor = new ArgumentProcesser(args);
            if (!processor.CanProcess()) return;

            IApplicationContext checklist = null;
            var loader = new ChecklistLoader();

            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine();
            if (processor.ListAvailable)
            {
                processor.WriteAvailableChecklistMessage(loader.AvailableChecklists());
                return;
            }

            var checklistName = processor.ChecklistName.ToLowerInvariant();
            if (loader.AvailableChecklists().Contains(checklistName))
            {
                checklist = loader.GetChecklist(checklistName);
            }
            if (checklist == null)
            {
                processor.WriteMissingChecklistMessage(loader.AvailableChecklists());
                return;
            }

            var result = checklist.Check();

            if (processor.Verbose)
            {
                if (result.HasFailures())
                {
                    WriteInverse("Failures:");
                    foreach (var ruleResult in result.Failures) Write(ruleResult);
                }
                if (result.HasWarnings())
                {
                    WriteInverse("Warnings:");
                    foreach (var ruleResult in result.Warnings) Write(ruleResult);
                }
                if (result.HasInformation())
                {
                    WriteInverse("Information:");
                    foreach (var ruleResult in result.Information) Write(ruleResult);
                }
                Console.ResetColor();
            }

            Console.WriteLine("");
            WriteInverse("Result:");

            var filename = SaveFile(checklistName, result);

            Console.WriteLine(result.Failures.All(r => r.Success)
                ? $"You have a Valid environment for {processor.ChecklistName}"
                : $"Environment check failed. Failures: {result.Failures.Count()}, Warnings: {result.Warnings.Count()}, Information: {result.Information.Count()}.  See the output report for full details ('{filename}').");
        }

        private static string SaveFile(string checklistName, IContext result)
        {
            var dir = Directory.GetCurrentDirectory();
            dir = Path.Combine(dir, "results");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var filename = Path.Combine(dir, $"{checklistName}.{DateTime.Now:yyMMddhhmmss}.txt");
            var file = new FileInfo(filename);
            using (var output = file.CreateText())
            {
                if (result.Failures.All(r => r.Success))
                    Write(output, $"You have a Valid environment for {checklistName}");
                else
                {
                    Write(output, "Environment check failed.");
                    Write(output, $"  Failures: {result.Failures.Count()},");
                    Write(output, $"  Warnings: {result.Warnings.Count()},");
                    Write(output, $"  Information: {result.Information.Count()}.");
                }

                Write(output, "");
                Write(output, "");
                Write(output, "Details:");

                if (result.HasFailures())
                {
                    Write(output, "Failures:");
                    foreach (var ruleResult in result.Failures) Write(output, ruleResult);
                    Write(output, "");
                }
                if (result.HasWarnings())
                {
                    Write(output, "Warnings:");
                    foreach (var ruleResult in result.Warnings) Write(output, ruleResult);
                    Write(output, "");
                }
                if (result.HasInformation())
                {
                    Write(output, "Information:");
                    foreach (var ruleResult in result.Information) Write(output, ruleResult);
                    Write(output, "");
                }

                output.Close(); 
            }
            return file.FullName;
        }

        public static void Write(StreamWriter writer, IRuleResult result)
        {
            writer.WriteLine($" - {result.Rule.Name, -25}: {result.Message}");
            foreach (var fix in result.Fixes)
            {
                var success = fix.Success ? "\u221A" : "x";
                writer.WriteLine($"{new string(' ', 29)} {success} - {fix.Message}");
                foreach (var item in fix.FailedInstructions)
                    writer.WriteLine($"{new string(' ', 29)}   - {item}");
            }
        }

        public static void Write(StreamWriter writer, string message)
        {
            writer.WriteLine(message);
        }

        private static void WriteInverse(string message)
        {
            var c = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;
            Console.BackgroundColor = c;

            Console.WriteLine(message);

            Console.ResetColor();
        }

        private static void Write(IRuleResult result)
        {
            if (result.Success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\u221A ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("x ");
            }

            var t = result.Message;
            var lines = new Queue<string>();
            while (t.Length > 55)
            {
                var max = Math.Min(t.Length, 55);
                var line = t.Substring(0, max);
                lines.Enqueue(line);
                t = t.Substring(max);
            }
            lines.Enqueue(t);

            Console.WriteLine($"{result.Rule.Name,-25}:  {lines.Dequeue()}");
            while (lines.Count > 0)
            {
                Console.WriteLine(new string(' ', 30) + lines.Dequeue());
            }

            Console.ResetColor();
        }
    }
}
