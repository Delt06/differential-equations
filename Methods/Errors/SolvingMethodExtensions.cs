using System;
using JetBrains.Annotations;

namespace DEAssignment.Methods.Errors
{
    public static class SolvingMethodExtensions
    {
        [NotNull]
        public static double[] GetGlobalErrors([NotNull] this ISolvingMethod solvingMethod,
            double step, Ivp ivp, double xMax)
        {
            if (solvingMethod is null) throw new ArgumentNullException(nameof(solvingMethod));
            
            var pointsCount = Utils.GetPointsCount(ivp.X0, xMax, step);
            var globalErrors = new double[pointsCount];

            for (var i = 0; i < pointsCount; i++)
            {
                var x = ivp.X0 + step * i;
                var y = solvingMethod[step, ivp, i];
                var yExact = Utils.Solution(x);

               globalErrors[i] = y - yExact;
            }

            return globalErrors;
        }

        [NotNull]
        public static double[] GetLocalErrors([NotNull] this ISolvingMethod solvingMethod,
            double step, Ivp ivp, double xMax, out double[] globalErrors)
        {
            if (solvingMethod is null) throw new ArgumentNullException(nameof(solvingMethod));
            
            var pointsCount = Utils.GetPointsCount(ivp.X0, xMax, step);
            globalErrors = solvingMethod.GetGlobalErrors(step, ivp, xMax);
            var localErrors = new double[pointsCount];

            localErrors[0] = 0d;

            for (var i = 1; i < pointsCount; i++)
            {
                localErrors[i] = globalErrors[i] - globalErrors[i - 1];
            }

            return localErrors;
        }
    }
}