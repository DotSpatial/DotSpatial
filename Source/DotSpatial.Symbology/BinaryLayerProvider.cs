// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/8/2008 10:52:42 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// BinaryLayerProvider
    /// </summary>
    public class BinaryLayerProvider : IRasterLayerProvider
    {
        #region Properties

        /// <summary>
        /// Gets the basic description that will fall next to your plugin in the Add Other Data dialog.
        /// This will only be shown if your plugin does not supply a DialogReadFilter.
        /// </summary>
        public virtual string Description => "This layer provider opens a new binary raster and returns an appropriate ILayer";

        /// <summary>
        /// Gets a dialog read filter that lists each of the file type descriptions and file extensions, delimeted
        /// by the | symbol. Each will appear in DotSpatial's open file dialog filter, preceeded by the name provided
        /// on this object.
        /// </summary>
        public virtual string DialogReadFilter => "Binary Files (*.bgd)|*.bgd";

        /// <summary>
        /// Gets a dialog filter that lists each of the file type descriptions and extensions for a Save File Dialog.
        /// Each will appear in DotSpatial's open file dialog filter, preceeded by the name provided on this object.
        /// </summary>
        public virtual string DialogWriteFilter => "Binary Files (*.bgd)|*.bgd";

        /// <summary>
        /// Gets a prefereably short name that identifies this data provider. Example might be GDAL.
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        public virtual string Name => "DotSpatial";

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new BinaryRasterLayer
        /// </summary>
        /// <param name="name">The string fileName for the new instance.</param>
        /// <param name="driverCode">The string short name of the driver for creating the raster.</param>
        /// <param name="xSize">The number of columns in the raster.</param>
        /// <param name="ySize">The number of rows in the raster.</param>
        /// <param name="numBands">The number of bands to create in the raster.</param>
        /// <param name="dataType">The data type to use for the raster.</param>
        /// <param name="options">The options to be used.</param>
        /// <returns>The created IRasterLayer.</returns>
        public IRasterLayer Create(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options)
        {
            IRaster raster = Raster.CreateRaster(name, driverCode, xSize, ySize, numBands, dataType, options);
            return new RasterLayer(raster);
        }

        /// <summary>
        /// Opens an existing raster and returns a layer containing it
        /// </summary>
        /// <param name="fileName">The string fileName to open</param>
        /// <param name="inRam">Opens in ram</param>
        /// <param name="container">A container to automatically add this layer to</param>
        /// <param name="progressHandler">Returns progress</param>
        /// <returns>An ILayer</returns>
        public ILayer OpenLayer(string fileName, bool inRam, ICollection<ILayer> container, IProgressHandler progressHandler)
        {
            IRaster raster = Raster.OpenFile(fileName, inRam, progressHandler);
            RasterLayer rl = new RasterLayer(raster, progressHandler);
            container.Add(rl);
            return rl;
        }

        #endregion
    }
}