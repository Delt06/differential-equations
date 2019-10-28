using DEAssignment.Methods;
using JetBrains.Annotations;

namespace DEAssignment.Charts
{
    public interface IHasSolvingMethod
    {
        [NotNull] ISolvingMethod Method { get; }
    }
}