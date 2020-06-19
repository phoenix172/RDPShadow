using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace RDPShadow
{
    class LoadingStatusImageConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((LoadingStatus)value)
            {
                case LoadingStatus.Failed:
                    return "Images\\failed.png";
                case LoadingStatus.Loading:
                    return "Images\\loading.png";
                case LoadingStatus.Done:
                    return "Images\\done.png";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
