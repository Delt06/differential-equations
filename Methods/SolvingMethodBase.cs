using System;
using DEAssignment.Methods.Visitors;
using JetBrains.Annotations;

namespace DEAssignment.Methods
{
    public abstract class SolvingMethodBase : ISolvingMethod
    {
        public double? this[double step, Ivp ivp, int i]
        {
            get
            {
                if (step <= 0d) throw new ArgumentOutOfRangeException(nameof(step));
                if (i < 0) throw new ArgumentOutOfRangeException(nameof(i));

                var value = GetValue(step, ivp, i);

                return Utils.CanBeRepresentedOnChart(value) ? value : (double?) null;
            }
        }

        public void Accept(IMethodVisitor visitor)
        {
            if (visitor is null) throw new ArgumentNullException(nameof(visitor));
            AcceptVisitorInternal(visitor);
        }

        protected abstract void AcceptVisitorInternal([NotNull] IMethodVisitor visitor);

        protected abstract double GetValue(double step, Ivp ivp, int i);
    }
}