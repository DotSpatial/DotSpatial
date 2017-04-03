// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/4/2009 12:00:07 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    public interface IMapGroup : IGroup, IMapLayer
    {
        #region Properties

        /// <summary>
        /// Gets the IMapLayerCollection for members contained by this group.
        /// </summary>
        IMapLayerCollection Layers { get; set; }

        /// <summary>
        /// Gets the map frame for this group.
        /// </summary>
        IMapFrame ParentMapFrame { get; }

        #endregion
    }
}