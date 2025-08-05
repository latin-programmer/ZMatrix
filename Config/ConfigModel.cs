using System;
using System.Collections.Generic;

namespace ZMatrixConfig
{
    public class ConfigModel
    {
        public int MaxStream { get; set; } = 1000;
        public int SpeedVariance { get; set; } = 5;
        public bool MonotonousCleanupEnabled { get; set; }
        public int BackTrace { get; set; } = 40;
        public bool RandomizedCleanupEnabled { get; set; }
        public int Leading { get; set; } = 10;
        public int SpacePad { get; set; } = 30;
        public int RefreshTime { get; set; } = 50;
        public string PriorityClass { get; set; } = "IDLE_PRIORITY_CLASS";
        public double SpecialStringStreamProbability { get; set; } = 0.1;

        public static ConfigModel FromIni(IniFile ini)
        {
            var model = new ConfigModel();
            if (!ini.Sections.TryGetValue("General", out var general))
                general = new Dictionary<string, string>();

            int GetInt(string key, int defaultValue) => general.TryGetValue(key, out var v) && int.TryParse(v, out var i) ? i : defaultValue;
            bool GetBool(string key, bool defaultValue) => general.TryGetValue(key, out var v) && (v == "1" || v.Equals("true", StringComparison.OrdinalIgnoreCase));
            double GetDouble(string key, double defaultValue) => general.TryGetValue(key, out var v) && double.TryParse(v, out var d) ? d : defaultValue;
            string GetString(string key, string defaultValue) => general.TryGetValue(key, out var v) ? v : defaultValue;

            model.MaxStream = GetInt("MaxStream", model.MaxStream);
            model.SpeedVariance = GetInt("SpeedVariance", model.SpeedVariance);
            model.MonotonousCleanupEnabled = GetBool("MonotonousCleanupEnabled", model.MonotonousCleanupEnabled);
            model.BackTrace = GetInt("BackTrace", model.BackTrace);
            model.RandomizedCleanupEnabled = GetBool("RandomizedCleanupEnabled", model.RandomizedCleanupEnabled);
            model.Leading = GetInt("Leading", model.Leading);
            model.SpacePad = GetInt("SpacePad", model.SpacePad);
            model.RefreshTime = GetInt("RefreshTime", model.RefreshTime);
            model.PriorityClass = GetString("PriorityClass", model.PriorityClass);
            model.SpecialStringStreamProbability = GetDouble("SpecialStringStreamProbability", model.SpecialStringStreamProbability);
            return model;
        }

        public void Apply(IniFile ini)
        {
            if (!ini.Sections.ContainsKey("General"))
                ini.Sections["General"] = new Dictionary<string, string>();
            var general = ini.Sections["General"];
            general["MaxStream"] = MaxStream.ToString();
            general["SpeedVariance"] = SpeedVariance.ToString();
            general["MonotonousCleanupEnabled"] = MonotonousCleanupEnabled ? "1" : "0";
            general["BackTrace"] = BackTrace.ToString();
            general["RandomizedCleanupEnabled"] = RandomizedCleanupEnabled ? "1" : "0";
            general["Leading"] = Leading.ToString();
            general["SpacePad"] = SpacePad.ToString();
            general["RefreshTime"] = RefreshTime.ToString();
            general["PriorityClass"] = PriorityClass;
            general["SpecialStringStreamProbability"] = SpecialStringStreamProbability.ToString("G");
        }
    }
}
