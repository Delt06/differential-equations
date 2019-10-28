using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DEAssignment.Methods;
using DEAssignment.Methods.Visitors;
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

        public Control Control => this;

        private double CalculateYMaxValue() => Math.Max(Enumerable.Range(0, N)
            .Max(i => Method[Step, Ivp, i]), Ivp.Y0);

        private double CalculateYMinValue() => Math.Min(Enumerable.Range(0, N)
            .Min(i => Method[Step, Ivp, i]), Ivp.Y0);
        
        private void ConfigureArea()
        {
            ConfigureAxis(Area.AxisX, Ivp.X0, XMax);
            ConfigureAxis(Area.AxisY, YMin, YMax);
        }

        private static void ConfigureAxis([NotNull] Axis axis, double min, double max)
        {
            axis.Minimum = min;
            axis.Maximum = max;

            axis.LabelStyle = new LabelStyle {Format = "F2"};
        }

        private void ConfigureSeries()
        {
            FunctionSeries.Points.Clear();

            foreach (var point in Enumerable.Range(0, N).Select(GetDataPoint))
            {
                FunctionSeries.Points.Add(point);
            }

            FunctionSeries.Color = Color;
        }

        [NotNull]
        private DataPoint GetDataPoint(int i)
        {
            var x = Ivp.X0 + Step * i;
            var y = Method[Step, Ivp, i];
            return new DataPoint(x, y);
        }
    }
}