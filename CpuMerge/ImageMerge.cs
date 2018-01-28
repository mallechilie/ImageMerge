using System;
using System.Drawing;

namespace CpuMerge
{
    public class ImageMerger
    {
        public enum CombineMode { Mask, Add, Mean }
        private readonly CombineMode mode;
        private readonly RainImage first;
        private readonly RainImage second;
        public readonly RainImage target;

        public ImageMerger(RainImage first, RainImage second, RainImage target, CombineMode mode = CombineMode.Mask)
        {
            this.first = first;
            this.second = second;
            this.target = target;
            this.mode = mode;
            Merge();
        }


        private void Merge()
        {
            for (int y = target.Position.y; y < target.Size.y + target.Position.y; y++)
                for (int x = target.Position.x; x < target.Size.x + target.Position.x; x++)
                    MergePixel(new Int2(x, y));
        }
        private void MergePixel(Int2 position)
        {
            Color firstColor = first.GetPixel(position);
            Color secondColor = second.GetPixel(position);
            target.Pixels[position.x - target.Position.x, position.y - target.Position.y] = Combine(firstColor, secondColor);
        }
        private Color Combine(Color firstColor, Color secondColor)
        {
            switch (mode)
            {
                case CombineMode.Mask:
                    {
                        return Color.FromArgb(Math.Min(firstColor.A, secondColor.A), firstColor.R, firstColor.G,
                            firstColor.B);
                    }
                case CombineMode.Add:
                    {
                        return ColorUtilities.AddColor(firstColor, secondColor);
                    }
                case CombineMode.Mean:
                    {
                        return ColorUtilities.MeanColor(firstColor, secondColor);
                    }
            }

            return Color.Empty;
        }
    }

    public struct RainImage
    {
        public readonly Color[,] Pixels;
        public readonly Int2 Position;
        public readonly Int2 Size;

        public RainImage(Int2 position, Int2 size, Color[,] pixels)
        {
            Position = position;
            Size = size;
            Pixels = pixels;
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
        }


        public Color GetPixel(Int2 location, bool nearest = true)
        {
            if (OutOfBounds(location))
                return Color.Empty;
            Float2 relative = new Float2((float)(location.x - Position.x) / Size.x, (float)(location.y - Position.y) / Size.y);
            Float2 scaled = new Float2(relative.x * Pixels.GetLength(0), relative.y * Pixels.GetLength(1));
            if (nearest)
                return Pixels[(int)Math.Round(scaled.x), (int)Math.Round(scaled.y)];
            Int2 inArray = new Int2((int)scaled.x, (int)scaled.y);
            Float2 betweenArray = new Float2(scaled.x % 1, scaled.y % 1);
            Color mean = new Color();
            mean = ColorUtilities.AddColor(mean, Pixels[inArray.x, inArray.y], (1 - betweenArray.x) * (1 - betweenArray.y));
            mean = ColorUtilities.AddColor(mean, Pixels[inArray.x + 1, inArray.y], betweenArray.x * (1 - betweenArray.y));
            mean = ColorUtilities.AddColor(mean, Pixels[inArray.x + 1, inArray.y + 1], betweenArray.x * betweenArray.y);
            mean = ColorUtilities.AddColor(mean, Pixels[inArray.x, inArray.y + 1], (1 - betweenArray.x) * betweenArray.y);
            return mean;
        }
        private bool OutOfBounds(Int2 location)
        {
            return location.x < Position.x || location.y < Position.y || location.x > Position.x + Size.x ||
                   location.y > Position.y + Size.y;
        }

        public static explicit operator Bitmap(RainImage v)
        {
            Bitmap map = new Bitmap(v.Pixels.GetLength(0), v.Pixels.GetLength(1));
            for (int y = 0; y < map.Height; y++)
                for (int x = 0; x < map.Width; x++)
                    map.SetPixel(x, y, v.Pixels[x, y]);
            return map;
        }
    }

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
