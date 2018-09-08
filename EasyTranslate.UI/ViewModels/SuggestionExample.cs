namespace EasyTranslate.UI.ViewModels
{
    internal class SuggestionExample
    {
        public string Title { get; set; }

        public SuggestionExample(string title)
        {
            Title = title;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}