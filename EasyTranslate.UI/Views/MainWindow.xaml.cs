using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EasyTranslate.Exceptions;
using EasyTranslate.TranslationData;
using EasyTranslate.Translators;
using EasyTranslate.UI.Models;
using EasyTranslate.UI.ViewModels;

namespace EasyTranslate.UI.Views
{
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _vm;
        private readonly JsonParser _jsonParser;
        private CancellationTokenSource _cts;

        public MainWindow()
        {
            InitializeComponent();

            _vm = DataContext as MainWindowViewModel;
            _jsonParser = new JsonParser();

            _jsonParser.DeserializeSequencesAsync();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            SavedTranslationSequence last = _jsonParser.Cache.LastOrDefault();

            _vm.Text = last?.SourceTranslationSequence.Sequence;

            TranslationSequence lastTranslationSequence = last?.TranslationSequence;

            _vm.Result = lastTranslationSequence?.Sequence;
            _vm.Suggestions = await GetSuggestions(lastTranslationSequence);

            _vm.Language = last?.Language ?? TranslateLanguages.Afrikaans;

            await Task.Run(() => _jsonParser.Cache.RemoveAll(saved =>
            {
                TimeSpan timeSpan = DateTime.Now - saved.LastTimeUsed;
                return timeSpan.Days >= 7;
            }));

        }

        private void OnClosing(object sender, EventArgs e)
        {
            if (_jsonParser.Cache.Count >= 1)
            {
                _jsonParser.Cache.LastOrDefault().Language = _vm.Language;
            }
            _jsonParser.SerializeSequences();
        }

        private async void OnTextChanged(object sender, TextChangedEventArgs e) 
            => await Translate();

        private async void OnLanguageChanged(object sender, SelectionChangedEventArgs e) 
            => await Translate();

        private async Task Translate()
        {
            if (_vm == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(_vm.Text))
            {
                return;
            }

            try
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();

                ITranslator translator = new GoogleTranslator();
                var sequence = new TranslationSequence(_vm.Text);
                _vm.IsLoading = true;

                SavedTranslationSequence saved = await ReadCache(sequence);
                TranslationSequence result;
                if (saved == null)
                {
                    result = await translator.TranslateAsync(sequence, _vm.Language, _cts.Token);
                    saved = new SavedTranslationSequence
                    {
                        SourceTranslationSequence = sequence,
                        TranslationSequence = result,
                        Language = _vm.Language
                    }; 
                }
                else
                {
                    result = saved.TranslationSequence;
                }

                _vm.Result = result.Sequence;
                _vm.Suggestions = await GetSuggestions(result);

                saved.LastTimeUsed = DateTime.Now;

                _jsonParser.Cache.Add(saved);
            }
            catch (TranslationFailedException)
            {
            }
            _vm.IsLoading = false;
        }

        private Task<SavedTranslationSequence> ReadCache(TranslationSequence sequence)
        {
            SavedTranslationSequence Enumerate()
            {
                SavedTranslationSequence savedSequence = null;

                Parallel.ForEach(_jsonParser.Cache, (saved, state) =>
                {
                    if (saved.SourceTranslationSequence.Sequence != sequence.Sequence ||
                        saved.Language != _vm.Language)
                    {
                        return;
                    }
                    saved.LastTimeUsed = DateTime.Now;
                    savedSequence = saved;
                    state.Stop();
                });
                return savedSequence;
            }

            return Task.Run((Func<SavedTranslationSequence>) Enumerate);
        }

        private async Task<IEnumerable<SuggestionType>> GetSuggestions(TranslationSequence result)
        {
            IEnumerable<SuggestionType> suggestions = new List<SuggestionType>();
            await Task.Run(() =>
            {
                try
                {
                    suggestions = ExtractSuggestions(result);
                }
                catch
                {
                    // ignored
                }
            });
            return suggestions;
        }

        private IEnumerable<SuggestionType> ExtractSuggestions(TranslationSequence result)
        {
            var types = new List<SuggestionType>();
            foreach (ExtraTranslation extra in result.Suggestions)
            {
                var type = extra.Type.ToString();
                bool IsTypeEqual(SuggestionType item) => item.Type == type;

                SuggestionType belongType = types.Any(IsTypeEqual)
                    ? types.Find(IsTypeEqual)
                    : new SuggestionType(type);

                var suggestion = new Suggestion(extra.Name);
                Parallel.ForEach(extra.Words, extraWord => suggestion.Examples.Add(new SuggestionExample(extraWord)));

                _cts.Token.ThrowIfCancellationRequested();

                if (suggestion.Examples.Count > 0)
                {
                    belongType.Suggestions.Add(suggestion);
                }
                int index = types.IndexOf(types.Find(IsTypeEqual));
                if (index > -1)
                {
                    types[index] = belongType;
                }
                else
                {
                    types.Add(belongType);
                }
            }
            return types;
        }
    }
}