using JetBrains.Annotations;

namespace DEAssignment.Methods.Visitors
{
    public class GetNameVisitor : IMethodVisitor
    {
        [NotNull]
        public string Result { get; private set; } = "Undefined"; 
        
        public void Visit(ISolvingMethod method)
        {
            Result = "Unknown Solving Method";
        }

        public void Visit(EulerMethod eulerMethod)
        {
            Result = "Euler's Method";
        }

        public void Visit(ExactMethod exactMethod)
        {
            Result = "Exact Method";
        }

        public void Visit(ImprovedEulerMethod improvedEulerMethod)
        {
            Result = "Improved Euler's Method";
        }

        public void Visit(ClassicRungeKuttaMethod classicRungeKuttaMethod)
        {
            Result = "Classic Runge-Kutta Method";
        }
    }
}