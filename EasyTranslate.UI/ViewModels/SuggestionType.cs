using System.Collections.Generic;

namespace EasyTranslate.UI.ViewModels
{
    internal class SuggestionType
    {
        public List<Suggestion> Suggestions { get; set; }

        public string Type { get; set; }

        public SuggestionType(string type)
        {
            Suggestions = new List<Suggestion>();
            Type = type;
        }
    }
}