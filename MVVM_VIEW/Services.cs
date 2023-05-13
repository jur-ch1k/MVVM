using MVVM_VIEWMODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_VIEW {
    internal class ServiceHandler : IServices {
        public string FileName { get; set; }

        public void SaveFile() {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "rawdata";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.ShowDialog();
            FileName = dialog.FileName;
        }

        public void LoadFile() {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "rawdata";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.ShowDialog();
            FileName = dialog.FileName;
        }

        public void ErrorReport(string message) {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBoxResult result;
            MessageBox.Show(message, "Ошибочка", button, icon, MessageBoxResult.Yes);
        }
    }
}
