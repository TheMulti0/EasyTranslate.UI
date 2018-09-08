using System.Collections.Generic;

namespace EasyTranslate.UI.ViewModels
{
    internal class Suggestion
    {
        public List<SuggestionExample> Examples { get; set; }

        public string Title { get; set; }

        public Suggestion(string title)
        {
            Examples = new List<SuggestionExample>();
            Title = title;
        }
    }
}