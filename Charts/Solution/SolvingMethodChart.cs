using System;
using System.Linq;
using System.Windows.Forms;
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
        public double YMax { get; private set; }
        public double YMin { get; private set; }
        public int N { get; private set; }

        public SolvingMethodChart([NotNull] ISolvingMethod method) : base(method)
        {
            
        }

        public void SetUp(int n, Ivp ivp, double xMax)
        {
            N = n;
            Step = Utils.GetStep(ivp.X0, xMax, n);
            Ivp = ivp;
            XMax = xMax;

            YMax = CalculateYMaxValue();
            YMin = CalculateYMinValue();
            
            ConfigureArea();
            ConfigureSeries();
            
            Area.RecalculateAxesScale();
        }

        public new Control Control => this;

        private double CalculateYMaxValue() => Math.Max(Enumerable.Range(0, N)
            .Select(i => Method[Step, Ivp, i])
            .Where(y => y != null)
            .Max(y => y.Value), Ivp.Y0);

        private double CalculateYMinValue() => Math.Min(Enumerable.Range(0, N)
            .Select(i => Method[Step, Ivp, i])
            .Where(y => y != null)
            .Min(y => y.Value), Ivp.Y0);
        
        private void ConfigureArea()
        {
            ConfigureAxis(Area.AxisX, Ivp.X0, XMax);
            ConfigureAxis(Area.AxisY, 0d, 0d);
        }

        private static void ConfigureAxis([NotNull] Axis axis, double? min, double? max)
        {
            axis.Minimum = min ?? axis.Minimum;

            axis.LabelStyle = new LabelStyle {Format = "F2"};
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