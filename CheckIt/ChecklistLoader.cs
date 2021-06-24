using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using CheckIt.Contract;

namespace CheckIt
{
    public class ChecklistLoader
    {
        private IReadOnlyCollection<IApplicationContext> _contexts;

        private static IReadOnlyCollection<string> GetApplicationContexts(string path)
        {
            var dir = new DirectoryInfo(path);
            var files = dir.EnumerateFiles("*.dll")
                .Select(file => Assembly.LoadFrom(file.FullName))
                .Where(checklist => checklist.ExportedTypes.Any(IsApplicationContext))
                .ToList();
            var paths = files.Select(f => f.Location);
            return new ReadOnlyCollection<string>(paths.ToList());
        }

        private static bool IsApplicationContext(Type type)
        {
            return type.IsClass && !type.IsAbstract && type.GetInterface("IApplicationContext", true) != null;
        }

        public void Refresh()
        {
            var path = Directory.GetCurrentDirectory();
            var filesWithChecklists = GetApplicationContexts(path);

            var contexts = new List<IApplicationContext>();
            foreach (var filePath in filesWithChecklists)
            {
                if (string.IsNullOrWhiteSpace(filePath)) continue;
                var loaded = Assembly.LoadFrom(filePath);
                if (loaded == null) continue;

                var contextTypes = loaded.ExportedTypes.Where(IsApplicationContext);
                contexts.AddRange(contextTypes.Select(type => loaded.CreateInstance(type.FullName)).OfType<IApplicationContext>());
            }
            _contexts = new ReadOnlyCollection<IApplicationContext>(contexts);
        }

        public IEnumerable<string> AvailableChecklists()
        {
            if (_contexts == null) Refresh();

            return _contexts.Select(c => c.Name.ToLowerInvariant());
        }

        public IApplicationContext GetChecklist(string checklistName)
        {
            return _contexts.FirstOrDefault(c => string.Equals(c.Name, checklistName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}