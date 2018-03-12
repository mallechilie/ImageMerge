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

            string parent = api.ReadString("ParentName", "");
            Measure measure;
            if (String.IsNullOrEmpty(parent))
            {
                measure = new ParentMeasure();
            }
            else
            {
                measure = new ChildMeasure();
            }

            data = GCHandle.ToIntPtr(GCHandle.Alloc(measure));
        }

        [DllExport]
        public static void Finalize(IntPtr data)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            measure.Dispose();
            GCHandle.FromIntPtr(data).Free();
        }

        [DllExport]
        public static void Reload(IntPtr data, IntPtr rm, ref double maxValue)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            if(rm!=api.m_Rm)
                api.Log(API.LogType.Warning, $"IntPtr values of previous api ({api.m_Rm}) and this api ({rm}) are different. Will keep using the old one.");
            // TODO: check if the static API works.
            measure.Reload(api);
            //measure.Reload(new API(rm));
        }

        [DllExport]
        public static double Update(IntPtr data)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            return measure.Update(api);
        }
    }
}