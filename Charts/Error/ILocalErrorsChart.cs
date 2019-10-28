namespace DEAssignment.Charts.Error
{
    public interface ILocalErrorsChart : IFunctionChart
    {
        void Update(double step, Ivp ivp, double xMax);
    }
}