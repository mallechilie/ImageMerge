/*
  Copyright (C) 2014 Birunthan Mohanathas

  This program is free software; you can redistribute it and/or
  modify it under the terms of the GNU General Public License
  as published by the Free Software Foundation; either version 2
  of the License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/

using System;
using System.Runtime.InteropServices;
using Rainmeter;

// Overview: This example demonstrates a basic implementation of a parent/child
// measure structure. In this particular example, we have a "parent" measure
// which contains the values for the options "ValueA", "ValueB", and "ValueC".
// The child measures are used to return a specific value from the parent.

// Use case: You could, for example, have a "main" parent measure that queries
// information some data set. The child measures can then be used to return
// specific information from the data queried by the parent measure.

// Sample skin:
/*
    [Rainmeter]
    Update=1000
    BackgroundMode=2
    SolidColor=000000

    [mParent]
    Measure=Plugin
    Plugin=ParentChild.dll
    ValueA=111
    ValueB=222
    ValueC=333
    Type=A

    [mChild1]
    Measure=Plugin
    Plugin=ParentChild.dll
    ParentName=mParent
    Type=B

    [mChild2]
    Measure=Plugin
    Plugin=ParentChild.dll
    ParentName=mParent
    Type=C

    [Text]
    Meter=STRING
    MeasureName=mParent
    MeasureName2=mChild1
    MeasureName3=mChild2
    X=5
    Y=5
    W=200
    H=55
    FontColor=FFFFFF
    Text="mParent: %1#CRLF#mChild1: %2#CRLF#mChild2: %3"
*/

namespace PluginParentChild
{
    internal class Measure
    {
        internal readonly string Name;
        internal readonly bool DynamicVariables;
        internal IntPtr Skin;
        protected API Api;


        internal Measure(API api)
        {
            this.Api = api;
            Name = api.GetMeasureName();
            Skin = api.GetSkin();
            DynamicVariables = api.ReadInt("DynamicVariables", 0) == 1;
        }
        internal static Measure MeasureFromData(API api, IntPtr data)
        {
            switch (api.ReadString("Type", ""))
            {
                case "ImageMaskMeasure":
                    return (ImageMaskMeasure)GCHandle.FromIntPtr(data).Target;
                case "ImageMeasure":
                    return (ImageMeasure)GCHandle.FromIntPtr(data).Target;
                case "ImageResultMeasure":
                    return (ImageBaseMeasure)GCHandle.FromIntPtr(data).Target;
                default:
                    return (Measure)GCHandle.FromIntPtr(data).Target;
            }
        }

        /// <summary>
        /// Called after creation and before each Update if dynamicvariables = 1.
        /// </summary>
        /// <param name="api">Insert the api to make sure updated information is loaded.</param>
        internal void Reload(API api)
        {
            this.Api = api;
            Reload();
        }
        protected virtual void Reload()
        {
        }

        internal virtual double Update()
        {
            return 0.0;
        }

        internal virtual void Dispose()
        {
        }
    }
}
