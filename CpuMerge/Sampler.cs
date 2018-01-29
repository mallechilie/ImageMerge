using System;
using System.Drawing;

namespace CpuMerge
{
    public class Sampler
    {
        private enum Image { Basic, Sample, Target }
        private readonly bool antialiasing;
        private Int2 basicPosition;
        private Int2 basicSize;
        private readonly RainImage.PreserveAspectRatio basicAspectRatio;
        private RainImage basic;
        private Int2 samplePosition;
        private Int2 sampleSize;
        private readonly RainImage.PreserveAspectRatio sampleAspectRatio;
        private RainImage sample;
        private Int2 targetPosition;
        private Int2 targetSize;
        private RainImage target;

        public Sampler(RainImage basic, RainImage sample, RainImage target, bool antialiasing)
        {
            this.basic = basic;
            basicAspectRatio = this.basic.AspectRatio;
            basicPosition = this.basic.Position;
            basicSize = this.basic.Size;
            this.sample = sample;
            sampleAspectRatio = this.sample.AspectRatio;
            samplePosition = this.sample.Position;
            sampleSize = this.sample.Size;
            this.target = target;
            targetPosition = this.target.Position;
            targetSize = this.target.Size;
            this.antialiasing = antialiasing;

            ResetSizeAndPosition();
        }


