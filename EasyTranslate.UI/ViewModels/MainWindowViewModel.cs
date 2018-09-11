using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EasyTranslate.TranslationData;

namespace EasyTranslate.UI.ViewModels
{
    internal class MainWindowViewModel : PropertyChangedHelper
    {
        public IEnumerable<TranslateLanguages> Languages { get; set; }

        public string Text
        {
            get => GetProperty<string>();
            set => SetProperty<string>(value);
        }

        public TranslateLanguages Language
        {
            get => GetProperty<TranslateLanguages>();
            set => SetProperty<TranslateLanguages>(value);
        }

        public string Result
        {
            get => GetProperty<string>();
            set => SetProperty<string>(value);
        }

        public IEnumerable<SuggestionType> Suggestions
        {
            get => GetProperty<IEnumerable<SuggestionType>>();
            set => SetProperty<IEnumerable<SuggestionType>>(value);
        }

        public bool IsLoading
        {
            get => GetProperty<bool>();
            set
            {
                SetProperty<bool>(value);
                ProgressVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility ProgressVisibility
        {
            get => GetProperty<Visibility>();
            set => SetProperty<Visibility>(value);
        }

        public MainWindowViewModel()
        {
            Languages = Enum.GetValues(typeof(TranslateLanguages))
                            .Cast<TranslateLanguages>();
            ProgressVisibility = Visibility.Collapsed;
        }
    }
}