using System;
using Rainmeter;

namespace PluginParentChild
{
    internal class ChildMeasure : Measure
    {
        protected ParentMeasure ParentMeasure
        {
            get;
            set;
        }

        internal override void Reload(API api)
        {
            base.Reload(api);

            string parentName = api.ReadString("ParentName", "");
            IntPtr skin = api.GetSkin();

            // Find parent using name AND the skin handle to be sure that it's the right one.
            ParentMeasure = null;
            foreach (ParentMeasure parentMeasure in ParentMeasure.ParentMeasures)
            {
                if (parentMeasure.Skin.Equals(skin) && parentMeasure.Name.Equals(parentName))
                {
                    if (this.ParentMeasure != null)
                        api.Log(API.LogType.Warning, $"ParentChild.dll: parentName={parentName} has multiple instances running");
                    this.ParentMeasure = parentMeasure;
                }
            }

            if (ParentMeasure == null)
            {
                api.Log(API.LogType.Error, $"ParentChild.dll: ParentName={parentName} not valid");
            }
        }
        internal override double Update(API api)
        {
            double d = base.Update(api);
            ParentMeasure.Update(api);
            return d;
        }
    }
}