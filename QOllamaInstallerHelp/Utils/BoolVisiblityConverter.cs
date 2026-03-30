using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace QOllamaInstallerHelp.Utils
{
    public class BoolVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isRever = parameter == null ? false : true;
            if (value is bool b)
            {
                b = isRever ? !b : b;
                var val = b ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                return val;
            }
            throw new("此类型不能作为 Bool->V BoolVisibilityConverter的参数 value=" + value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isRever = parameter == null ? false : true;
            if (value is Visibility v)
            {
                var val = v == Visibility.Visible;
                return isRever ? !val : val;
            }
            throw new("此类型不能作为 V->Bool BoolVisibilityConverter的参数 value=" + value);
        }
    }
}
