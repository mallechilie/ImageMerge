using Rainmeter;

namespace PluginParentChild
{
    internal class ImageResultMeasure : ImageBaseMeasure
    {
        internal string Path;

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
        }
    }
}