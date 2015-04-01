using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace APDUTester.Converters
{
    public class HexByteArrayConverter : IValueConverter
    {
        IEnumerable<string> Split(string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
                yield return str.Substring(i, chunkSize);
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            string result = "";
            byte[] bytes = (byte[])value;
            foreach (byte b in bytes)
            {
                result += System.Convert.ToString(b, 16) + " ";
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            try
            {
                string hexString = (string)value;
                hexString = hexString.ToUpper();
                hexString = hexString.Replace(" ", "");
                byte[] result = new byte[hexString.Length / 2];
                int i = 0;
                foreach (string b in Split(hexString, 2))
                {
                    result[i] = System.Convert.ToByte(b, 16);
                    i++;
                }
                return result;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
                
    }
}
