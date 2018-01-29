using System;
using System.Drawing;

namespace CpuMerge
{
    public class Sampler
    {
        private readonly bool antialiasing;
        private RainImage basic;
        private RainImage sample;
        private RainImage target;

        public Sampler(RainImage basic, RainImage sample, RainImage target, bool antialiasing)
        {
            this.basic = basic;
            this.sample = sample;
            this.target = target;
            this.antialiasing = antialiasing;

            ResetSizeAndPosition(this.basic, this.basic.SelfBoundingBox ? this.basic.Copy() : this.target);
            ResetSizeAndPosition(this.sample, this.sample.SelfBoundingBox ? this.sample.Copy() : this.target);
        }


        private void ResetSizeAndPosition(RainImage image, RainImage basis)
        {
            switch (image.AspectRatio)
            {
                case RainImage.PreserveAspectRatio.Coordinates:
                    break;
                case RainImage.PreserveAspectRatio.Exact:
                    {
                        image.Position = basis.Position;
                        image.Size = basis.Size;
                        break;
                    }
                case RainImage.PreserveAspectRatio.Fill:
                    {
                        Float2 scaling = (Float2)basis.Size / image.ImageSize;
                        float scale = Math.Max(scaling.X, scaling.Y);
                        image.Size = (Int2)(image.ImageSize * scale);
                        image.Position = basis.Position + (basis.Size - image.Size) / 2;
                        break;
                    }
                case RainImage.PreserveAspectRatio.Preserve:
                    {
                        Float2 scaling = (Float2)basis.Size / image.ImageSize;
                        float scale = Math.Min(scaling.X, scaling.Y);
                        image.Size = (Int2)(image.ImageSize * scale);
                        image.Position = basis.Position + (basis.Size - image.Size) / 2;
                        break;
                    }
            }
        }
        public Color[][,] GetColors()
        {
            Color[][,] colors = new Color[2][,];
            colors[0] = new Color[target.Width, target.Height];
            colors[1] = new Color[target.Width, target.Height];

            for (int y = target.Position.Y; y < target.Position.Y + target.Size.Y; y++)
                for (int x = target.Position.X; x < target.Position.X + target.Size.X; x++)
                {
                    colors[0][x - target.Position.X, y - target.Position.Y] = GetPixel(new Int2(x, y), basic);
                    colors[1][x - target.Position.X, y - target.Position.Y] = GetPixel(new Int2(x, y), sample);
                }
            return colors;
        }
        private Color GetPixel(Int2 location, RainImage image)
        {
            Float2 relative = (Float2) (location - image.Position) / image.Size;
            Float2 scaled = relative * image.ImageSize;
            if (!antialiasing)
                return image[(int)Math.Round(scaled.X), (int)Math.Round(scaled.Y)];

            Int2 inArray = (Int2) scaled;
            Float2 betweenArray = new Float2(scaled.X % 1, scaled.Y % 1);

            Color mean = new Color();
            mean = ColorUtilities.AddColor(mean, image[inArray.X, inArray.Y], (1 - betweenArray.X) * (1 - betweenArray.Y));
            mean = ColorUtilities.AddColor(mean, image[inArray.X + 1, inArray.Y], betweenArray.X * (1 - betweenArray.Y));
            mean = ColorUtilities.AddColor(mean, image[inArray.X + 1, inArray.Y + 1], betweenArray.X * betweenArray.Y);
            mean = ColorUtilities.AddColor(mean, image[inArray.X, inArray.Y + 1], (1 - betweenArray.X) * betweenArray.Y);
            return mean;
        }
    }
}