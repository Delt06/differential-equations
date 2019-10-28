using JetBrains.Annotations;

namespace DEAssignment.Methods.Factories
{
    public interface IMethodFactory
    {
        [NotNull]
        EulerMethod CreateEulerMethod();

        [NotNull]
        ImprovedEulerMethod CreateImprovedEulerMethod();

        [NotNull]
        ExactMethod CreateExactMethod();

        [NotNull]
        ClassicRungeKuttaMethod CreateClassicRungeKuttaMethod();
    }
}