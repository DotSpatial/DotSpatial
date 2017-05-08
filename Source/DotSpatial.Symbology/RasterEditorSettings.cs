// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/14/2009 8:50:58 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// RasterEditorSettings
    /// </summary>
    [Serializable]
    public class RasterEditorSettings : EditorSettings
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterEditorSettings"/> class.
        /// </summary>
        public RasterEditorSettings()
        {
            NumBreaks = 2;
            MaxSampleCount = 10000;
            Min = -100000;
            Max = 1000000;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the maximum value that will contribute to statistics.
        /// </summary>
        [Serialize("Max")]
        public double Max { get; set; }

        /// <summary>
        /// Gets or sets the minimum value that will contribute to statistics
        /// </summary>
        [Serialize("Min")]
        public double Min { get; set; }

        #endregion
    }
}