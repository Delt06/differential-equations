namespace DEAssignment
{
    public readonly struct Ivp
    {
        public readonly double X0;
        public readonly double Y0;

        public Ivp(double x0, double y0)
        {
            X0 = x0;
            Y0 = y0;
        }

        public void Deconstruct(out double x0, out double y0)
        {
            x0 = X0;
            y0 = Y0;
        }
    }
}