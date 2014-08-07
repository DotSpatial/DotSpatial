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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/25/2008 9:37:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This map draws geographic coordinates directly to pixel coordinates
    /// </summary>
    public interface IMap : IBasicMap
    {
        #region Events


        /// <summary>
        /// Occurs after the map refreshes the image
        /// </summary>
        event EventHandler FinishedRefresh;

        /// <summary>
        /// Occurs when the map function mode has changed
        /// </summary>
        /// <remarks>Example of changing the function mode
        /// is changing from zoom mode to select mode.</remarks>
        event EventHandler FunctionModeChanged;

        /// <summary>
        /// Occurs after a resize event
        /// </summary>
        event EventHandler Resized;

        /// <summary>
        /// Occurs after a layer has been added to the mapframe, or any of the child groups of that mapframe.
        /// </summary>
        event EventHandler<LayerEventArgs> LayerAdded;

        /// <summary>
        /// Public event advertising the mouse movement
        /// </summary>
        event EventHandler<GeoMouseArgs> GeoMouseMove;

        /// <summary>
        /// Fires after the view extents have been altered and the map has redrawn to the new extents.
        /// This is an echo of the MapFrame.ViewExtentsChanged, so you only want one handler.
        /// </summary>
        event EventHandler<ExtentArgs> ViewExtentsChanged;

        /// <summary>
        /// Occurs after the projection of the map has been changed
        /// </summary>
        event EventHandler ProjectionChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new layer to the map by using an open file dialog
        /// </summary>
        /// <returns>An IMapLayer that represents the layer in the map.</returns>
        List<IMapLayer> AddLayers();

        /// <summary>
        /// Creates a new layer from the specified fileName and adds it to the map.
        /// </summary>
        /// <param name="fileName">The string fileName to add to the map</param>
        /// <returns>The newly created IMapLayer</returns>
        IMapLayer AddLayer(string fileName);

        /// <summary>
        /// Uses the file dialog to allow selection of a fileName for opening the
        /// new layer, but does not allow multiple files to be added at once.
        /// </summary>
        /// <returns>The newly opened IMapLayer</returns>
        new IMapLayer AddLayer();

        /// <summary>
        /// Allows a multi-select file dialog to add raster layers, applying a
        /// filter so that only supported raster formats will appear.
        /// </summary>
        /// <returns>A list of the IMapRasterLayers that were opened.</returns>
        List<IMapRasterLayer> AddRasterLayers();

        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster to the map as a layer, and returns the added layer.
        /// </summary>
        /// <returns>The IMapRasterLayer that was added</returns>
        IMapRasterLayer AddRasterLayer();

        /// <summary>
        /// Allows a mult-select open file dialog to specify several fileNames to add.
        /// Only files with supported vector extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapFeatureLayers</returns>
        List<IMapFeatureLayer> AddFeatureLayers();

        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster tot he map as a layer, and returns the added layer.
        /// </summary>
        /// <returns></returns>
        IMapFeatureLayer AddFeatureLayer();

        /// <summary>
        /// Allows a mult-select open file dialog to specify several fileNames to add.
        /// Only files with supported image extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapImageLayers</returns>
        List<IMapImageLayer> AddImageLayers();

        /// <summary>
        /// Allows an open dialog without multi-select to specify a single fileName
        /// to be added to the map as a new layer and returns the newly added layer.
        /// </summary>
        /// <returns>The layer that was added to the map.</returns>
        IMapImageLayer AddImageLayer();

        /// <summary>
        /// This causes all of the datalayers to re-draw themselves to the buffer, rather than just drawing
        /// the buffer itself like what happens during "Invalidate"
        /// </summary>
        void Refresh();

        /// <summary>
        /// This can be called any time, and is currently being used to capture
        /// the end of a resize event when the actual data should be updated.
        /// </summary>
        void ResetBuffer();

        /// <summary>
        ///
        /// </summary>
        void SaveLayer();

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        bool CollectAfterDraw
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Cursor.  This will be changed as the cursor mode changes.
        /// </summary>
        Cursor Cursor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dictionary of tools built into this project
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        List<IMapFunction> MapFunctions
        {
            get;
            set;
        }

        Dictionary<FunctionMode, IMapFunction> FunctionLookup
        {
            get;
            set;
        }

        /// <summary>
        /// If this is true, then point layers in the map will only draw points that are
        /// more than 50% revealed.  This should increase drawing speed for layers that have
        /// a large number of points.
        /// </summary>
        bool CollisionDetection
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a MapFrame
        /// </summary>
        new IMapFrame MapFrame
        {
            get;
            set;
        }

        /// <summary>
        /// The layers for this map
        /// </summary>
        IMapLayerCollection Layers
        {
            get;
        }

        /// <summary>
        /// Gets or sets the progress handler for this component.
        /// </summary>
        IProgressHandler ProgressHandler
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the projection.  This should reflect the projection of the first data layer loaded.
        /// Loading subsequent, but non-matching projections should throw an alert, and allow reprojection.
        /// </summary>
        ProjectionInfo Projection
        {
            get;
            set;
        }

        #endregion
    }
}