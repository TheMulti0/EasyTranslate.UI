using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace EasyTranslate.UI.ViewModels
{
    internal class SuggestionExamplesToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => string.Join(", ", (List<SuggestionExample>) value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}