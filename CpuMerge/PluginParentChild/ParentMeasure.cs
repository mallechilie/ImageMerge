using System.Collections.Generic;
using Rainmeter;

namespace PluginParentChild
{
    internal class ParentMeasure : Measure
    {
        // This list of all parent measures is used by the child measures to find their parent.
        internal static readonly List<ParentMeasure> ParentMeasures = new List<ParentMeasure>();
        protected List<ChildMeasure> Children;

        internal ParentMeasure(API api) : base(api)
        {
            Children = new List<ChildMeasure>();
            ParentMeasures.Add(this);
        }

        internal override void Dispose()
        {
            ParentMeasures.Remove(this);
        }
    }
}