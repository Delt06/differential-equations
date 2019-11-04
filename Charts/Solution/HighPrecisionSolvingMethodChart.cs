using DEAssignment.Methods;
using JetBrains.Annotations;

namespace DEAssignment.Charts.Solution
{
    public sealed class HighPrecisionSolvingMethodChart : SolvingMethodChart
    {
        public HighPrecisionSolvingMethodChart([NotNull] ISolvingMethod method) : base(method)
        {
        }

        protected override int MinN => Utils.HighN;
    }
}