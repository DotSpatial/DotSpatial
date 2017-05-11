// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Just specifies the organization of interfaces that make up a layer.
    /// It is recommended to create derived classes that inherit from an
    /// abstract layer that implements the majority of this shared functionality
    /// </summary>
    public interface ILayer : ILegendItem, IRenderable, ISelectable, IDynamicVisibility, IDisposable, IDisposeLock, IReproject
    {
        #region Events

        /// <summary>
        /// Occurs when all aspects of the layer finish loading.
        /// </summary>
        event EventHandler FinishedLoading;

        /// <summary>
        /// Occurs if this layer was selected
        /// </summary>
        event EventHandler<LayerSelectedEventArgs> LayerSelected;

        /// <summary>
        /// Occurs before the properties are actually shown, also allowing the event to be handled.
        /// </summary>
        event HandledEventHandler ShowProperties;

        /// <summary>
        /// Occurs if the maps should zoom to this layer.
        /// </summary>
        event EventHandler<EnvelopeArgs> ZoomToLayer;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the core dataset for this layer.
        /// </summary>
        IDataSet DataSet { get; set; }

        /// <summary>
        /// Gets the currently invalidated region.
        /// </summary>
        Extent InvalidRegion { get; }

        /// <summary>
        /// Gets or sets the MapFrame that contains this layer.
        /// </summary>
        IFrame MapFrame { get; set; }

        /// <summary>
        /// Gets or sets the progress handler
        /// </summary>
        IProgressHandler ProgressHandler { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Notifies the layer that the next time an area that intersects with this region
        /// is specified, it must first re-draw content to the image buffer.
        /// </summary>
        /// <param name="region">The envelope where content has become invalidated.</param>
        void Invalidate(Extent region);

        /// <summary>
        /// Queries this layer and the entire parental tree up to the map frame to determine if
        /// this layer is within the selected layers.
        /// </summary>
        /// <returns>True, if the layer is within legend selection.</returns>
        bool IsWithinLegendSelection();

        /// <summary>
        /// Given a geographic extent, this tests the "IsVisible", "UseDynamicVisibility",
        /// "DynamicVisibilityMode" and "DynamicVisibilityWidth"
        /// In order to determine if this layer is visible.
        /// </summary>
        /// <param name="geographicExtent">The geographic extent, where the width will be tested.</param>
        /// <returns>Boolean, true if this layer should be visible for this extent.</returns>
        bool VisibleAtExtent(Extent geographicExtent);

        #endregion
    }
}