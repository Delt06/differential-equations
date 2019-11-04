using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DEAssignment.Charts.Error;
using DEAssignment.Charts.Factories;
using DEAssignment.Charts.Solution;
using DEAssignment.Methods;
using JetBrains.Annotations;

namespace DEAssignment.Charts
{
    public class FunctionChartCollection : IEnumerable<IFunctionChart>
    {
        [NotNull] public readonly IReadOnlyList<ISolvingMethodChart> SolutionCharts;
        [NotNull] public readonly IReadOnlyList<ILocalErrorsChart> LocalErrorsCharts;
        [NotNull] public readonly IReadOnlyList<IGlobalErrorsChart> GlobalErrorsCharts;

        [NotNull]
        public static FunctionChartCollection CreateViaFactory([NotNull] ISolvingMethod[] methods,
            [NotNull] IMethodChartFactory factory)
        {
            if (methods is null) throw new ArgumentNullException(nameof(methods));
            if (factory is null) throw new ArgumentNullException(nameof(factory));

            var solutionCharts = methods
                .Select(m => m is ExactMethod ? 
                    factory.CreateHighPrecisionChartFor(m) : 
                    factory.CreateSolutionChartFor(m));
            var localErrorsCharts = methods
                .Where(m => m is ApproximatedMethod)
                .Select(factory.CreateLocalErrorsChartFor);
            var globalErrorsCharts = methods
                .Where(m => m is ApproximatedMethod)
                .Select(factory.CreateGlobalErrorsChartFor);
            
            return new FunctionChartCollection(solutionCharts, localErrorsCharts, globalErrorsCharts);
        }
        
        public FunctionChartCollection([NotNull] IEnumerable<ISolvingMethodChart> solutionCharts,
            [NotNull] IEnumerable<ILocalErrorsChart> localErrorsCharts,
            [NotNull] IEnumerable<IGlobalErrorsChart> globalErrorsCharts)
        {
            if (solutionCharts is null) throw new ArgumentNullException(nameof(solutionCharts));
            if (localErrorsCharts is null) throw new ArgumentNullException(nameof(localErrorsCharts));
            if (globalErrorsCharts is null) throw new ArgumentNullException(nameof(globalErrorsCharts));

            SolutionCharts = solutionCharts.ToArray();
            LocalErrorsCharts = localErrorsCharts.ToArray();
            GlobalErrorsCharts = globalErrorsCharts.ToArray();
        }

        public IEnumerator<IFunctionChart> GetEnumerator()
        {
            foreach (var chart in SolutionCharts)
            {
                yield return chart;
            }

            foreach (var chart in LocalErrorsCharts)
            {
                yield return chart;
            }

            foreach (var chart in GlobalErrorsCharts)
            {
                yield return chart;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}