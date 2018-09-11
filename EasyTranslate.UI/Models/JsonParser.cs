using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EasyTranslate.UI.Models
{
    internal class JsonParser
    {
        public List<SavedTranslationSequence> Cache { get; set; }

        public string CachePath { get; set; }

        public JsonParser()
        {
            Cache = new List<SavedTranslationSequence>();
            CachePath = GetPath();
        }

        public string GetPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folderPath = Path.Combine(appData, @"EasyTranslate.UI\");

            string cachePath = Path.Combine(folderPath, "cache.json");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return cachePath;
        }

        public void SerializeSequences()
        {
            string settingsJson = JsonConvert.SerializeObject(Cache, Formatting.Indented);
            File.WriteAllText(CachePath, settingsJson);
        }

        public void DeserializeSequencesAsync()
        {
            try
            {
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