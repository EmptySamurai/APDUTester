using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace APDUTester.Converters
{
    public class HexByteConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            byte bValue = (byte)value;
            return System.Convert.ToString(bValue, 16);
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string hexString = (string)value;
                hexString = hexString.ToUpper();
                return System.Convert.ToByte(hexString, 16);
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
