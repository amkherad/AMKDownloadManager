using System;
using System.Globalization;
using Avalonia.Markup;
using DrawingSize = System.Drawing.Size;
using AvalonSize = Avalonia.Size;

namespace AMKDownloadManager.UI.AvaloniaUI.Converters
{
    public class DrawingSizeToAvalonSize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (!(value is DrawingSize drawingSize))
            {
                if (value is AvalonSize)
                    return value;
                return null;
            }

            return new AvalonSize(drawingSize.Width, drawingSize.Height);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (!(value is AvalonSize avalonSize))
            {
                if (value is DrawingSize)
                    return value;
                return null;
            }

            return new DrawingSize((int)avalonSize.Width, (int)avalonSize.Height);
        }
    }
}