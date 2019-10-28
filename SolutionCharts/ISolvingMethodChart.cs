using System.Windows.Forms;
using DEAssignment.Methods;
using DEAssignment.Methods.Visitors;
using JetBrains.Annotations;

namespace DEAssignment.SolutionCharts
{
    public interface ISolvingMethodChart
    {
        double Step { get; }
        Ivp Ivp { get; }
        double XMax { get; }
        double YMax { get; }
        double YMin { get; }
        ColorMapping ColorMapping { get; set; }
        void SetUp(double step, Ivp ivp, double xMax);
        
        [NotNull]
        Control Control { get; }

        ISolvingMethod Method { get; }
    }
}