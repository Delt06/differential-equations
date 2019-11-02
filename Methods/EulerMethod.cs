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
                var yPrime = RightSideFunction(x, y);
                y += step * yPrime;
                x += step;
            }

            return y;
        }
    }
}