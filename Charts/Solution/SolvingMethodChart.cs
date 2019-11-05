using System;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using DEAssignment.Methods;
using JetBrains.Annotations;

namespace DEAssignment.Charts.Solution
{
    public class SolvingMethodChart : FunctionChartBase, ISolvingMethodChart
    {
        public double Step { get; private set; }
        public Ivp Ivp { get; private set; }
        public double XMax { get; private set; }
        public int N { get; private set; }

        protected virtual int MinN => 2;
        protected virtual int MaxN => int.MaxValue;

        private const string AxisFormat = "F2";

        public SolvingMethodChart([NotNull] ISolvingMethod method) : base(method)
        {
            ConfigureAxis(Area.AxisX, "x");
            ConfigureAxis(Area.AxisY, "y");
        }
        
        private static void ConfigureAxis([NotNull] Axis axis, [NotNull] string title)
        {
            axis.Title = title;
            axis.LabelStyle = new LabelStyle {Format = AxisFormat};
        }
        
        private void ConfigureArea()
        {
            XAxisRange = (Ivp.X0, XMax);
            UpdateYAxisRange();
        }

        public void SetUp(int n, Ivp ivp, double xMax)
        {
            N = n;
            ClampN();
            Ivp = ivp;
            XMax = xMax;
            RecalculateStep();

            ConfigureSeries();
            ConfigureArea();
            UpdateGridIntervals();

            Area.RecalculateAxesScale();
        }

        private void RecalculateStep()
        {
            Step = Utils.GetStep(Ivp.X0, XMax, N);
        }

        private void ClampN()
        {
            N = Math.Clamp(N, MinN, MaxN);
        }

        private void ConfigureSeries()
        {
            FunctionSeries.Points.Clear();

            foreach (var point in Enumerable.Range(0, N)
                .Select(i => TryGetDataPoint(i, out var point) ? point : null)
                .Where(p => p != null))
            {
                FunctionSeries.Points.Add(point);
            }

            FunctionSeries.Color = Color;
        }

        private bool TryGetDataPoint(int i, out DataPoint point)
        {
            var x = Ivp.X0 + Step * i;
            var y = Method[Step, Ivp, i];
            
            if (y == null)
            {
                point = default;
                return false;
            }

            point = new DataPoint(x, y.Value);
            return true;
        }
    }
}