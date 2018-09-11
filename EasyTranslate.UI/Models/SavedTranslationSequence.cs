using System;
using EasyTranslate.TranslationData;

namespace EasyTranslate.UI.Models
{
    internal class SavedTranslationSequence
    {
        public TranslationSequence SourceTranslationSequence { get; set; }

        public TranslateLanguages SourceLanguage { get; set; }

        public TranslationSequence TranslationSequence { get; set; }

        public DateTime TimeSaved { get; set; }
    }
}