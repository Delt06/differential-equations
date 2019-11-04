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

        public SolvingMethodChart([NotNull] ISolvingMethod method) : base(method)
        {
            
        }
        
        private void ConfigureArea()
        {
            ConfigureAxis(Area.AxisX, Ivp.X0, XMax);
            ConfigureAxis(Area.AxisY, null, null);
        }
        
        private static void ConfigureAxis([NotNull] Axis axis, double? min, double? max)
        {
            axis.Minimum = min ?? axis.Minimum;
            axis.Maximum = max ?? axis.Maximum;
            
            axis.LabelStyle = new LabelStyle {Format = "F2"};
        }

        public void SetUp(int n, Ivp ivp, double xMax)
        {
            N = n;
            ClampN();
            Ivp = ivp;
            XMax = xMax;
            RecalculateStep();

            ConfigureArea();
            ConfigureSeries();
            
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