using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EasyTranslate.TranslationData;

namespace EasyTranslate.UI.ViewModels
{
    internal class MainWindowViewModel : PropertyChangedHelper
    {
        private readonly (int width, int height) _defaultWindowSize;
        private readonly (int width, int height) _minimalWindowSize;

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

        public bool IsTopmost
        {
            get => GetProperty<bool>();
            set
            {
                SetProperty<bool>(value);

                if (Application.Current.MainWindow == null)
                {
                    return;
                }
                if (value)
                {
                    Application.Current.MainWindow.Width = _minimalWindowSize.width;
                    Application.Current.MainWindow.Height = _minimalWindowSize.height;
                }
                else
                {
                    Application.Current.MainWindow.Width = _defaultWindowSize.width;
                    Application.Current.MainWindow.Height = _defaultWindowSize.height;
                }
            }
        }

        public MainWindowViewModel()
        {
            _defaultWindowSize = (600, 400);
            _minimalWindowSize = (300, 300);

            Languages = Enum.GetValues(typeof(TranslateLanguages))
                            .Cast<TranslateLanguages>();
            ProgressVisibility = Visibility.Collapsed;
            IsTopmost = false;
        }
    }
}