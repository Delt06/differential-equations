using DEAssignment.Charts.Error;
using DEAssignment.Methods.Visitors;

namespace DEAssignment.Charts
{
    public interface IFunctionChart : IHasControl, IHasSolvingMethod
    {
        ColorMapping ColorMapping { get; set; }
    }
}