using MVVM_MODEL;

namespace TEST_MODEL {
    public class RawDataTest {

        private string RawFile = "..\\..\\..\\..\\test_data\\model_test\\save_test.txt";
        private string OkFile = "..\\..\\..\\..\\test_data\\model_test\\RawDataTest_OK.txt";
        private double[] Ends = new double[] { 0f, 5f };
        private int NumOfNodes = 6;
        private FRawEnum Func = FRawEnum.Linear;

        [Fact]
        public void SaveTest() {
            RawData raw = new RawData(Ends, NumOfNodes, true, Func);
            raw.Save(RawFile);
            Assert.Equal(new StreamReader(File.OpenRead(RawFile)).ReadToEnd(), new StreamReader(File.OpenRead(OkFile)).ReadToEnd());
        }

        [Fact]
        public void LoadTest() {
            RawData okraw = new RawData(Ends, NumOfNodes, true, Func);
            RawData raw = new RawData(OkFile);
            Assert.Equivalent(okraw, raw);
        }
    }
}