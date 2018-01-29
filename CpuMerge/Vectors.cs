using System;

namespace CpuMerge
{
    public struct Int2
    {
        public readonly int X, Y;
        public Int2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
        #region operators
        public static Float2 operator /(Int2 a, float b)
        {
            return Combine(a, b, (c, d) => (c / d));
        }
        public static Float2 operator /(float a, Int2 b)
        {
            return Combine(a, b, (c, d) => (c / d));
        }
        public static Float2 operator *(Int2 a, float b)
        {
            return Combine(a, b, (c, d) => (c * d));
        }
        public static Float2 operator *(float a, Int2 b)
        {
            return Combine(a, b, (c, d) => (c * d));
        }
        public static Int2 operator /(Int2 a, int b)
        {
            return Combine(a, b, (c, d) => c / d);
        }
        public static Int2 operator /(int a, Int2 b)
        {
            return Combine(a, b, (c, d) => c / d);
        }
        public static Int2 operator *(Int2 a, int b)
        {
            return Combine(a, b, (c, d) => c * d);
        }
        public static Int2 operator *(int a, Int2 b)
        {
            return Combine(a, b, (c, d) => c * d);
        }
        public static Int2 operator -(Int2 a, Int2 b)
        {
            return Combine(a, b, (c, d) => c - d);
        }
        public static Int2 operator +(Int2 a, Int2 b)
        {
            return Combine(a, b, (c, d) => c + d);
        }

        public static explicit operator Int2(Float2 a)
        {
            return new Int2((int)a.X, (int)a.Y);
        }

        private static Int2 Combine<T>(Int2 a, T b, Func<int, T, int> f)
        {
            return new Int2(f(a.X, b), f(a.Y, b));
        }
        private static Int2 Combine<T>(T a, Int2 b, Func<T, int, int> f)
        {
            return new Int2(f(a, b.X), f(a, b.Y));
        }
        private static Int2 Combine(Int2 a, Int2 b, Func<int, int, int> f)
        {
            return new Int2(f(a.X, b.X), f(a.Y, b.Y));
        }
        private static Float2 Combine(Int2 a, float b, Func<int, float, float> f)
        {
            return new Float2(f(a.X, b), f(a.Y, b));
        }
        private static Float2 Combine(float a, Int2 b, Func<float, int, float> f)
        {
            return new Float2(f(a, b.X), f(a, b.Y));
        }
        #endregion
    }

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
