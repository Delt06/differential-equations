using JetBrains.Annotations;

namespace DEAssignment.Methods.Visitors
{
    public interface IMethodVisitor
    {
        void Visit([NotNull] ISolvingMethod method);
        void Visit([NotNull] EulerMethod eulerMethod);
        void Visit([NotNull] ExactMethod exactMethod);
        void Visit([NotNull] ImprovedEulerMethod improvedEulerMethod);
        void Visit([NotNull] ClassicRungeKuttaMethod classicRungeKuttaMethod);
    }
}