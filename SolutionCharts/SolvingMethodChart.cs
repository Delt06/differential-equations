using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DEAssignment.Methods;
using DEAssignment.Methods.Visitors;
using JetBrains.Annotations;

namespace DEAssignment.SolutionCharts
{
    public class SolvingMethodChart : Chart, ISolvingMethodChart
    {
        [NotNull] public ISolvingMethod Method { get; }

        private const double MajorGridInterval = 1d;
        
        public double Step { get; private set; }
        public Ivp Ivp { get; private set; }
        public double XMax { get; private set; }
        public double YMax { get; private set; }
        public double YMin { get; private set; }

        private readonly ChartArea _area;
        private readonly Series _series;
        
        public SolvingMethodChart([NotNull] ISolvingMethod method)
        {
            Size = Utils.ChartSize;
            
            Method = method;
            
            _area = new ChartArea();
            ChartAreas.Add(_area);

            _series = new Series {ChartType = SeriesChartType.Line};
            Series.Add(_series);
        }

        public void SetUp(double step, Ivp ivp, double xMax)
        {
            Step = step;
            Ivp = ivp;
            XMax = xMax;
            YMax = CalculateYMaxValue();
            YMin = CalculateYMinValue();
            
            ConfigureArea();
            ConfigureSeries();
            
            _area.RecalculateAxesScale();
        }

        public Control Control => this;

        private double CalculateYMaxValue() => Math.Max(Enumerable.Range(0, Utils.GetPointsCount(Ivp.X0, XMax, Step))
            .Max(i => Method[Step, Ivp, i]), Ivp.Y0);

        private double CalculateYMinValue() => Math.Min(Enumerable.Range(0, Utils.GetPointsCount(Ivp.X0, XMax, Step))
            .Min(i => Method[Step, Ivp, i]), Ivp.Y0);
        
        private void ConfigureArea()
        {
            ConfigureAxis(_area.AxisX, Ivp.X0, XMax);
            ConfigureAxis(_area.AxisY, YMin, YMax);
        }

        private void ConfigureAxis([NotNull] Axis axis, double min, double max)
        {
            axis.Minimum = min;
            axis.Maximum = max;
            axis.MajorGrid.Interval = MajorGridInterval;
            axis.MinorGrid.Interval = Step;

            axis.LabelStyle = new LabelStyle {Format = "F2"};
        }

        private void ConfigureSeries()
        {
            _series.Points.Clear();
            
            var pointsCount = Utils.GetPointsCount(Ivp.X0, XMax, Step);

            foreach (var point in Enumerable.Range(0, pointsCount).Select(GetDataPoint))
            {
                _series.Points.Add(point);
            }

            _series.Color = Color;
        }

        [NotNull]
        private DataPoint GetDataPoint(int i)
        {
            var x = Ivp.X0 + Step * i;
            var y = Method[Step, Ivp, i];
            return new DataPoint(x, y);
        }

        private Color Color
        {
            get
            {
                Method.Accept(_getColorVisitor);
                return _getColorVisitor.Result;
            }
        }
        
        [NotNull] private readonly GetColorVisitor _getColorVisitor = new GetColorVisitor();

        public ColorMapping ColorMapping { get; set; }
    }
}