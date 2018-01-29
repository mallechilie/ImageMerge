using System;
using System.Drawing;

namespace CpuMerge
{
    public static class ColorUtilities
    {
        public static Color MeanColor(Color a, Color b, int weightA = 1, int weightB = 1)
        {
            return ExecuteFunction(a, b, (c, d) => (c * weightA + d * weightB) / (weightA + weightB));
        }
        public static Color AddColor(Color basis, Color added, float factor = 1)
        {
            return ExecuteFunction(basis, added, (a, b) => a + (int)(b * factor));
        }
        private static Color ExecuteFunction(Color a, Color b, Func<int, int, int> f)
        {
            // Added a range check
            int G(int c, int d) => Math.Min(255, Math.Max(0, f(c, d)));
            return Color.FromArgb(G(a.A, b.A), G(a.R, b.R), G(a.G, b.G), G(a.B, b.B));
        }
    }
}