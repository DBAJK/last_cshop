using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApp3.Converter
{
    internal class ErrMsgConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return Binding.DoNothing;

            string retStr = string.Empty;
            foreach (var value in values)
            {
                string input = value as string;
                if (input != null)
                {
                    retStr = retStr + input;
                }
            }

            if(retStr.Equals(""))
            {
                return Binding.DoNothing;
            }
            else
            {
                return retStr;
            }

            
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
