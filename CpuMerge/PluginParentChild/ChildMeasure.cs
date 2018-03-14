using System;
using Rainmeter;

namespace PluginParentChild
{
    internal class ChildMeasure : Measure
    {
        protected readonly ParentMeasure ParentMeasure;

        internal ChildMeasure(API api) : base(api)
        {
            string parentName = Api.ReadString("Parent", "");
            IntPtr skin = Api.GetSkin();

            // Find parent using name AND the skin handle to be sure that it's the right one.
            ParentMeasure = null;
            foreach (ParentMeasure parentMeasure in ParentMeasure.ParentMeasures)
            {
                if (parentMeasure.Skin.Equals(skin) && parentMeasure.Name.Equals(parentName))
                {
                    if (ParentMeasure != null)
                        Api.Log(API.LogType.Warning, $"ParentChild.dll: Parent={parentName} has multiple instances running");
                    ParentMeasure = parentMeasure;
                }
            }

            if (ParentMeasure == null)
            {
                Api.Log(API.LogType.Error, $"ParentChild.dll: Parent={parentName} not valid");
            }
        }
        
        internal override double Update()
        {
            double d = base.Update();
            ParentMeasure.Update();
            return d;
        }
    }
}