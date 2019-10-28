using System;
using System.Drawing;

namespace DEAssignment
{
    public static class Utils
    {
        public const string ErrorFormat = "e2";
        public const int N = 10;
        public const int NMax = 20;
        public static readonly Ivp Ivp = new Ivp(1d, 1d);
        public const double XMax = 9d;

        public static double Solution(double x) => x;

        public static double RightSideFunction(double x, double y) => (2d * x + 1d) * y / x - y * y / x - x;

        public static double GetStep(double x0, double xMax, int pointsCount) => (xMax - x0) / (pointsCount - 1);

        public static int GetPointsCount(double x0, double xMax, double step)
        {
            var segments = (xMax - x0) / step;
            var segmentsCeil = (int) Math.Ceiling(segments);

            return segmentsCeil + 1;
        }

        public static readonly Size ChartSize = new Size(ChartSide, ChartSide);
        private const int ChartSide = 325;
    }
}