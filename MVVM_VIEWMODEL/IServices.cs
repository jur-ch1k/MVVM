using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_VIEWMODEL {
    public interface IServices {
        string FileName { get; set; }
        void SaveFile();
        void LoadFile();
        void ErrorReport(string message);
    }
}
