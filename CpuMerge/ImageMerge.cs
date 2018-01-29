using System;
using System.Drawing;

namespace CpuMerge
{
    public class ImageMerger
    {
        public enum CombineMode { Mask, Add, Mean }
        private readonly CombineMode mode;
        private readonly Color[,] first;
        private readonly Color[,] second;
        public  RainImage Target;
        private Sampler sampler;

        public ImageMerger(RainImage first, RainImage second, RainImage target, CombineMode mode = CombineMode.Mask)
        {
            Target = target;
            this.mode = mode;
            sampler = new Sampler(first, second, target, true);
            Color[][,] arrays = sampler.GetColors();
            this.first = arrays[0]; 
            this.second = arrays[1]; 
            Merge();
        }


        private void Merge()
        {
            for (int y = Target.Position.Y; y < Target.Size.Y + Target.Position.Y; y++)
                for (int x = Target.Position.X; x < Target.Size.X + Target.Position.X; x++)
                    MergePixel(new Int2(x, y));
        }
        private void MergePixel(Int2 position)
        {
            Color firstColor = first[position.X, position.Y];
            Color secondColor = second[position.X, position.Y];
            Target[position.X , position.Y] = Combine(firstColor, secondColor);
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
}
