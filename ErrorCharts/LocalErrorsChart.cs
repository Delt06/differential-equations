using System;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using DEAssignment.Methods;
using DEAssignment.Methods.Errors;
using DEAssignment.Methods.Visitors;
using JetBrains.Annotations;

namespace DEAssignment.ErrorCharts
{
    public class LocalErrorsChart : Chart
    {
        [NotNull] private readonly ISolvingMethod _method;
        private readonly Series _series;

        public LocalErrorsChart([NotNull] ISolvingMethod method)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));

            Size = Utils.ChartSize;
            _series = new Series();
            Series.Add(_series);
        }

        public void Update(double step, Ivp ivp, double xMax)
        {
            var localErrors = _method.GetLocalErrors(step, ivp, xMax, out _);

            UpdateSeries(localErrors);
        }

        private void UpdateSeries(double[] localErrors)
        {
            _series.Points.Clear();

            for (var i = 0; i < localErrors.Length; i++)
            {
                var point = new DataPoint(i, localErrors[i]);
                _series.Points.Add(point);
            }

            _series.Color = Color;
        }

        private Color Color
        {
            get
            {
                _method.Accept(_getColorVisitor);
                return _getColorVisitor.Result;
            }
        }
        
        [NotNull]
        private readonly GetColorVisitor _getColorVisitor = new GetColorVisitor();
        
        public ColorMapping ColorMapping;
    }
}