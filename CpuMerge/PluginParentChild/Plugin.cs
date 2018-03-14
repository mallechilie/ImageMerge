using System;
using System.Runtime.InteropServices;
using Rainmeter;

namespace PluginParentChild
{
    public static class Plugin
    {
        private static API api;

        [DllExport]
        public static void Initialize(ref IntPtr data, IntPtr rm)
        {
            api = new API(rm);
            
            Measure measure;
            switch (api.ReadString("Type", ""))
            {
                case "ImageMaskMeasure":
                {
                    measure = new ImageMaskMeasure(api);
                    break;
                }
                case "ImageMeasure":
                {
                    measure = new ImageMeasure(api);
                    break;
                }
                case "ImageResultMeasure":
                {
                    measure = new ImageBaseMeasure(api);
                    break;
                }
                default:
                {
                    measure = new Measure(api);
                    break;
                }
            }
            data = GCHandle.ToIntPtr(GCHandle.Alloc(measure));
        }

        [DllExport]
        public static void Finalize(IntPtr data)
        {
            GCHandle.FromIntPtr(data).Free();
        }

        [DllExport]
        public static void Reload(IntPtr data, IntPtr rm, ref double maxValue)
        {
            api = new API(rm);
            Measure measure = Measure.MeasureFromData(rm, data);
            measure.Reload(rm);
        }

        [DllExport]
        public static double Update(IntPtr data)
        {
            Measure measure = Measure.MeasureFromData(api, data);

            return 0.0;
        }

        [DllExport]
        public static IntPtr GetString(IntPtr data)
        {
            Measure measure = Measure.MeasureFromData(api, data);

            return Marshal.StringToHGlobalUni(""); //returning IntPtr.Zero will result in it not being used
        }

        //[DllExport]
        //public static void ExecuteBang(IntPtr data, [MarshalAs(UnmanagedType.LPWStr)]String args)
        //{
        //    Measure measure = (Measure)data;
        //}

        //[DllExport]
        //public static IntPtr (IntPtr data, int argc,
        //    [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 1)] string[] argv)
        //{
        //    Measure measure = (Measure)data;
        //
        //    return Marshal.StringToHGlobalUni(""); //returning IntPtr.Zero will result in it not being used
        //}
    }
}