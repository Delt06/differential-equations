using System;
using JetBrains.Annotations;

namespace DEAssignment
{
    public class Constraint
    {
        [NotNull] private readonly Predicate<double> _condition;

        public bool IsSatisfied(double value) => _condition(value);

        public Constraint([NotNull] Predicate<double> condition)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        [NotNull] public static readonly Constraint None = new Constraint(n => true);
    }
}