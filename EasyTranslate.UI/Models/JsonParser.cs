using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EasyTranslate.UI.Models
{
    internal class JsonParser
    {
        public AppSettings Settings { get; set; }

        public List<SavedTranslationSequence> Cache { get; set; }

        public string SettingsPath { get; set; }

        public string CachePath { get; set; }

        public JsonParser()
        {
            Settings = new AppSettings();
            Cache = new List<SavedTranslationSequence>();
            (SettingsPath, CachePath) = GetPath();
        }

        public (string settings, string cache) GetPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folderPath = Path.Combine(appData, @"EasyTranslate.UI\");

            string settingsPath = Path.Combine(folderPath, "settings.json");
            string cachePath = Path.Combine(folderPath, "cache.json");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return (settingsPath, cachePath);
        }

        public void SerializeSequences()
        {
            string settingsJson = JsonConvert.SerializeObject(Settings, Formatting.Indented);
            File.WriteAllText(SettingsPath, settingsJson);

            string cacheJson = JsonConvert.SerializeObject(Cache, Formatting.Indented);
            File.WriteAllText(CachePath, cacheJson);
        }

        public void DeserializeSequencesAsync()
        {
            try
            {
                string settingsJson = File.ReadAllText(SettingsPath);
                Settings = JsonConvert.DeserializeObject<AppSettings>(settingsJson);

                string cacheJson = File.ReadAllText(CachePath);
                Cache = JsonConvert.DeserializeObject<List<SavedTranslationSequence>>(cacheJson);
            }
            catch
            {
                //File does not exist or deserialization failed
            }
        }
    }
}