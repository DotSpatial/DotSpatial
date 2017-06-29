// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/27/2010 9:39:13 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// MapTileLayer
    /// </summary>
    public class MapTileLayer : Layer, IMapLayer
    {
        #region IMapLayer Members

        /// <summary>
        /// This is a place holder for future stuff
        /// </summary>
        /// <param name="args"></param>
        /// <param name="regions"></param>
        public void DrawRegions(MapArgs args, List<Extent> regions)
        {
            // To get information on the pixel resolution you can use
            //  Rectangle mapRegion = args.ImageRectangle;
            // Handle tile management here based on geographic extent and pixel resolution from mapRegion
        }

        #endregion
    }
}