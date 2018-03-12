using System;
using System.Drawing;
using CpuMerge;
using Rainmeter;

namespace PluginParentChild
{
    internal class ImageMaskMeasure : ParentMeasure
    {
        private ImageMeasure image;
        private string imagePath;
        private ImageMeasure mask;
        private string maskPath;
        private ImageResultMeasure result;
        public ImageResultMeasure Result
        {
            get
            {
                if (image.Path != imagePath || mask.Path != maskPath)
                {
                    imagePath = image.Path;
                    maskPath = mask.Path;
                    Merge();
                }
                return result;
            }
            set => result = value;
        }

        internal override void Reload(API api)
        {
            base.Reload(api);
            Merge();
        }

        private void Merge()
        {
            RainImage rainImage = new RainImage(image.Position, image.Size, image.Path, ConvertAspectRatio(image.AspectRatio));
            RainImage rainMask = new RainImage(mask.Position, mask.Size, mask.Path, ConvertAspectRatio(mask.AspectRatio));
            RainImage rainResult = new RainImage(result.Position, result.Size, new Color[result.Size.X, result.Size.Y]);

            ImageMerger merger = new ImageMerger(rainImage, rainMask, rainResult, ImageMerger.CombineMode.Mask);
            ((Bitmap)merger.Target).Save(result.Path);
        }

        internal override double Update(API api)
        {
            // Check if anything changed within image or mask.
            result = Result;
            return base.Update(api);
        }

        private static RainImage.PreserveAspectRatio ConvertAspectRatio(ImageMeasure.PreserveAspectRatio ratio)
        {
            switch (ratio)
            {
                case ImageMeasure.PreserveAspectRatio.Fit:
                    return RainImage.PreserveAspectRatio.Exact;
                case ImageMeasure.PreserveAspectRatio.Preserve:
                    return RainImage.PreserveAspectRatio.Preserve;
                case ImageMeasure.PreserveAspectRatio.Fill:
                    return RainImage.PreserveAspectRatio.Fill;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ratio), ratio, null);
            }
        }
    }
}