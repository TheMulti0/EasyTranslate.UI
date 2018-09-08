using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyTranslate.UI.ViewModels
{
    internal class PropertyChangedHelper : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;

        public T GetProperty<T>([CallerMemberName] string propertyName = "")
        {
            EnsureElement<T>(propertyName);
            return (T) _values[propertyName];
        }

        public void SetProperty<T>(object value, [CallerMemberName] string propertyName = "")
        {
            EnsureElement<T>(propertyName);
            _values[propertyName] = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void EnsureElement<T>(string propertyName)
        {
            if (!_values.ContainsKey(propertyName))
            {
                _values.Add(propertyName, default(T));
            }
        }
    }
}