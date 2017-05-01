// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/17/2008 1:37:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    public interface IMapLabelLayer : ILabelLayer, IMapLayer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the feature layer that this label layer is attached to.
        /// </summary>
        new IMapFeatureLayer FeatureLayer { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Resolves ambiguity
        /// </summary>
        new void Invalidate();

        #endregion
    }
}