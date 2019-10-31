using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DEAssignment.Methods;
using DEAssignment.Methods.Errors;
using JetBrains.Annotations;

namespace DEAssignment.Charts.Error
{
    public class GlobalErrorsChart : FunctionChartBase, IGlobalErrorsChart
    {
        public GlobalErrorsChart([NotNull] ISolvingMethod method) : base(method)
        {
        }

        public new Control Control => this;
        public int NMin { get; private set; }
        public int NMax { get; private set; }
        public Ivp Ivp { get; private set; }
        public double XMax { get; private set; }
        
        public void Update(int nMin, int nMax, Ivp ivp, double xMax)
        {
            NMin = nMin;
            NMax = nMax;
            Ivp = ivp;
            XMax = xMax;

            var pointCount = NMax - NMin + 1;
            var errors = Enumerable.Range(NMin, pointCount)
                .Select(GetLastGlobalError)
                .ToArray();
            UpdateAxes(errors);
            UpdateSeries(nMin, errors);
        }

        private double GetLastGlobalError(int n)
        {
            var step = Utils.GetStep(Ivp.X0, XMax, n);
            var errors = Method.GetGlobalErrors(step, Ivp, XMax);
            return errors.Last(e => e != null) ?? 0d;
        }

        private void UpdateAxes([NotNull] IReadOnlyCollection<double> errors)
        {
            Area.AxisX.Minimum = NMin;
            Area.AxisX.Maximum = NMax;
            Area.AxisX.Interval = 1;

            var min = errors.Min();
            var max = errors.Max();

            if (min >= max || Math.Abs(max - min) < 1d)
            {
                min = max - 0.5d;
                max += 0.5d;
            }

            Area.AxisY.Minimum = min;
            Area.AxisY.Maximum = max;
            Area.AxisY.LabelStyle = new LabelStyle() {Format = Utils.ErrorFormat};
        }

        private void UpdateSeries(int nMin, [NotNull] IReadOnlyList<double> errors)
        {
            FunctionSeries.Points.Clear();

            for (var i = 0; i < errors.Count; i++)
            {
                var point = new DataPoint(nMin + i, errors[i]);
                FunctionSeries.Points.Add(point);
            }

            FunctionSeries.Color = Color;
        }
    }
}