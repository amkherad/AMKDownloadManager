using System;
using System.Globalization;
using Avalonia.Markup;
using DrawingPoint = System.Drawing.Point;
using AvalonPoint = Avalonia.Point;

namespace AMKDownloadManager.UI.AvaloniaUI.Converters
{
    public class DrawingPointToAvalonPoint : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (!(value is DrawingPoint drawingPoint))
            {
                if (value is AvalonPoint)
                    return value;
                return null;
            }
            
            return new AvalonPoint(drawingPoint.X, drawingPoint.Y);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (!(value is AvalonPoint avalonPoint))
            {
                if (value is DrawingPoint)
                    return value;
                return null;
            }

            return new DrawingPoint((int)avalonPoint.X, (int)avalonPoint.Y);
        }
    }
}