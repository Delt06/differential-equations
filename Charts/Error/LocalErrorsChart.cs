using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using DEAssignment.Methods;
using DEAssignment.Methods.Errors;
using JetBrains.Annotations;

namespace DEAssignment.Charts.Error
{
    public class LocalErrorsChart : FunctionChartBase, ILocalErrorsChart
    {
        protected sealed override bool RoundXIntervalToInt => true;
        
        public LocalErrorsChart([NotNull] ISolvingMethod method) : base(method)
        {
            Area.AxisX.Title = "i";
            Area.AxisY.Title = "error";
            Area.AxisY.LabelStyle = new LabelStyle {Format = Utils.ErrorFormat};
        }

        public void Update(double step, Ivp ivp, double xMax)
        {
            var localErrors = Method.GetLocalErrors(step, ivp, xMax, out _);

            UpdateSeries(localErrors);
            UpdateAxes(localErrors);
            UpdateGridIntervals();
        }

        private void UpdateAxes(IReadOnlyCollection<double?> localErrors)
        {
            XAxisRange = (0, localErrors.Count - 1);
            Area.AxisX.Interval = 1;
            
            UpdateYAxisRange();
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