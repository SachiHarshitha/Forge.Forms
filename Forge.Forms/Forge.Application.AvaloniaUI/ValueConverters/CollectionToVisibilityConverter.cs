using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Forge.Application.AvaloniaUI.ValueConverters
{
    internal class CollectionToVisibilityConverter : IValueConverter
    {
        public Visibility EmptyValue { get; set; } = Visibility.Hidden;

        public Visibility NotEmptyValue { get; set; } = Visibility.Visible;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumerable = value as IEnumerable<object>;
            if (enumerable == null)
            {
                return this.EmptyValue;
            }

            return enumerable.Any() ? this.NotEmptyValue : this.EmptyValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
