using System;
using EasyTranslate.TranslationData;

namespace EasyTranslate.UI.Models
{
    internal class SavedTranslationSequence
    {
        public TranslationSequence SourceTranslationSequence { get; set; }

        public TranslationSequence TranslationSequence { get; set; }

        public TranslateLanguages Language { get; set; }

        public DateTime LastTimeUsed { get; set; }
    }
}