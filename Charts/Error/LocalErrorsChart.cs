using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DEAssignment.Methods;
using DEAssignment.Methods.Errors;
using DEAssignment.Methods.Visitors;
using JetBrains.Annotations;

namespace DEAssignment.Charts.Error
{
    public class LocalErrorsChart : FunctionChartBase, ILocalErrorsChart
    {
        public LocalErrorsChart([NotNull] ISolvingMethod method) : base(method)
        {
            
        }

        public void Update(double step, Ivp ivp, double xMax)
        {
            var localErrors = Method.GetLocalErrors(step, ivp, xMax, out _);

            UpdateSeries(localErrors);
            UpdateAxes(localErrors);
        }

        public new Control Control => this;

        private void UpdateAxes(IReadOnlyCollection<double?> localErrors)
        {
            Area.AxisX.Minimum = 0;
            Area.AxisX.Maximum = localErrors.Count - 1;
            Area.AxisX.Interval = 1;

            var min = localErrors.Where(e => e != null).Min() ?? -1d;
            var max = localErrors.Where(e => e != null).Max() ?? 1d;
            
            if (min >= max)
            {
                min = max - 0.5d;
                max += 0.5d;
            }
            
            Area.AxisY.Minimum = min;
            Area.AxisY.Maximum = max;
            Area.AxisY.LabelStyle = new LabelStyle() {Format = Utils.ErrorFormat};
        }
        
        private void UpdateSeries(IReadOnlyList<double?> localErrors)
        {
            FunctionSeries.Points.Clear();

            for (var i = 0; i < localErrors.Count; i++)
            {
                if (localErrors[i] == null) continue;
                
                var point = new DataPoint(i, localErrors[i].Value);
                FunctionSeries.Points.Add(point);
            }

            FunctionSeries.Color = Color;
        }
    }
}