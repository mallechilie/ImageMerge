using System;
using System.Drawing;
using CpuMerge;
using Rainmeter;

namespace PluginParentChild
{
    internal class ImageMaskMeasure : ParentMeasure
    {
        //TODO: assign image, mask, result.
        private ImageMeasure image;
        private string imagePath;
        private ImageMeasure mask;
        private string maskPath;
        private ImageBaseMeasure result;
        public ImageBaseMeasure Result
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


        internal ImageMaskMeasure(API api) : base(api)
        {

        }


        protected override void Reload()
        {
            base.Reload();
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

        internal override double Update()
        {
            // Check if anything changed within image or mask.
            result = Result;
            return base.Update();
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