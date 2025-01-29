using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace quizzer.Converters;
public class BoolToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isCorrect)
        {
            return new SolidColorBrush(isCorrect ? Colors.Green : Colors.Red);
        }

        return new SolidColorBrush(Colors.Transparent);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
