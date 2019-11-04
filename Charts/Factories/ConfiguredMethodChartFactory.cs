using System;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using DEAssignment.Charts.Error;
using DEAssignment.Charts.Solution;
using DEAssignment.Methods;
using DEAssignment.Methods.Visitors;
using JetBrains.Annotations;

namespace DEAssignment.Charts.Factories
{
    public class ConfiguredMethodChartFactory : IMethodChartFactory
    {
        [NotNull] private readonly GetNameVisitor _nameVisitor = new GetNameVisitor();

        private readonly ColorMapping _colorMapping = new ColorMapping
        {
            FallbackColor = Color.Black,
            ClassicRungeKuttaMethodColor = Color.Purple,
            EulerMethodColor = Color.Red,
            ExactMethodColor = Color.Blue,
            ImprovedEulerMethodColor = Color.Orange
        };
        
        public ISolvingMethodChart CreateSolutionChartFor(ISolvingMethod solvingMethod)
        {
            if (solvingMethod is null) throw new ArgumentNullException(nameof(solvingMethod));

            var chart = new SolvingMethodChart(solvingMethod);
            Configure(chart);
            return chart;
        }

        private void Configure([NotNull] FunctionChartBase chart, [NotNull] string suffix = "")
        {
            if (chart is null) throw new ArgumentNullException(nameof(chart));

            chart.ColorMapping = _colorMapping;

            var name = GetNameOf(chart.Method);
            
            if (!string.IsNullOrEmpty(suffix))
            {
                name += suffix;
            }
            
            var title = new Title(name);
            chart.Titles.Add(title);
        }

        [NotNull]
        private string GetNameOf([NotNull] ISolvingMethod solvingMethod)
        {
            solvingMethod.Accept(_nameVisitor);
            return _nameVisitor.Result;
        }
        
        public ISolvingMethodChart CreateHighPrecisionChartFor(ISolvingMethod solvingMethod)
        {
            if (solvingMethod is null) throw new ArgumentNullException(nameof(solvingMethod));

            var chart = new HighPrecisionSolvingMethodChart(solvingMethod);
            Configure(chart);
            return chart;
        }

        public ILocalErrorsChart CreateLocalErrorsChartFor(ISolvingMethod solvingMethod)
        {
            if (solvingMethod is null) throw new ArgumentNullException(nameof(solvingMethod));

            var chart = new LocalErrorsChart(solvingMethod);
            Configure(chart, ". Local errors");
            return chart;
        }

        public IGlobalErrorsChart CreateGlobalErrorsChartFor(ISolvingMethod solvingMethod)
        {
            if (solvingMethod is null) throw new ArgumentNullException(nameof(solvingMethod));

            var chart = new GlobalErrorsChart(solvingMethod);
            Configure(chart, ". Global errors");
            return chart;
        }
    }
}