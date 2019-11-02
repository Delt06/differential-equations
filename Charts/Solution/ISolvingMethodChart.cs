namespace DEAssignment.Charts.Solution
{
    public interface ISolvingMethodChart : IFunctionChart
    {
        int N { get; }
        double Step { get; }
        Ivp Ivp { get; }
        double XMax { get; }
        void SetUp(int n, Ivp ivp, double xMax);
    }
}