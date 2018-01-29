using System;
using System.Drawing;

namespace CpuMerge
{
    public class RainImage
    {
        private readonly Color[,] pixels;
        public Int2 Position;
        public Int2 Size;
        public readonly PreserveAspectRatio AspectRatio;
        public Int2 ImageSize => new Int2(Width, Height);
        public int Width => pixels.GetLength(0);
        public int Height => pixels.GetLength(1);
        public readonly bool SelfBoundingBox;

        public Color this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return Color.Empty;
                return pixels[x, y];
            }
            set
            {

                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    return;
                pixels[x, y] = value;

            }
        }

        internal RainImage Copy()
        {
            return new RainImage(Position, Size, pixels, AspectRatio);
        }

        public enum PreserveAspectRatio
        {
            Exact = 0,
            Preserve = 1,
            Fill = 2,
            Coordinates = 3
        }


        public RainImage(Int2 position, Int2 size, Color[,] pixels, PreserveAspectRatio aspectRatio = PreserveAspectRatio.Coordinates)
        {
            Position = position;
            Size = size;
            this.pixels = pixels;
            AspectRatio = aspectRatio;
            SelfBoundingBox = true;
        }
        public RainImage(Int2 position, Int2 size, string fileLocation, PreserveAspectRatio aspectRatio = PreserveAspectRatio.Coordinates)
        {
            Position = position;
            Size = size;
            Bitmap image = new Bitmap(fileLocation);
            pixels = new Color[image.Width, image.Height];
            for (int y = 0; y < pixels.GetLength(1); y++)
            for (int x = 0; x < pixels.GetLength(0); x++)
                pixels[x, y] = image.GetPixel(x, y);
            AspectRatio = aspectRatio;
            SelfBoundingBox = true;
        }
        public RainImage(PreserveAspectRatio aspectRatio, string fileLocation)
        {
            if (aspectRatio == PreserveAspectRatio.Coordinates)
                throw new ArgumentOutOfRangeException("Use other constructors for this argument.");
            AspectRatio = aspectRatio;
            Bitmap image = new Bitmap(fileLocation);
            pixels = new Color[image.Width, image.Height];
            for (int y = 0; y < pixels.GetLength(1); y++)
            for (int x = 0; x < pixels.GetLength(0); x++)
                    pixels[x, y] = image.GetPixel(x, y);
            Position = default(Int2);
            Size = new Int2(pixels.GetLength(0), pixels.GetLength(1));
            SelfBoundingBox = false;
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