using System;
using System.Drawing;

namespace DEAssignment
{
    public static class Utils
    {
        public const int HighN = 100;
        
        public const string ErrorFormat = "e2";
        public const int N = 10;
        public const int NMax = 20;
        public static readonly Ivp Ivp = new Ivp(0d, Math.Sqrt(1d / 2d));
        public const double XMax = 3d;

        public static double Solution(double x) => Math.Exp(x * x / 2d) / Math.Sqrt(Math.Exp(x * x) + 1d);

        public static double RightSideFunction(double x, double y) => x * y * (1 - y * y);

        public static double GetStep(double x0, double xMax, int pointsCount) => (xMax - x0) / (pointsCount - 1);

        public static int GetPointsCount(double x0, double xMax, double step)
        {
            var segments = (xMax - x0) / step;
            var segmentsCeil = (int) Math.Ceiling(segments);

            return segmentsCeil + 1;
        }

        public static readonly Size ChartSize = new Size(ChartSide, ChartSide);
        private const int ChartSide = 325;

        public static bool CanBeRepresentedOnChart(double value) => IsValid(value) && IsInRangeOfDecimal(value);
        
        private static bool IsValid(double value) => !double.IsInfinity(value) && !double.IsNaN(value);

        private static bool IsInRangeOfDecimal(double value) => (double) decimal.MinValue <= value && value <= (double) decimal.MaxValue;
        
    }
}