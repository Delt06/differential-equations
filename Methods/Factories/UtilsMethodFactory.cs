namespace DEAssignment.Methods.Factories
{
    public class UtilsMethodFactory : IMethodFactory
    {
        public EulerMethod CreateEulerMethod()
        {
            return new EulerMethod(Utils.RightSideFunction);
        }

        public ImprovedEulerMethod CreateImprovedEulerMethod()
        {
            return new ImprovedEulerMethod(Utils.RightSideFunction);
        }

        public ExactMethod CreateExactMethod()
        {
            return new ExactMethod(Utils.Solution);
        }

        public ClassicRungeKuttaMethod CreateClassicRungeKuttaMethod()
        {
            return new ClassicRungeKuttaMethod(Utils.RightSideFunction);
        }
    }
}