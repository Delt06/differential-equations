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
            ClassicRungeKuttaColor = Color.Purple,
            EulerMethodColor = Color.Red,
            ExactMethodColor = Color.Blue,
            ImprovedEulerMethodColor = Color.Orange
        };
        
        public ISolvingMethodChart CreateSolutionChartFor(ISolvingMethod solvingMethod)
        {
            if (solvingMethod is null) throw new ArgumentNullException(nameof(solvingMethod));

            var chart = new SolvingMethodChart(solvingMethod)
            {
                ColorMapping = _colorMapping
            };

            var title = new Title(GetNameOf(solvingMethod));
            chart.Titles.Add(title);

            return chart;
        }

        public ILocalErrorsChart CreateLocalErrorsChartFor(ISolvingMethod solvingMethod)
        {
            if (solvingMethod is null) throw new ArgumentNullException(nameof(solvingMethod));

            var chart = new LocalErrorsChart(solvingMethod)
            {
                ColorMapping = _colorMapping
            };

            var title = new Title(GetNameOf(solvingMethod) + ". Local errors");
            chart.Titles.Add(title);

            return chart;
        }

        public IGlobalErrorsChart CreateGlobalErrorsChartFor(ISolvingMethod solvingMethod)
        {
            if (solvingMethod is null) throw new ArgumentNullException(nameof(solvingMethod));
            
            var chart = new GlobalErrorsChart(solvingMethod)
            {
                ColorMapping = _colorMapping
            };

            var title = new Title(GetNameOf(solvingMethod) + ". Global errors");
            chart.Titles.Add(title);

            return chart;
        }

        [NotNull]
        private string GetNameOf([NotNull] ISolvingMethod solvingMethod)
        {
            solvingMethod.Accept(_nameVisitor);
            return _nameVisitor.Result;
        }
    }
}