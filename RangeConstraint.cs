using System;
using JetBrains.Annotations;

namespace DEAssignment
{
    public class RangeConstraint
    {
        [NotNull] private readonly Func<double> _minValueFunction;
        [NotNull] private readonly Func<double> _maxValueFunction;

        public double MaxValue => _maxValueFunction();
        public double MinValue => _minValueFunction();

        public RangeConstraint([NotNull] Func<double> minValue, [NotNull] Func<double> maxValue)
        {
            _minValueFunction = minValue ?? throw new ArgumentNullException(nameof(minValue));
            _maxValueFunction = maxValue ?? throw new ArgumentNullException(nameof(maxValue));
        }
        
        [NotNull]
        public static readonly RangeConstraint None = new RangeConstraint(
            () => double.NegativeInfinity, 
            () => double.PositiveInfinity);
        
        [NotNull]
        public static readonly RangeConstraint ZeroOne = new RangeConstraint(() => 0d, () => 1d);
    }
}