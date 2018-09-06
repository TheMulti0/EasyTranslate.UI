using System.Collections.ObjectModel;

namespace EasyTranslate.UI.ViewModels
{
    internal class Suggestion
    {
        public ObservableCollection<Suggestion> Items { get; set; }

        public string Title { get; set; }

        public Suggestion()
        {

        }

        public Suggestion(string title)
        {
            Items = new ObservableCollection<Suggestion>();
            Title = title;
        }
    }
}