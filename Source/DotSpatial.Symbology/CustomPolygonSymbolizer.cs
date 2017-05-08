// ********************************************************************************************************
// Product Name: DotSpatial.Drawing.PredefinedSymbols.dll Alpha
// Description:  The basic module for DotSpatial.Drawing.PredefinedSymbols version 6.0
// ********************************************************************************************************
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
    /// <summary>
    /// A custom polygon symbolizer.
    /// </summary>
    [Serializable]
    public class CustomPolygonSymbolizer : CustomSymbolizer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPolygonSymbolizer"/> class.
        /// </summary>
        public CustomPolygonSymbolizer()
        {
            Symbolizer = new PolygonSymbolizer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPolygonSymbolizer"/> class.
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Polygon symbolizer
        /// </summary>
        /// <remarks>
        /// [Editor(typeof(PolygonSymbolizerEditor), typeof(UITypeEditor))]
        /// </remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new IPolygonSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as IPolygonSymbolizer;
            }

            set
            {
                base.Symbolizer = value;
            }
        }

        #endregion
    }
}