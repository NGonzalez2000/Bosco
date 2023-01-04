using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bosco.XAML.Converters
{
    class CUITConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "00-00000000-0";
            string cuil = (string)value;
            cuil = StrReverse(cuil);
            while (cuil.Length < 11) cuil += "0";
            cuil = StrReverse(cuil);
            return $"{cuil[..2]}-{cuil[2..10]}-{cuil[^1..]}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "00000000000";
            string cuil = (string)value;
            cuil = cuil.TrimStart('0').Replace("-", "");
            if (cuil.Length == 12) cuil = cuil.Remove(11);
            cuil = StrReverse(cuil);
            while (cuil.Length < 11) cuil += "0";
            return StrReverse(cuil);
        }
        private static string StrReverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
