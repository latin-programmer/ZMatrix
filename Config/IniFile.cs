using System;
using System.Collections.Generic;
using System.IO;

namespace ZMatrixConfig
{
    public class IniFile
    {
        public Dictionary<string, Dictionary<string, string>> Sections { get; } = new(StringComparer.OrdinalIgnoreCase);

        public static IniFile Load(string path)
        {
            var ini = new IniFile();
            string currentSection = string.Empty;
            if (!File.Exists(path))
                return ini;
            foreach (var rawLine in File.ReadAllLines(path))
            {
                var line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith(";"))
                    continue;
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = line.Substring(1, line.Length - 2);
                    if (!ini.Sections.ContainsKey(currentSection))
                        ini.Sections[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    int idx = line.IndexOf('=');
                    if (idx > 0)
                    {
                        var key = line.Substring(0, idx).Trim();
                        var value = line.Substring(idx + 1).Trim();
                        if (!ini.Sections.ContainsKey(currentSection))
                            ini.Sections[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        ini.Sections[currentSection][key] = value;
                    }
                }
            }
            return ini;
        }

        public void Save(string path)
        {
            using var writer = new StreamWriter(path);
            foreach (var section in Sections)
            {
                writer.WriteLine($"[{section.Key}]");
                foreach (var kv in section.Value)
                {
                    writer.WriteLine($"{kv.Key}={kv.Value}");
                }
            }
        }
    }
}
