using System;

namespace CpuMerge
{
    public struct Float2
    {
        public readonly float X, Y;
        public Float2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
        #region operators
        public static Float2 operator /(Float2 a, float b)
        {
            return Combine(a, b, (c, d) => (int)(c / d));
        }
        public static Float2 operator /(float a, Float2 b)
        {
            return Combine(a, b, (c, d) => (int)(c / d));
        }
        public static Float2 operator *(Float2 a, float b)
        {
            return Combine(a, b, (c, d) => (int)(c * d));
        }
        public static Float2 operator *(float a, Float2 b)
        {
            return Combine(a, b, (c, d) => (int)(c * d));
        }
        public static Float2 operator /(Float2 a, int b)
        {
            return Combine(a, b, (c, d) => c / d);
        }
        public static Float2 operator /(int a, Float2 b)
        {
            return Combine(a, b, (c, d) => c / d);
        }
        public static Float2 operator *(Float2 a, int b)
        {
            return Combine(a, b, (c, d) => c * d);
        }
        public static Float2 operator *(int a, Float2 b)
        {
            return Combine(a, b, (c, d) => c * d);
        }
        public static Float2 operator /(Float2 a, Int2 b)
        {
            return Combine(a, b, (c, d) => c / d);
        }
        public static Float2 operator /(Int2 a, Float2 b)
        {
            return Combine(a, b, (c, d) => c / d);
        }
        public static Float2 operator *(Float2 a, Int2 b)
        {
            return Combine(a, b, (c, d) => c * d);
        }
        public static Float2 operator *(Int2 a, Float2 b)
        {
            return Combine(a, b, (c, d) => c * d);
        }
        public static Float2 operator /(Float2 a, Float2 b)
        {
            return Combine(a, b, (c, d) => c / d);
        }
        public static Float2 operator *(Float2 a, Float2 b)
        {
            return Combine(a, b, (c, d) => c * d);
        }
        public static Float2 operator -(Float2 a, Float2 b)
        {
            return Combine(a, b, (c, d) => c - d);
        }
        public static Float2 operator +(Float2 a, Float2 b)
        {
            return Combine(a, b, (c, d) => c + d);
        }

        public static explicit operator Float2(Int2 a)
        {
            return new Float2(a.X, a.Y);
        }

        private static Float2 Combine<T>(Float2 a, T b, Func<float, T, float> f)
        {
            return new Float2(f(a.X, b), f(a.Y, b));
        }
        private static Float2 Combine<T>(T a, Float2 b, Func<T, float, float> f)
        {
            return new Float2(f(a, b.X), f(a, b.Y));
        }
        private static Float2 Combine(Float2 a, Float2 b, Func<float, float, float> f)
        {
            return new Float2(f(a.X, b.X), f(a.Y, b.Y));
        }
        private static Float2 Combine(Int2 a, Float2 b, Func<float, float, float> f)
        {
            return new Float2(f(a.X, b.X), f(a.Y, b.Y));
        }
        private static Float2 Combine(Float2 a, Int2 b, Func<float, float, float> f)
        {
            return new Float2(f(a.X, b.X), f(a.Y, b.Y));
        }
        #endregion
    }
}