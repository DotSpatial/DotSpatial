// ********************************************************************************************************
// Product Name: DotSpatial.Drawing.PredefinedSymbols.dll Alpha
// Description:  The basic module for DotSpatial.Drawing version 6.0
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
    /// A custom point symbolizer.
    /// </summary>
    [Serializable]
    public class CustomPointSymbolizer : CustomSymbolizer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPointSymbolizer"/> class.
        /// </summary>
        public CustomPointSymbolizer()
        {
            Symbolizer = new PointSymbolizer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPointSymbolizer"/> class.
        /// </summary>
        /// <param name="uniqueName">the unique name</param>
        /// <param name="name">the name of the custom symbolizer</param>
        /// <param name="category">the map category of the custom symbolizer</param>
        /// <param name="symbolizer">the associated Point symbolizer</param>
        public CustomPointSymbolizer(string uniqueName, string name, string category, PointSymbolizer symbolizer)
        {
            UniqueName = uniqueName;
            Name = name;
            Category = category;
            base.Symbolizer = symbolizer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Point symbolizer
        /// </summary>
        /// <remarks>
        /// [Editor(typeof(PointSymbolizerEditor), typeof(UITypeEditor))]
        /// </remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new IPointSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as IPointSymbolizer;
            }

            set
            {
                base.Symbolizer = value;
            }
        }

        #endregion
    }
}