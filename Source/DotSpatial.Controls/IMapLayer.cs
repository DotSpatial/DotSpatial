// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/8/2008 4:44:51 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    public interface IMapLayer : ILayer
    {
        /// <summary>
        /// This draws content from the specified geographic regions onto the specified graphics
        /// object specified by MapArgs.
        /// </summary>
        void DrawRegions(MapArgs args, List<Extent> regions);
    }
}