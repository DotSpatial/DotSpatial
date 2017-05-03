// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/8/2008 11:25:56 AM
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
    /// A provider for shapefile layers.
    /// </summary>
    public class ShapefileLayerProvider : ILayerProvider
    {
        #region Properties

        /// <summary>
        /// Gets a basic description that will fall next to your plugin in the Add Other Data dialog.
        /// This will only be shown if your plugin does not supply a DialogReadFilter.
        /// </summary>
        public virtual string Description => "This data provider gives a simple version of .shp reading for now.";

        /// <summary>
        /// Gets a dialog read filter that lists each of the file type descriptions and file extensions, delimeted
        /// by the | symbol. Each will appear in DotSpatial's open file dialog filter, preceeded by the name provided
        /// on this object.
        /// </summary>
        public virtual string DialogReadFilter => "Shapefiles (*.shp)|*.shp";

        /// <summary>
        /// Gets a dialog filter that lists each of the file type descriptions and extensions for a Save File Dialog.
        /// Each will appear in DotSpatial's open file dialog filter, preceeded by the name provided on this object.
        /// </summary>
        public virtual string DialogWriteFilter => "Shapefiles (*.shp)|*.shp";

        /// <summary>
        /// Gets a prefereably short name that identifies this data provider. Example might be GDAL.
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        public virtual string Name => "DotSpatial";

        #endregion

        #region Methods

        /// <summary>
        /// This create new method implies that this provider has the priority for creating a new file.
        /// An instance of the dataset should be created and then returned. By this time, the fileName
        /// will already be checked to see if it exists, and deleted if the user wants to overwrite it.
        /// </summary>
        /// <param name="fileName">The string fileName for the new instance</param>
        /// <param name="featureType">Point, Line, Polygon etc. Sometimes this will be specified, sometimes it will be "Unspecified"</param>
        /// <param name="inRam">Boolean, true if the dataset should attempt to store data entirely in ram</param>
        /// <param name="container">The container for this layer. This can be null.</param>
        /// <param name="progressHandler">An IProgressHandler for status messages.</param>
        /// <returns>An IRaster</returns>
        public IFeatureLayer CreateNew(string fileName, FeatureType featureType, bool inRam, ICollection<ILayer> container, IProgressHandler progressHandler)
        {
            ShapefileDataProvider dp = new ShapefileDataProvider();
            if (progressHandler == null) progressHandler = LayerManager.DefaultLayerManager.ProgressHandler;
            IFeatureSet fs = dp.CreateNew(fileName, featureType, inRam, progressHandler);

            if (fs.FeatureType == FeatureType.Line)
            {
                return new MapLineLayer(fs, container);
            }

            if (fs.FeatureType == FeatureType.Polygon)
            {
                return new MapPolygonLayer(fs, container);
            }

            if (fs.FeatureType == FeatureType.Point || fs.FeatureType == FeatureType.MultiPoint)
            {
                return new MapPointLayer(fs, container);
            }

            return null;
        }

        /// <summary>
        /// Opens a shapefile, but returns it as a FeatureLayer
        /// </summary>
        /// <param name="fileName">The string fileName</param>
        /// <param name="inRam">Boolean, if this is true it will attempt to open the entire layer in memory.</param>
        /// <param name="container">A container to hold this layer.</param>
        /// <param name="progressHandler">The progress handler that should receive status messages</param>
        /// <returns>An IFeatureLayer</returns>
        public ILayer OpenLayer(string fileName, bool inRam, ICollection<ILayer> container, IProgressHandler progressHandler)
        {
            ShapefileDataProvider dp = new ShapefileDataProvider();
            IFeatureSet fs = dp.Open(fileName);

            if (fs != null)
            {
                if (fs.FeatureType == FeatureType.Line)
                {
                    return new MapLineLayer(fs, container);
                }

                if (fs.FeatureType == FeatureType.Polygon)
                {
                    return new MapPolygonLayer(fs, container);
                }

                if (fs.FeatureType == FeatureType.Point || fs.FeatureType == FeatureType.MultiPoint)
                {
                    return new MapPointLayer(fs, container);
                }
            }

            return null;
        }

        #endregion
    }
}