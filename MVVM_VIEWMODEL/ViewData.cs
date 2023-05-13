using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using MVVM_MODEL;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MVVM_VIEWMODEL {
    public class ViewData : INotifyPropertyChanged, IDataErrorInfo {
        public event PropertyChangedEventHandler PropertyChanged;

        //для RawData
        private int nodesnum;
        public int NodesNum {
            get { return nodesnum; }
            set {
                nodesnum = value;
                OnPropertyChanged("NodesNum");
            }
        }
        private double lend;
        public double LEnd {
            get { return lend; }
            set {
                lend = value;
                OnPropertyChanged("LEnd");
            }
        }
        private double rend;
        public double REnd {
            get { return rend; }
            set {
                rend = value;
                OnPropertyChanged("REnd");
            }
        }

        private bool isuniform;
        public bool IsUnuform {
            get { return isuniform; }
            set {
                isuniform = value;
                OnPropertyChanged("IsUnuform");
            }
        }
        private FRawEnum functype;
        public FRawEnum FuncType {
            get { return functype; }
            set {
                functype = value;
                OnPropertyChanged("FuncType");
            }
        }
        public RawData? Raw { get; set; }

        //для SplineData
        public int SplineNodesNum { get; set; }
        public double LeftSndDer { get; set; }
        public double RightSndDer { get; set; }
        private SplineData? spline;
        public SplineData? Spline {
            get { return spline; }
            set {
                spline = value;
                OnPropertyChanged("Spline");
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string fieldname] {
            get {
                return GetValidationError(fieldname);
            }
        }

        public void Save(string filename) {
            if (Raw == null) {
                double[] Ends = new double[] { LEnd, REnd };
                RawData rawData = new RawData(Ends, NodesNum, IsUnuform, FuncType);
                Raw = rawData;
            }
            Raw.Save(filename);
        }
        public void Load(string filename) {
            RawData rawData = new RawData(filename);
            NodesNum = rawData.NumOfNodes;
            LEnd = rawData.Ends[0];
            REnd = rawData.Ends[1];
            IsUnuform = rawData.IsUnuform;
            FuncType = rawData.Func;
            Raw = rawData;
        }
        public ViewData(IServices services) {
            NodesNum = 6;
            LEnd = 0;
            REnd = 5;
            IsUnuform = true;
            FuncType = FRawEnum.Linear;

            SplineNodesNum = 11;
            LeftSndDer = 0;
            RightSndDer = 0;
            Raw = null;
            Spline = null;

            FuncEnum = Enum.GetValues(typeof(FRawEnum));
            this.services = services;
        }

        public void CreateRawData() {
            double[] Ends = new double[] { LEnd, REnd };
            Raw = new RawData(Ends, NodesNum, IsUnuform, FuncType);
        }

        public void ExiquteSpline(bool createRawData = true) {
            if (createRawData)
                CreateRawData();
            SplineData splineData = new SplineData(Raw, LeftSndDer, RightSndDer, SplineNodesNum);
            splineData.Spline();
            Spline = splineData;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        //---------------------new---------------------//
        private string GetValidationError(string fieldname) {
            string error = null;
            switch (fieldname) {
                case "NodesNum":
                    if (NodesNum < 2)
                        error = "Число узлов должно быть >= 2!";
                    break;
                case "SplineNodesNum":
                    if (SplineNodesNum < 3)
                        error = "Число узлов должно быть >= 3!";
                    break;
                case "LEnd":
                    if (LEnd >= REnd)
                        error = "Левый конец отрезка должен быть строго меньше, чем правый!";
                    break;
                case "REnd":
                    if (REnd <= LEnd)
                        error = "Правый конец отрезка должен быть строго больше, чем левый!";
                    break;
            }
            return error;
        }

        private string[] rawData_info_srts;
        public string[] RawData_info_srts {
            get { return rawData_info_srts; }
            set {
                rawData_info_srts = value;
                OnPropertyChanged("RawData_info_srts");
            }
        }

        IServices services;
        private PlotModel model;
        public PlotModel Model {
            get { return model; }
            set {
                model = value;
                OnPropertyChanged("Model");
            }
        }

        private Array funcEnum;
        public Array FuncEnum {
            get { return funcEnum; }
            set { 
                funcEnum = value;
                OnPropertyChanged("FuncEnum");
            }
        }

        private readonly string[] filednames = { "NodesNum", "SplineNodesNum", "LEnd", "REnd" };
        private bool isDataValid() {
            foreach (string name in filednames)
                if (GetValidationError(name) != null)
                    return false;
            return true;
        }

        private CustomCommand createCommand;
        public CustomCommand CreateCommand {
            get {
                if (createCommand != null)
                    return createCommand;
                else {
                    createCommand = new CustomCommand(obj => {
                        ExiquteSpline();
                        string[] info = new string[NodesNum];
                        for (int i = 0; i < NodesNum; i++) {
                            info[i] = $"Coord = {Raw.Coord[i].ToString("0.000")}\nVal = {Raw.Val[i].ToString("0.000")}";
                        }
                        RawData_info_srts = info;
                        DrawPlot();
                    }, 
                    canExecute => isDataValid());
                    return createCommand;
                }
            }
        }

        private CustomCommand saveCommand;
        public CustomCommand SaveCommand {
            get {
                if (saveCommand != null) return saveCommand;
                else {
                    saveCommand = new CustomCommand(obj => {
                        try {
                            services.SaveFile();
                            Save(services.FileName);
                        }
                        catch (Exception ex) {
                            services.ErrorReport(ex.Message);
                        }
                    },
                    canExecute => isDataValid());
                    return saveCommand;
                }
            }
        }

        private CustomCommand loadCommand;
        public CustomCommand LoadCommand {
            get {
                if (loadCommand != null) return loadCommand;
                else {
                    loadCommand = new CustomCommand(obj => {
                        try {
                            services.LoadFile();
                            Load(services.FileName);
                            ExiquteSpline(false);
                            string[] info = new string[NodesNum];
                            for (int i = 0; i < NodesNum; i++) {
                                info[i] = $"Coord = {Raw.Coord[i].ToString("0.000")}\nVal = {Raw.Val[i].ToString("0.000")}";
                            }
                            RawData_info_srts = info;
                            DrawPlot();
                        }
                        catch(Exception ex) {
                            services.ErrorReport(ex.Message);
                        }
                    },
                    canExecute => isDataValid());
                    return loadCommand;
                }
            }
        }

        private void DrawPlot() {
            PlotModel model = new PlotModel();
            model.Axes.Add(new LinearAxis() {
                Position = AxisPosition.Bottom,
                Minimum = LEnd,
                Maximum = REnd,
                Title = "Coord"
            });
            model.Axes.Add(new LinearAxis() {
                Position = AxisPosition.Left,
                Minimum = Spline.SplineList.Min(x => x.Val),
                Maximum = Spline.SplineList.Max(x => x.Val),
                Title = "Value"
            });

            LineSeries rawline = new LineSeries() {
                Title = "RawData",
                Color = OxyColors.Transparent,
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Black
            };
            for (int i = 0; i < NodesNum; i++) {
                rawline.Points.Add(new DataPoint(Raw.Coord[i], Raw.Val[i]));
            }

            LineSeries splineline = new LineSeries() {
                Title = "SplineData",
                Color = OxyColors.Green,
            };
            for (int i = 0; i < SplineNodesNum; i++) {
                splineline.Points.Add(new DataPoint(
                    Spline.SplineList[i].Coord,
                    Spline.SplineList[i].Val
                    ));
            }

            model.Series.Add(splineline);
            model.Series.Add(rawline);
            Model = model;
            
            Model.InvalidatePlot(true);
        }
    }
}