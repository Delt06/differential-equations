using System.Drawing;

namespace DEAssignment.Methods.Visitors
{
    public struct ColorMapping
    {
        public Color FallbackColor { get; set; }
        public Color EulerMethodColor { get; set; }
        public Color ExactMethodColor { get; set; }
        public Color ImprovedEulerMethodColor { get; set; }
        public Color ClassicRungeKuttaColor { get; set; }
    }
}