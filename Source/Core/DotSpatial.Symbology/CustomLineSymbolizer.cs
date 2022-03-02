// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// CustomLineSymbolizer.
    /// </summary>
    [Serializable]
    public class CustomLineSymbolizer : CustomSymbolizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLineSymbolizer"/> class.
        /// </summary>
        public CustomLineSymbolizer()
        {
            Symbolizer = new LineSymbolizer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLineSymbolizer"/> class.
        /// </summary>
        /// <param name="uniqueName">the unique name.</param>
        /// <param name="name">the name of the custom symbolizer.</param>
        /// <param name="category">the map category of the custom symbolizer.</param>
        /// <param name="symbolizer">the associated line symbolizer.</param>
        public CustomLineSymbolizer(string uniqueName, string name, string category, LineSymbolizer symbolizer)
        {
            UniqueName = uniqueName;
            Name = name;
            Category = category;
            base.Symbolizer = symbolizer;
        }

        /// <summary>
        /// Gets or sets the line symbolizer.
        /// </summary>
        /// <remarks>// Editor(typeof(LineSymbolizerEditor), typeof(UITypeEditor)).</remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new ILineSymbolizer Symbolizer
        {
            get { return base.Symbolizer as ILineSymbolizer; }
            set { base.Symbolizer = value; }
        }
    }
}