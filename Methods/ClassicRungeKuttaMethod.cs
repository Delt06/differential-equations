using DEAssignment.Methods.Visitors;

namespace DEAssignment.Methods
{
    public sealed class ClassicRungeKuttaMethod : ApproximatedMethod
    {
        public ClassicRungeKuttaMethod(RightSideFunction rightSideFunction) : 
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
                var k1 = GetK1(step, x, y);
                var k2 = GetK2(step, x, y);
                var k3 = GetK3(step, x, y);
                var k4 = GetK4(step, x, y);

                y += (k1 + 2d * k2 + 2d * k3 + k4) / 6d;
                x += step;
            }

            return y;
        }

        private double GetK1(double step, double x, double y) => step * RightSideFunction(x, y);

        private double GetK2(double step, double x, double y) =>
            step * RightSideFunction(x + step / 2d, y + GetK1(step, x, y) / 2d);

        private double GetK3(double step, double x, double y) =>
            step * RightSideFunction(x + step / 2d, y + GetK2(step, x, y) / 2d);

        private double GetK4(double step, double x, double y) =>
            step * RightSideFunction(x + step, y + GetK3(step, x, y));
    }
}