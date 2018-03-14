using System;
using Rainmeter;

namespace PluginParentChild
{
    internal class ImageMeasure : ImageBaseMeasure
    {
        internal readonly PreserveAspectRatio AspectRatio;
        internal enum PreserveAspectRatio
        {
            Fit = 0, Preserve = 1, Fill = 2
        }


        public ImageMeasure(API api) : base(api)
        {
            AspectRatio = (PreserveAspectRatio)Api.ReadInt("PreserveAspectRatio", 0);
            if (!Enum.IsDefined(typeof(PreserveAspectRatio), AspectRatio))
                Api.Log(API.LogType.Error, $"ImageMeasure.dll: PreserveAspectRatio={AspectRatio} not valid");
        }
    }
}