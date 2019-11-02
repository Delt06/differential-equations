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
                var slopeLeft = RightSideFunction(x, y);
                var slopeRight = RightSideFunction(x + step, y + step * slopeLeft);
                var slopeIdeal = 0.5d * (slopeLeft + slopeRight);
                y += step * slopeIdeal;
                x += step;
            }

            return y;
        }
    }
}