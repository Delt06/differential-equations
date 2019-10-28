using DEAssignment.Methods.Visitors;

namespace DEAssignment.Methods
{
    public sealed class ImprovedEulerMethod : ApproximatedMethod
    {
        public ImprovedEulerMethod(RightSideFunction rightSideFunction) : 
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
                var yWithCap = y + step * RightSideFunction(x, y);
                y += step * (RightSideFunction(x, y) + RightSideFunction(x + step, yWithCap)) / 2d;
                x += step;
            }

            return y;
        }
    }
}