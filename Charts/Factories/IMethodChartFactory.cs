using DEAssignment.Charts.Error;
using DEAssignment.Charts.Solution;
using DEAssignment.Methods;
using JetBrains.Annotations;

namespace DEAssignment.Charts.Factories
{
    public interface IMethodChartFactory
    {
        [NotNull]
        ISolvingMethodChart CreateSolutionChartFor([NotNull] ISolvingMethod solvingMethod);

        [NotNull]
        ISolvingMethodChart CreateHighPrecisionChartFor([NotNull] ISolvingMethod solvingMethod);

        [NotNull]
        ILocalErrorsChart CreateLocalErrorsChartFor([NotNull] ISolvingMethod solvingMethod);

        [NotNull]
        IGlobalErrorsChart CreateGlobalErrorsChartFor([NotNull] ISolvingMethod solvingMethod);
    }
}