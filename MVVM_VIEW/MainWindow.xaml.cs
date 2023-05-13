using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using MVVM_VIEWMODEL;
using MVVM_MODEL;


namespace MVVM_VIEW {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        ViewData viewData = new ViewData(new ServiceHandler());
        public MainWindow() {
            InitializeComponent();
            this.DataContext = viewData;
            //init_type.ItemsSource = Enum.GetValues(typeof(FRawEnum));
        }
    }
}
