using CpuMerge;
using Rainmeter;

namespace PluginParentChild
{
    abstract internal class ImageBaseMeasure : ChildMeasure
    {
        internal Int2 Position;
        internal Int2 Size;

        internal override void Reload(API api)
        {
            base.Reload(api);

            int x = api.ReadInt("X", 0);
            int y = api.ReadInt("Y", 0);
            Position = new Int2(x, y);

            int w = api.ReadInt("W", 0);
            int h = api.ReadInt("H", 0);
            Size = new Int2(w, h);
            if (w == 0 || h == 0)
                api.Log(API.LogType.Warning, $"ImageMeasure.dll: Size={Size} could be unuseful");
        }

        internal override double Update(API api)
        {
            double d = base.Update(api);
            return d;
        }
    }
}