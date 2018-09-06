using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
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
                    // ignored
                }
            }
            catch (TranslationFailedException)
            {
            }
            _vm.IsLoading = false;
        }

        private static IEnumerable<Suggestion> GetSuggestions(TranslationSequence result)
        {
            List<Suggestion> types = new List<Suggestion>();
            foreach (ExtraTranslation extra in result.Suggestions)
            {
                string type = extra.Type.ToString();
                Suggestion belongType = types.Any(item => item.Title == type)
                    ? types.Find(item => item.Title == type)
                    : new Suggestion(type);

                Suggestion suggestion = null;
                foreach (string extraWord in extra.Words)
                {
                    suggestion = new Suggestion(extra.Name);
                    suggestion.Items.Add(new Suggestion(extraWord));
                }
                if (suggestion != null)
                {
                    belongType.Items.Add(suggestion);
                }
                types.Add(belongType);
            }
            return types;
        }
    }
}