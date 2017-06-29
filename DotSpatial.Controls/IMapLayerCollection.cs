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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/19/2008 10:26:38 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// IGeoLayerCollection
    /// </summary>
    public interface IMapLayerCollection : ILayerEventList<IMapLayer>
    {
        #region Events

        /// <summary>
        /// Occurs when a rectangular region of the buffer for any of the layers in this collection
        /// should be updated.
        /// </summary>
        event EventHandler<ClipArgs> BufferChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified fileName to the map as a new layer.
        /// </summary>
        /// <param name="fileName">The string fileName to add as a layer.</param>
        /// <returns>An IMapLayer that is the layer handle for the specified file.</returns>
        IMapLayer Add(string fileName);

        /// <summary>
        /// Adds the dataset specified to the file.  Depending on whether this is a featureSet,
        /// Raster, or ImageData, this will return the appropriate layer for the map.
        /// </summary>
        /// <param name="dataSet">A dataset</param>
        /// <returns>The IMapLayer to add</returns>
        IMapLayer Add(IDataSet dataSet);

        /// <summary>
        /// This overload automatically constructs a new MapLayer from the specified
        /// feature layer with the default drawing characteristics and returns a valid
        /// IMapLayer which can be further cast into a PointLayer, MapLineLayer or
        /// a PolygonLayer, depending on the data that is passed in.
        /// </summary>
        /// <param name="featureSet">Any valid IFeatureSet that does not yet have drawing characteristics</param>
        /// <returns>A newly created valid implementation of FeatureLayer which at least gives a few more common
        /// drawing related methods and can also be cast into the appropriate Point, Line or Polygon layer.</returns>
        IMapFeatureLayer Add(IFeatureSet featureSet);

        /// <summary>
        /// Adds the specified raster as a new layer
        /// </summary>
        /// <param name="raster">The raster to add as a layer</param>
        /// <returns>the MapRasterLayer interface</returns>
        IMapRasterLayer Add(IRaster raster);

        /// <summary>
        /// Adds the specified ImageData class to the map as a new layer and returns the newly created layer.
        /// </summary>
        /// <param name="image">The image being created</param>
        /// <returns>An interface to the newly created MapImageLayer</returns>
        IMapImageLayer Add(IImageData image);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the map frame that this belongs to.  These are designed to only work with one map frame at a time.
        /// </summary>
        IMapFrame MapFrame
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ParentGroup for this layer collection, even if that parent group
        /// is not actually a map frame.
        /// </summary>
        IMapGroup ParentGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the progress handler to report progress for time consuming actions.
        /// </summary>
        IProgressHandler ProgressHandler
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the currently active layer.
        /// </summary>
        IMapLayer SelectedLayer
        {
            get;
            set;
        }

        /// <summary>
        /// Given a base name, this increments a number for appending
        /// if the name already exists in the collection.
        /// </summary>
        /// <param name="baseName">The string base name to start with</param>
        /// <returns>The base name modified by a number making it unique in the collection</returns>
        string UnusedName(string baseName);

        #endregion
    }
}