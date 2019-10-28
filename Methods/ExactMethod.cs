using System;
using DEAssignment.Methods.Visitors;
using JetBrains.Annotations;

namespace DEAssignment.Methods
{
    public sealed class ExactMethod : SolvingMethodBase
    {
        [NotNull] private readonly EquationSolution _solution;

        public ExactMethod([NotNull] EquationSolution solution)
        {
            _solution = solution ?? throw new ArgumentNullException(nameof(solution));
        }

        protected override void AcceptVisitorInternal(IMethodVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override double GetValue(double step, Ivp ivp, int i)
        {
            var x = ivp.X0 + step * i;
            return _solution(x);
        }
    }
}