using DEAssignment.Methods.Visitors;

namespace DEAssignment.Charts.Solution
{
    public interface ISolvingMethodChart : IFunctionChart
    {
        int N { get; }
        double Step { get; }
        Ivp Ivp { get; }
        double XMax { get; }
        double YMax { get; }
        double YMin { get; }
        ColorMapping ColorMapping { get; set; }
        void SetUp(int n, Ivp ivp, double xMax);
    }
}