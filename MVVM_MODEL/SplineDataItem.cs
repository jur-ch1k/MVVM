using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_MODEL {
    public class SplineDataItem {
        public double Coord { get; set; }
        public double Val { get; set; }
        public double FstDer { get; set; }
        public double SndDer { get; set; }
        public SplineDataItem(double coord, double val, double fstder, double sndder) {
            Coord = coord;
            Val = val;
            FstDer = fstder;
            SndDer = sndder;
        }
        public string ToString(string fortam) {
            return $"Coord = {Coord.ToString(fortam)}\nVal = {Val.ToString(fortam)}\nFstder = {FstDer.ToString(fortam)}\nSndder = {SndDer.ToString(fortam)}";
        }
        public override string ToString() {
            return $"{Coord}\n{Val}\n{FstDer}\n{SndDer}";
        }
    }
}