        private void ResetSizeAndPosition()
        {
            switch (basicAspectRatio)
            {
                case RainImage.PreserveAspectRatio.Coordinates:
                    break;
                case RainImage.PreserveAspectRatio.Exact:
                    {
                        basicPosition = targetPosition;
                        basicSize = targetSize;
                        break;
                    }
                case RainImage.PreserveAspectRatio.Fill:
                    {
                        Float2 scaling = new Float2((float)targetSize.X / basicSize.X, (float)targetSize.Y / basicSize.Y);
                        float scale = Math.Max(scaling.X, scaling.Y);
                        basicSize = new Int2((int)(basicSize.X * scale), (int)(basicSize.Y * scale));
                        basicPosition = new Int2((targetSize.X - basicSize.X) / 2, (targetSize.Y - basicSize.Y) / 2);
                        break;
                    }
                case RainImage.PreserveAspectRatio.Preserve:
                    {
                        Float2 scaling = new Float2((float)targetSize.X / basicSize.X, (float)targetSize.Y / basicSize.Y);
                        float scale = Math.Min(scaling.X, scaling.Y);
                        basicSize = new Int2((int)(basicSize.X * scale), (int)(basicSize.Y * scale));
                        basicPosition = new Int2((targetSize.X - basicSize.X) / 2, (targetSize.Y - basicSize.Y) / 2);
                        break;
                    }
            }
            switch (sampleAspectRatio)
            {
                case RainImage.PreserveAspectRatio.Coordinates:
                    break;
                case RainImage.PreserveAspectRatio.Exact:
                    {
                        samplePosition = targetPosition;
                        sampleSize = targetSize;
                        break;
                    }
                case RainImage.PreserveAspectRatio.Fill:
                    {
                        Float2 scaling = new Float2((float)targetSize.X / sampleSize.X, (float)targetSize.Y / sampleSize.Y);
                        float scale = Math.Max(scaling.X, scaling.Y);
                        sampleSize = new Int2((int)(sampleSize.X * scale), (int)(sampleSize.Y * scale));
                        samplePosition = new Int2((targetSize.X - sampleSize.X) / 2, (targetSize.Y - sampleSize.Y) / 2);
                        break;
                    }
                case RainImage.PreserveAspectRatio.Preserve:
                    {
                        Float2 scaling = new Float2((float)targetSize.X / sampleSize.X, (float)targetSize.Y / sampleSize.Y);
                        float scale = Math.Min(scaling.X, scaling.Y);
                        sampleSize = new Int2((int)(sampleSize.X * scale), (int)(sampleSize.Y * scale));
                        samplePosition = new Int2((targetSize.X - sampleSize.X) / 2, (targetSize.Y - sampleSize.Y) / 2);
                        break;
                    }
            }
        }
        private bool OutOfRange(Int2 location, Image image)
        {
            switch (image)
            {
                case Image.Basic:
                    return location.X < basicPosition.X || location.Y < basicPosition.Y || location.X > basicPosition.X + basicSize.X - 1 ||
                           location.Y > basicPosition.Y + basicSize.Y - 1;
                case Image.Sample:
                    return location.X < samplePosition.X || location.Y < samplePosition.Y || location.X > samplePosition.X + sampleSize.X - 1 ||
                           location.Y > samplePosition.Y + sampleSize.Y - 1;
                case Image.Target:
                    return location.X < targetPosition.X || location.Y < targetPosition.Y || location.X > targetPosition.X + targetSize.X - 1 ||
                           location.Y > targetPosition.Y + targetSize.Y - 1;
            }
            return true;
        }
        public Color[][,] GetColors()
        {
            Color[][,] colors = new Color[2][,];
            colors[0] = new Color[targetSize.X, targetSize.Y];
            colors[1] = new Color[targetSize.X, targetSize.Y];

            for (int x = targetPosition.X; x < targetPosition.X + targetSize.X; x++)
                for (int y = targetPosition.Y; y < targetPosition.Y + targetSize.Y; y++)
                {
                    colors[0][x, y] = GetPixel(new Int2(x, y), Image.Basic);
                    colors[1][x, y] = GetPixel(new Int2(x, y), Image.Sample);
                }
            return colors;
        }
        private Color GetPixel(Int2 location, Image image)
        {
            if (OutOfRange(location, image))
                return Color.Empty;
            if (image == Image.Basic)
            {
                Float2 relative = new Float2((float) (location.X - basicPosition.X) / basicSize.X, (float) (location.Y - basicPosition.Y) / basicSize.Y);
                Float2 scaled = new Float2(relative.X * basic.Width, relative.Y * basic.Height);
                if (!antialiasing)
                    return basic[(int) Math.Round(scaled.X), (int) Math.Round(scaled.Y)];
                Int2 inArray = new Int2((int) scaled.X, (int) scaled.Y);
                Float2 betweenArray = new Float2(scaled.X % 1, scaled.Y % 1);
                Color mean = new Color();
                mean = ColorUtilities.AddColor(mean, basic[inArray.X, inArray.Y], (1 - betweenArray.X) * (1 - betweenArray.Y));
                mean = ColorUtilities.AddColor(mean, basic[inArray.X + 1, inArray.Y], betweenArray.X * (1 - betweenArray.Y));
                mean = ColorUtilities.AddColor(mean, basic[inArray.X + 1, inArray.Y + 1], betweenArray.X * betweenArray.Y);
                mean = ColorUtilities.AddColor(mean, basic[inArray.X, inArray.Y + 1], (1 - betweenArray.X) * betweenArray.Y);
                return mean;
            }
            else
            {
                Float2 relative = new Float2((float)(location.X - samplePosition.X) / sampleSize.X, (float)(location.Y - samplePosition.Y) / sampleSize.Y);
                Float2 scaled = new Float2(relative.X * sample.Width, relative.Y * sample.Height);
                if (!antialiasing)
                    return sample[(int)Math.Round(scaled.X), (int)Math.Round(scaled.Y)];
                Int2 inArray = new Int2((int)scaled.X, (int)scaled.Y);
                Float2 betweenArray = new Float2(scaled.X % 1, scaled.Y % 1);
                Color mean = new Color();
                mean = ColorUtilities.AddColor(mean, sample[inArray.X, inArray.Y], (1 - betweenArray.X) * (1 - betweenArray.Y));
                mean = ColorUtilities.AddColor(mean, sample[inArray.X + 1, inArray.Y], betweenArray.X * (1 - betweenArray.Y));
                mean = ColorUtilities.AddColor(mean, sample[inArray.X + 1, inArray.Y + 1], betweenArray.X * betweenArray.Y);
                mean = ColorUtilities.AddColor(mean, sample[inArray.X, inArray.Y + 1], (1 - betweenArray.X) * betweenArray.Y);
                return mean;
            }
        }
    }
}