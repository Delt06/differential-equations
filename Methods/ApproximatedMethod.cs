using System;
using JetBrains.Annotations;

namespace DEAssignment.Methods
{
    public abstract class ApproximatedMethod : SolvingMethodBase
    {
        [NotNull] protected RightSideFunction RightSideFunction { get; }

        protected ApproximatedMethod([NotNull] RightSideFunction rightSideFunction)
        {
            RightSideFunction = rightSideFunction ?? throw new ArgumentNullException(nameof(rightSideFunction));
        }
    }
}