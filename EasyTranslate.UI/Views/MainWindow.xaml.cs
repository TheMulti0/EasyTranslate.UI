using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using EasyTranslate.Exceptions;
using EasyTranslate.TranslationData;
using EasyTranslate.Translators;
using EasyTranslate.UI.ViewModels;

namespace EasyTranslate.UI.Views
{
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _vm;
        private CancellationTokenSource _cts;

        public MainWindow()
        {
            InitializeComponent();
            _vm = DataContext as MainWindowViewModel;
        }

        private async void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            ITranslator translator = new GoogleTranslator();
            var word = new TranslationSequence(_vm.Text);
            
            try
            {
                _vm.IsLoading = true;
                TranslationSequence result = await translator.TranslateAsync(word, _vm.Language, _cts.Token);
                _vm.Result = result.Sequence;

                try
                {
                    _vm.Suggestions = GetSuggestions(result);
                }
                catch
                {
                    _vm.Suggestions = new List<SuggestionType>();
                }
            }
            catch (TranslationFailedException)
            {
            }
            _vm.IsLoading = false;
        }

        private static IEnumerable<SuggestionType> GetSuggestions(TranslationSequence result)
        {
            void GetList(ExtraTranslation extra, List<SuggestionType> suggestionTypes)
            {
                string type = extra.Type.ToString();
                bool IsTypeEqual(SuggestionType item) => item.Type == type;

                SuggestionType belongType = suggestionTypes.Any(IsTypeEqual) 
                        ? suggestionTypes.Find(IsTypeEqual) 
                        : new SuggestionType(type);

                var suggestion = new Suggestion(extra.Name);
                Parallel.ForEach(extra.Words, extraWord => suggestion.Examples.Add(new SuggestionExample(extraWord)));

                if (suggestion.Examples.Count > 0)
                {
                    belongType.Suggestions.Add(suggestion);
                }
                int index = suggestionTypes.IndexOf(suggestionTypes.Find(IsTypeEqual));
                if (index > -1)
                {
                    suggestionTypes[index] = belongType;
                }
                else
                {
                    suggestionTypes.Add(belongType);
                }
            }

            List<SuggestionType> types = new List<SuggestionType>();
            Parallel.ForEach(result.Suggestions, extra => GetList(extra, types));
            return types;
        }
    }
}