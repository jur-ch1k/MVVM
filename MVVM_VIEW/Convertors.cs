using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using MVVM_VIEWMODEL;
using MVVM_MODEL;

namespace MVVM_VIEW {
    public class ArrayToStrConverter : IMultiValueConverter {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture) {
            return string.Join(" ", value);
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture) {
            double[] ders = new double[0];
            try {
                string val = (string)value;
                ders = Array.ConvertAll(val.Split(' '), double.Parse);
            }
            catch {
                string messageBoxText = "Введите все требуемые значения!";
                string caption = "Ошибочка";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
            return ders.Cast<object>().ToArray();
        }
    }

    public class BoolConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }
    }
    public class SplineListConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string str = "";
            if (value != null) {
                str = ((SplineDataItem)value).ToString("0.000");
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }
    }

    public class CoordToSting : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return "Coord = " + ((double)value).ToString("0.000");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }
    }
    public class ValToSting : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return "Val = " + ((double)value).ToString("0.000");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }
    }
    public class SndDerToSting : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return "SndDer = " + ((double)value).ToString("0.000");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }
    }
    public class IntegralValFormat : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return ((double)value).ToString("0.000");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }
    }
}
