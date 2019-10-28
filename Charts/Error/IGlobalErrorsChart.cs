namespace DEAssignment.Charts.Error
{
    public interface IGlobalErrorsChart : IFunctionChart
    {
        int NMin { get; }
        int NMax { get; }

        void Update(int nMin, int nMax, Ivp ivp, double xMax);
    }
}