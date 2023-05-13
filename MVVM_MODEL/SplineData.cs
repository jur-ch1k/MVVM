using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_MODEL {
    public class SplineData {
        public RawData Data { get; set; }
        public int UniformGridNodesNum { get; set; }
        public double LeftSndDer { get; set; }
        public double RightSndDer { get; set; }
        public List<SplineDataItem> SplineList { get; set; }
        public double IntergralVal { get; set; }
        public SplineData(RawData data, double leftsndder, double rightsndder, int uniformgridnodesnum) {
            Data = data;
            UniformGridNodesNum = uniformgridnodesnum;
            LeftSndDer = leftsndder;
            RightSndDer = rightsndder;
            SplineList = new List<SplineDataItem>();
        }
        public void Spline() {
            double step = (Data.Ends[1] - Data.Ends[0]) / (UniformGridNodesNum - 1);

            int nx = Data.NumOfNodes;
            int ny = 1;
            double[] x;
            if (Data.IsUnuform) {
                x = new double[2];
                x[0] = Data.Ends[0];
                x[1] = Data.Ends[1];
            }
            else {
                x = new double[Data.NumOfNodes];
                x = Data.Coord;
            }
            double[] y = new double[Data.NumOfNodes];
            y = Data.Val;
            double[] bc = new double[] { LeftSndDer, RightSndDer };
            double[] scoeff = new double[ny * 4 * (nx - 1)];
            int nsite = UniformGridNodesNum;
            double[] site = new double[UniformGridNodesNum];
            for (int i = 0; i < UniformGridNodesNum - 1; i++) {
                site[i] = Data.Ends[0] + step * i;
            }
            site[UniformGridNodesNum - 1] = Data.Ends[1];
            int ndorder = 3;
            int[] dorder = new int[] { 1, 1, 1 };
            double[] interpolate_result = new double[ny * 3 * nsite];
            int nlim = 1;
            double[] llim = new double[1] { Data.Ends[0] };
            double[] rlim = new double[1] { Data.Ends[1] };
            double[] integral_resault = new double[ny * nlim];
            bool IsUnuform = Data.IsUnuform;
            int[] ret = new int[2] { -1, 0 };

            string a = Environment.CurrentDirectory;

            MKL_func(nx, ny, x, y, bc, scoeff, nsite, site, ndorder, dorder, interpolate_result, nlim, llim, rlim, integral_resault, IsUnuform, ret);

            for (int i = 0; i < UniformGridNodesNum * 3; i += 3) {
                double coord = site[i / 3];
                double val = interpolate_result[i];
                double fstder = interpolate_result[i + 1];
                double sndder = interpolate_result[i + 2];

                SplineList.Add(new SplineDataItem(coord, val, fstder, sndder));
            }

            IntergralVal = integral_resault[0];

        }
        [DllImport("..\\..\\..\\..\\x64\\Debug\\MKL_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
            void MKL_func(
                int nx,
                int ny,
                double[] x,
                double[] y,
                double[] bc,
                double[] scoeff,
                int nsite,
                double[] site,
                int ndorder,
                int[] dorder,
                double[] interpolate_result,
                int nlim,
                double[] llim,
                double[] rlim,
                double[] integral_resault,
                bool IsUnuform,
                int[] ret
            );
    }
}
