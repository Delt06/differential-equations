using DEAssignment.Methods.Visitors;

namespace DEAssignment.Methods
{
    public sealed class EulerMethod : ApproximatedMethod
    {
        public EulerMethod(RightSideFunction rightSideFunction) : 
            base(rightSideFunction)
        {
        }

        protected override void AcceptVisitorInternal(IMethodVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override double GetValue(double step, Ivp ivp, int i)
        {
            var (x, y) = ivp;   

            for (var j = 1; j <= i; j++)
            {
                y += step * RightSideFunction(x, y);
                x += step;
            }

            return y;
        }
    }
}