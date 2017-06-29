// ********************************************************************************************************
// Product Name: DotSpatial.Drawing.PredefinedSymbols.dll Alpha
// Description:  The basic module for DotSpatial.Drawing.PredefinedSymbols version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from DotSpatial.Drawing.PredefinedSymbols.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/22/2009 2:49:57 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;

namespace DotSpatial.Symbology
{
    [Serializable]
    public class CustomPolygonSymbolizer : CustomSymbolizer
    {
        /// <summary>
        /// Creates a new CustomSymbolizer for symbolizing Polygons
        /// </summary>
        public CustomPolygonSymbolizer()
        {
            Symbolizer = new PolygonSymbolizer();
        }

        /// <summary>
        /// Creates a new Custom Polygon symbolizer with the specified properties
        /// </summary>
        /// <param name="uniqueName">the unique name</param>
        /// <param name="name">the name of the custom symbolizer</param>
        /// <param name="category">the map category of the custom symbolizer</param>
        /// <param name="symbolizer">the associated Polygon symbolizer</param>
        public CustomPolygonSymbolizer(string uniqueName, string name, string category, PolygonSymbolizer symbolizer)
        {
            UniqueName = uniqueName;
            Name = name;
            Category = category;
            base.Symbolizer = symbolizer;
        }

        /// <summary>
        /// Gets or sets the Polygon symbolizer
        /// </summary>
        /// <remarks>
        /// [Editor(typeof(PolygonSymbolizerEditor), typeof(UITypeEditor))]
        /// </remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new IPolygonSymbolizer Symbolizer
        {
            get { return base.Symbolizer as IPolygonSymbolizer; }
            set { base.Symbolizer = value; }
        }
    }
}