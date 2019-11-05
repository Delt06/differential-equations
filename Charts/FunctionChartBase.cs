using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DEAssignment.Charts.Solution;
using DEAssignment.Methods;
using DEAssignment.Methods.Visitors;
using JetBrains.Annotations;

namespace DEAssignment.Charts
{
    public abstract class FunctionChartBase : Chart, IFunctionChart
    {
        private const int LineWidth = 3;
        private const int DefaultGridCellCount = 5;
        protected virtual bool RoundXIntervalToInt => false;
        
        public ISolvingMethod Method { get; }
        
        [NotNull]
        protected ChartArea Area { get; }
        [NotNull]
        protected Series FunctionSeries { get; }

        protected FunctionChartBase([NotNull] ISolvingMethod method)
        {
            Method = method ?? throw new ArgumentNullException(nameof(method));
            
            Size = Utils.ChartSize;

            Area = new ChartArea
            {
                AxisX = 
                {
                    IntervalAutoMode = IntervalAutoMode.FixedCount,
                    MajorGrid = {LineColor = Color.LightGray}
                },
                AxisY = {MajorGrid = {LineColor = Color.LightGray}}
            };
            ChartAreas.Add(Area);
            UpdateGridIntervals();

            FunctionSeries = new Series
            {
                ChartType = SeriesChartType.Line, 
                BorderWidth = LineWidth
            };
            Series.Add(FunctionSeries);
        }

        protected void UpdateGridIntervals(int gridCellCount = DefaultGridCellCount)
        {
            UpdateGridIntervalForAxis(Area.AxisX, gridCellCount, RoundXIntervalToInt);
            UpdateGridIntervalForAxis(Area.AxisY, gridCellCount, false);
        }

        private void UpdateGridIntervalForAxis([NotNull] Axis axis, int gridCellCount = DefaultGridCellCount, bool round = false)
        {
            if (axis is null) throw new ArgumentNullException(nameof(axis));

            var range = axis.Maximum - axis.Minimum;
            var interval = range / gridCellCount;
            
            if (round)
            {
                interval = Math.Round(interval);
            }

            axis.Interval = interval;
            axis.MajorGrid.Interval = interval;
            axis.LabelStyle.Interval = interval;
        }
        
        protected Color Color
        {
            get
            {
                _getColorVisitor.Mapping = ColorMapping;
                Method.Accept(_getColorVisitor);
                return _getColorVisitor.Result;
            }
        }
        
        [NotNull]
        private readonly GetColorVisitor _getColorVisitor = new GetColorVisitor();

        public Control Control => this;
        public ColorMapping ColorMapping { get; set; }

        protected (double xMin, double xMax) XAxisRange
        {
            get => (Area.AxisX.Minimum, Area.AxisX.Maximum);
            set => (Area.AxisX.Minimum, Area.AxisX.Maximum) = value;
        }

        protected void UpdateYAxisRange()
        {
            var axis = Area.AxisY;
            (axis.Minimum, axis.Maximum) = GetYRange();
        }

        private (double yMin, double yMax) GetYRange()
        {
            if (FunctionSeries.Points.Count <= 0) return (0f, 1f);
            
            var yMin = FunctionSeries.Points.Select(p => p.YValues[0]).Min();
            var yMax = FunctionSeries.Points.Select(p => p.YValues[0]).Max();

            if (Math.Abs(yMin - yMax) > double.Epsilon) return (yMin, yMax);

            const double halfRange = 0.5d;
            yMax += halfRange;
            yMin -= halfRange;

            return (yMin, yMax);
        }
    }
}