// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/14/2009 4:23:47 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    [Serializable]
    public class ImageSymbolizer : LegendItem, IImageSymbolizer
    {
        #region Constructors
        public ImageSymbolizer()
        {
            Opacity = 1;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a float value from 0 to 1, where 1 is fully opaque while 0 is fully transparent.
        /// </summary>
        [Serialize("Opacity")]
        public float Opacity { get; set; }
        #endregion
    }
}