using DEAssignment.Methods;
using JetBrains.Annotations;

namespace DEAssignment.SolutionCharts.Factories
{
    public interface IMethodChartFactory
    {
        [NotNull]
        ISolvingMethodChart CreateChartFor([NotNull] ISolvingMethod solvingMethod);
    }
}