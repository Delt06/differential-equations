using System.Drawing;

namespace DEAssignment.Methods.Visitors
{
    public class GetColorVisitor : IMethodVisitor
    {
        public Color Result { get; private set; }
        public ColorMapping Mapping { get; set; }

        public void Visit(ISolvingMethod method) => 
            Result = Mapping.FallbackColor;

        public void Visit(EulerMethod eulerMethod) => 
            Result = Mapping.EulerMethodColor;

        public void Visit(ExactMethod exactMethod) => 
            Result = Mapping.ExactMethodColor;

        public void Visit(ImprovedEulerMethod improvedEulerMethod) => 
            Result = Mapping.ImprovedEulerMethodColor;

        public void Visit(ClassicRungeKuttaMethod classicRungeKuttaMethod) => 
            Result = Mapping.ClassicRungeKuttaMethodColor;
    }
}