using System;
using System.Drawing;

namespace DEAssignment
{
    public static class Utils
    {
        public const double Step = 0.1d;
        public static readonly Ivp Ivp = new Ivp(1d, 1d);
        public const double XMax = 9d;

        public static double Solution(double x) => x;

        public static double RightSideFunction(double x, double y) => (2d * x + 1d) * y / x - y * y / x - x;

        public static int GetPointsCount(double x0, double xMax, double step) => (int) Math.Ceiling((xMax - x0) / step) + 1;

        public static readonly Size ChartSize = new Size(500, 500);
    }
}