using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace CpuMerge
{
    public struct RainImage
    {
        private readonly Color[,] Pixels;
        public readonly Int2 Position;
        public readonly Int2 Size;
        public readonly PreserveAspectRatio AspectRatio;
        public int Width => Pixels.GetLength(0);
        public int Height => Pixels.GetLength(1);
        public Color this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return Color.Empty;
                return Pixels[x, y];
            }
            set
            {

                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return;
                Pixels[x, y] = value;

            }
        }
        public enum PreserveAspectRatio
        {
            Exact = 0,
            Preserve = 1,
            Fill = 2,
            Coordinates = 3
        }


        public RainImage(Int2 position, Int2 size, Color[,] pixels)
        {
            Position = position;
            Size = size;
            Pixels = pixels;
            AspectRatio = PreserveAspectRatio.Coordinates;
        }
        public RainImage(PreserveAspectRatio aspectRatio, Color[,] pixels)
        {
            if (aspectRatio == PreserveAspectRatio.Coordinates)
                throw new ArgumentOutOfRangeException("Use other constructors for this argument.");
            AspectRatio = aspectRatio;
            Pixels = pixels;
            Position = default(Int2);
            Size = new Int2(Pixels.GetLength(0), Pixels.GetLength(1));
        }
        public RainImage(Int2 position, Int2 size, string fileLocation)
        {
            Position = position;
            Size = size;
            Bitmap image = new Bitmap(fileLocation);
            Pixels = new Color[image.Width, image.Height];
            for (int y = 0; y < Pixels.GetLength(1); y++)
            for (int x = 0; x < Pixels.GetLength(0); x++)
                Pixels[x, y] = image.GetPixel(x, y);
            AspectRatio = PreserveAspectRatio.Coordinates;
        }
        public RainImage(PreserveAspectRatio aspectRatio, string fileLocation)
        {
            if (aspectRatio == PreserveAspectRatio.Coordinates)
                throw new ArgumentOutOfRangeException("Use other constructors for this argument.");
            AspectRatio = aspectRatio;
            Bitmap image = new Bitmap(fileLocation);
            Pixels = new Color[image.Width, image.Height];
            for (int y = 0; y < Pixels.GetLength(1); y++)
            for (int x = 0; x < Pixels.GetLength(0); x++)
                    Pixels[x, y] = image.GetPixel(x, y);
            Position = default(Int2);
            Size = new Int2(Pixels.GetLength(0), Pixels.GetLength(1));
        }


        public Color GetPixel(Int2 location, bool antialiasing = true)
        {
            if (OutOfBounds(location))
                return Color.Empty;
            Float2 relative = new Float2((float)(location.X - Position.X) / Size.X, (float)(location.Y - Position.Y) / Size.Y);
            Float2 scaled = new Float2(relative.X * Width, relative.Y * Height);
            if (!antialiasing)
                return Pixels[(int)Math.Round(scaled.X), (int)Math.Round(scaled.Y)];
            Int2 inArray = new Int2((int)scaled.X, (int)scaled.Y);
            Float2 betweenArray = new Float2(scaled.X % 1, scaled.Y % 1);
            Color mean = new Color();
            mean = ColorUtilities.AddColor(mean, Pixels[inArray.X, inArray.Y], (1 - betweenArray.X) * (1 - betweenArray.Y));
            mean = ColorUtilities.AddColor(mean, Pixels[inArray.X + 1, inArray.Y], betweenArray.X * (1 - betweenArray.Y));
            mean = ColorUtilities.AddColor(mean, Pixels[inArray.X + 1, inArray.Y + 1], betweenArray.X * betweenArray.Y);
            mean = ColorUtilities.AddColor(mean, Pixels[inArray.X, inArray.Y + 1], (1 - betweenArray.X) * betweenArray.Y);
            return mean;
        }
        private bool OutOfBounds(Int2 location)
        {
            return location.X < Position.X || location.Y < Position.Y || location.X >= Position.X + Size.X ||
                   location.Y >= Position.Y + Size.Y;
        }

        public static explicit operator Bitmap(RainImage v)
        {
            Bitmap map = new Bitmap(v.Width, v.Height);
            for (int y = 0; y < map.Height; y++)
            for (int x = 0; x < map.Width; x++)
                map.SetPixel(x, y, v[x, y]);
            return map;
        }
    }
}