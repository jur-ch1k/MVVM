using MVVM_MODEL;

namespace TEST_MODEL {
    public class SplineDataTest {

        private double[] Ends = new double[] { 0f, 5f };
        private int NumOfNodes = 6;
        private FRawEnum Func = FRawEnum.Linear;

        [Fact]
        public void MKLSplineTest() {
            RawData raw = new RawData(Ends, NumOfNodes, true, Func);
            SplineData splinedata = new SplineData(raw, 0, 0, 11);
            splinedata.Spline();
            double[] coord_vals = new double[] { 0, 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5 };
            for (int i = 0; i < 11; i++) {
                Assert.InRange<double>(splinedata.SplineList[i].FstDer, 0.99, 1.01);
                Assert.InRange<double>(splinedata.SplineList[i].SndDer, -0.01, 0.01);
                Assert.InRange<double>(splinedata.SplineList[i].Coord, coord_vals[i] - 0.01, coord_vals[i] + 0.01);
                Assert.InRange<double>(splinedata.SplineList[i].Val, coord_vals[i] - 0.01, coord_vals[i] + 0.01);
            }

        }
    }
}
