using System;
using JetBrains.Annotations;

namespace DEAssignment.Methods.Errors
{
    public static class SolvingMethodExtensions
    {
        [NotNull]
        public static double?[] GetGlobalErrors([NotNull] this ISolvingMethod solvingMethod,
            double step, Ivp ivp, double xMax)
        {
            if (solvingMethod is null) throw new ArgumentNullException(nameof(solvingMethod));
            
            var pointsCount = Utils.GetPointsCount(ivp.X0, xMax, step);
            var globalErrors = new double?[pointsCount];

            for (var i = 0; i < pointsCount; i++)
            {
                var x = ivp.X0 + step * i;
                var y = solvingMethod[step, ivp, i];
                if (y == null) continue;
                
                var yExact = Utils.Solution(x);

                var error = y.Value - yExact;

                globalErrors[i] = Utils.CanBeRepresentedOnChart(error) ? error : (double?) null;
            }

            return globalErrors;
        }

        [NotNull]
        public static double?[] GetLocalErrors([NotNull] this ISolvingMethod solvingMethod,
            double step, Ivp ivp, double xMax, out double?[] globalErrors)
        {
            if (solvingMethod is null) throw new ArgumentNullException(nameof(solvingMethod));
            
            var pointsCount = Utils.GetPointsCount(ivp.X0, xMax, step);
            globalErrors = solvingMethod.GetGlobalErrors(step, ivp, xMax);
            var localErrors = new double?[pointsCount];

            localErrors[0] = 0d;

            for (var i = 1; i < pointsCount; i++)
            {
                if (globalErrors[i] == null) continue;
                var currentGlobalError = globalErrors[i].GetValueOrDefault();
                if (globalErrors[i - 1] == null) continue;
                var previousGlobalError = globalErrors[i - 1].GetValueOrDefault();

                var error = currentGlobalError - previousGlobalError;
                
                localErrors[i] = Utils.CanBeRepresentedOnChart(error) ? error : (double?) null;
            }

            return localErrors;
        }
    }
}