// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/14/2009 4:23:47 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Serialization;
using System;

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