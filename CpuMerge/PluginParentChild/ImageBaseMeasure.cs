using CpuMerge;
using Rainmeter;

namespace PluginParentChild
{
    internal class ImageBaseMeasure : ChildMeasure
    {
        internal string Path;
        internal Int2 Position;
        internal Int2 Size;

        internal ImageBaseMeasure(API api) : base(api)
        {
        }

        protected override void Reload()
        {
            base.Reload();

            int x = Api.ReadInt("X", 0);
            int y = Api.ReadInt("Y", 0);
            Position = new Int2(x, y);

            int w = Api.ReadInt("W", 0);
            int h = Api.ReadInt("H", 0);
            Size = new Int2(w, h);
            if (w == 0 || h == 0)
                Api.Log(API.LogType.Warning, $"ImageMeasure.dll: Size={Size} could be unuseful");

            // TODO: check rules around api.ReadString("Measure", "")
            string path = Api.ReadPath("ImagePath", "");
            string imageName = path == "" ? Api.ReadPath("ImageName", "") : Api.ReadString("ImageName", "");
            Path = path + imageName;
            if (Path.Length == 0)
                //TODO: make sure this is logged when using images too.                
                Api.Log(API.LogType.Error, $"ImageMeasure.dll: Path={Path} is not a valid path");
        }
    }
}