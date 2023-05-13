using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_MODEL {
    public class RawData {
        public double[] Ends { get; set; }
        public int NumOfNodes { get; set; }
        public bool IsUnuform { get; set; }
        public double[] Coord { get; set; }
        public double[] Val { get; set; }
        public FRawEnum Func { get; set; }
        public RawData(double[] ends, int numOfNodes, bool isUnuform, FRawEnum f) {
            Func = f;
            FRaw func = Del_Func.Linear;
            switch (f) {
                case FRawEnum.Linear:
                    func = Del_Func.Linear;
                    break;
                case FRawEnum.Cubic:
                    func = Del_Func.Cubic;
                    break;
                case FRawEnum.Random:
                    func = Del_Func.Random;
                    break;
            }
            Ends = new double[2];
            Ends = ends;
            NumOfNodes = numOfNodes;
            IsUnuform = isUnuform;
            Coord = new double[numOfNodes];
            Val = new double[numOfNodes];
            for (int i = 0; i < numOfNodes - 1; i++) {
                double step = (Ends[1] - Ends[0]) / (NumOfNodes - 1);
                if (isUnuform) {
                    Coord[i] = Ends[0] + step * i;
                }
                else {
                    Coord[i] = new Random().NextDouble() * (Ends[1] - Ends[0]) + Ends[0];
                }
                Val[i] = func(Coord[i]);
            }
            Coord[NumOfNodes - 1] = ends[1];
            Val[NumOfNodes - 1] = func(Coord[NumOfNodes - 1]);
            if (!isUnuform) {
                Coord = Coord.OrderBy(x => x).ToArray();
                Val = Val.OrderBy(x => x).ToArray();
                Coord[0] = Ends[0];
                Val[0] = func(Coord[0]);
            }
        }
        public RawData(string filename) {
            FileStream fileStream = null;
            StreamReader streamReader = null;
            try {
                fileStream = File.OpenRead(filename);
                streamReader = new StreamReader(fileStream);
                Ends = new double[2];
                Ends[0] = Convert.ToDouble(streamReader.ReadLine());
                Ends[1] = Convert.ToDouble(streamReader.ReadLine());
                NumOfNodes = Convert.ToInt32(streamReader.ReadLine());
                IsUnuform = Convert.ToBoolean(streamReader.ReadLine());
                Coord = new double[NumOfNodes];
                Val = new double[NumOfNodes];
                for (int i = 0; i < NumOfNodes; i++) {
                    Coord[i] = Convert.ToDouble(streamReader.ReadLine());
                }
                for (int i = 0; i < NumOfNodes; i++) {
                    Val[i] = Convert.ToDouble(streamReader.ReadLine());
                }
                Func = (FRawEnum)Convert.ToInt32(streamReader.ReadLine());
                int a = 0;
            }
            finally {
                if (streamReader != null)
                    streamReader.Dispose();
                if (fileStream != null)
                    fileStream.Close();
            }
        }
        public void Save(string filename) {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try {
                fileStream = File.OpenWrite(filename);
                streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(Ends[0].ToString());
                streamWriter.WriteLine(Ends[1].ToString());
                streamWriter.WriteLine(NumOfNodes.ToString());
                streamWriter.WriteLine(IsUnuform.ToString());
                for (int i = 0; i < NumOfNodes; i++) {
                    streamWriter.WriteLine(Coord[i].ToString());
                }
                for (int i = 0; i < NumOfNodes; i++) {
                    streamWriter.WriteLine(Val[i].ToString());
                }
                streamWriter.WriteLine(((int)Func).ToString());
            }
            finally {
                if (streamWriter != null)
                    streamWriter.Dispose();
                if (fileStream != null)
                    fileStream.Close();
            }
        }
        static public void Load(string filename, out RawData rawData) {
            rawData = new RawData(filename);
        }
        public double Linear(double x) { return x; }
        public double Cubic(double x) { return x * x * x; }
        public double Random(double x) { return new Random().NextDouble(); }
    }

    public delegate double FRaw(double x);

    public enum FRawEnum {
        Linear = 0,
        Cubic = 1,
        Random = 2,
    }

    static public class Del_Func {
        static public double Linear(double x) { return x; }
        static public double Cubic(double x) { return x * x * x; }
        static public double Random(double x) { return new Random().NextInt64(10) + new Random().NextDouble(); }
    }
}
