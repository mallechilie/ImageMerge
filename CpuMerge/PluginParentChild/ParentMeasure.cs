using System.Collections.Generic;

namespace PluginParentChild
{
    internal class ParentMeasure : Measure
    {
        // This list of all parent measures is used by the child measures to find their parent.
        internal static readonly List<ParentMeasure> ParentMeasures = new List<ParentMeasure>();

        internal List<ChildMeasure> Cildren;


        internal ParentMeasure()
        {
            ParentMeasures.Add(this);
        }

        internal override void Dispose()
        {
            ParentMeasures.Remove(this);
        }
    }
}