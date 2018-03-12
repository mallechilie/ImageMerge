using System;
using Rainmeter;

namespace PluginParentChild
{
    internal class ImageMeasure : ImageBaseMeasure
    {
        internal string Path;
        internal PreserveAspectRatio AspectRatio;
        internal enum PreserveAspectRatio
        {
            Fit = 0, Preserve = 1, Fill = 2
        }

        internal override void Reload(API api)
        {
            base.Reload(api);
            // TODO: check rules around api.ReadString("Measure", "")
            string path = api.ReadPath("ImagePath", "");
            string imageName = path == "" ? api.ReadPath("ImageName", "") : api.ReadString("ImageName", "");
            Path = path + imageName;
            if (Path.Length == 0)
                //TODO: make sure this is logged when using images too.                
                api.Log(API.LogType.Error, $"ImageMeasure.dll: Path={Path} is not a valid path");

            AspectRatio = (PreserveAspectRatio)api.ReadInt("PreserveAspectRatio", 0);
            if (!Enum.IsDefined(typeof(PreserveAspectRatio), AspectRatio))
                api.Log(API.LogType.Error, $"ImageMeasure.dll: PreserveAspectRatio={AspectRatio} not valid");
        }

        internal override double Update(API api)
        {
            double d = base.Update(api);
            if (DynamicVariables)
            {
                string path = api.ReadPath("ImagePath", "");
                path += path == "" ? api.ReadPath("ImageName", "") : api.ReadString("ImageName", "");
                if (Path.Length == 0)
                    //TODO: make sure this is logged when using images too.
                    api.Log(API.LogType.Error, $"ImageMeasure.dll: Path={Path} contains no image");
                else
                    Path = path;
            }
            return d;
        }
    }
}