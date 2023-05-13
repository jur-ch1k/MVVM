using MVVM_MODEL;
using MVVM_VIEWMODEL;

namespace TEST_VIEWMODEL {

    public class ViewModelTest {

        string RawFile = "..\\..\\..\\..\\test_data\\viewmodel_test\\save_test.txt";
        string OkFile = "..\\..\\..\\..\\test_data\\viewmodel_test\\RawDataTest_OK.txt";
        private double[] Ends = new double[] { 0f, 5f };
        private int NumOfNodes = 6;
        private FRawEnum Func = FRawEnum.Linear;

        [Fact]
        public void Save() {
            ViewData viewdata = new ViewData(new TestServices());
            RawData raw = new RawData(Ends, NumOfNodes, true, Func);

            viewdata.Save(RawFile);

            Assert.Equal(new StreamReader(File.OpenRead(RawFile)).ReadToEnd(), new StreamReader(File.OpenRead(OkFile)).ReadToEnd());
        }

        [Fact]
        public void Load() {
            RawData okraw = new RawData(Ends, NumOfNodes, true, Func);
            ViewData viewdata = new ViewData(new TestServices());

            viewdata.Load(OkFile);

            Assert.Equivalent(okraw, viewdata.Raw);
        }

        [Fact]
        public void CanExecuteTest() {
            ViewData goodData = new ViewData(new TestServices());
            ViewData badData = new ViewData(new TestServices());
            badData.NodesNum = 0;

            Assert.True(goodData.CreateCommand.CanExecute(null));
            Assert.True(goodData.SaveCommand.CanExecute(null));
            Assert.True(goodData.LoadCommand.CanExecute(null));

            Assert.False(badData.CreateCommand.CanExecute(null));
            Assert.False(badData.SaveCommand.CanExecute(null)); 
            Assert.False(badData.LoadCommand.CanExecute(null));
        }

        [Fact]
        public void CreateCommandTest() {
            double[] coord_vals = new double[] { 0, 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5 };
            ViewData viewdata = new ViewData(new TestServices());

            viewdata.CreateCommand.Execute(null);

            for (int i = 0; i < 11; i++) {
                Assert.InRange<double>(viewdata.Spline.SplineList[i].FstDer, 0.99, 1.01);
                Assert.InRange<double>(viewdata.Spline.SplineList[i].SndDer, -0.01, 0.01);
                Assert.InRange<double>(viewdata.Spline.SplineList[i].Coord, coord_vals[i] - 0.01, coord_vals[i] + 0.01);
                Assert.InRange<double>(viewdata.Spline.SplineList[i].Val, coord_vals[i] - 0.01, coord_vals[i] + 0.01);
            }
        }

        [Fact]
        public void SaveCommandTest() {
            TestServices services = new TestServices();
            ViewData viewdata = new ViewData(services);

            viewdata.SaveCommand.Execute(null);

            Assert.Equal(new StreamReader(File.OpenRead(services.FileName)).ReadToEnd(), new StreamReader(File.OpenRead(OkFile)).ReadToEnd());
        }

        [Fact]
        public void LoadCommandTest() {
            RawData okraw = new RawData(Ends, NumOfNodes, true, Func);
            ViewData viewdata = new ViewData(new TestServices());

            viewdata.LoadCommand.Execute(null);

            Assert.Equivalent(okraw, viewdata.Raw);
        }

        [Fact]
        public void BadDataLoadTest() {
            ViewData viewdata = new ViewData(new BadDataTestServices());

            viewdata.LoadCommand.Execute(null);
        }
    }

    public class TestServices : IServices {
        public string FileName { get; set; }

        public void ErrorReport(string message) {
            throw new NotImplementedException();
        }

        public void LoadFile() {
            FileName = "..\\..\\..\\..\\test_data\\viewmodel_test\\RawDataTest_OK.txt";
        }

        public void SaveFile() {
            FileName = "..\\..\\..\\..\\test_data\\viewmodel_test\\viewmodel_command_test.txt";
        }
    }

    public class BadDataTestServices : IServices {
        public string FileName { get; set; }

        public void ErrorReport(string message) {
            Assert.True(true);
        }

        public void LoadFile() {
            FileName = "..\\..\\..\\..\\test_data\\viewmodel_test\\RawDataTest_BadData.txt";
        }

        public void SaveFile() {
            throw new NotImplementedException();
        }
    }
}