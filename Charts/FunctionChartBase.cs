using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DEAssignment.Methods;
using DEAssignment.Methods.Visitors;
using JetBrains.Annotations;

namespace DEAssignment.Charts
{
    public abstract class FunctionChartBase : Chart, IFunctionChart
    {
        public ISolvingMethod Method { get; }
        
        [NotNull]
        protected ChartArea Area { get; }
        [NotNull]
        protected Series FunctionSeries { get; }

        protected FunctionChartBase([NotNull] ISolvingMethod method)
        {
            Method = method ?? throw new ArgumentNullException(nameof(method));
            
            Size = Utils.ChartSize;
            
            Area = new ChartArea();
            ChartAreas.Add(Area);

            FunctionSeries = new Series {ChartType = SeriesChartType.Line};
            Series.Add(FunctionSeries);
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
    }
}