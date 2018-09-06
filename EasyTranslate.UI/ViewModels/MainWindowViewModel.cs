using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using EasyTranslate.TranslationData;
using EasyTranslate.UI.Annotations;

namespace EasyTranslate.UI.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _result;
        private IEnumerable<Suggestion> _suggestions;
        private bool _isLoading;
        private Visibility _progressVisibility;

        public string Text { get; set; }

        public IEnumerable<TranslateLanguages> Languages { get; set; }

        public TranslateLanguages Language { get; set; }

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Suggestion> Suggestions
        {
            get => _suggestions;
            set
            {
                _suggestions = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
                ProgressVisibility = value 
                    ? Visibility.Visible 
                    : Visibility.Collapsed;
            }
        }

        public Visibility ProgressVisibility
        {
            get => _progressVisibility;
            set
            {
                _progressVisibility = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            Languages = Enum.GetValues(typeof(TranslateLanguages))
                            .Cast<TranslateLanguages>();
            ProgressVisibility = Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}