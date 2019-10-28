using DEAssignment.Methods.Visitors;
using JetBrains.Annotations;

namespace DEAssignment.Methods
{
    public interface ISolvingMethod
    {
        double this[double step, Ivp ivp, int i] { get; }

        void Accept([NotNull] IMethodVisitor visitor);
    }
}