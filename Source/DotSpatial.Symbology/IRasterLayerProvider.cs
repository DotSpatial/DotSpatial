// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/08/2008 10:54:10 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A DataProvider that is specific to raster formats.
    /// </summary>
    public interface IRasterLayerProvider : ILayerProvider
    {
        /// <summary>
        /// This create new method implies that this provider has the priority for creating a new file.
        /// An instance of the dataset should be created and then returned.  By this time, the fileName
        /// will already be checked to see if it exists, and deleted if the user wants to overwrite it.
        /// </summary>
        /// <param name="name">The string fileName for the new instance.</param>
        /// <param name="driverCode">The string short name of the driver for creating the raster.</param>
        /// <param name="xSize">The number of columns in the raster.</param>
        /// <param name="ySize">The number of rows in the raster.</param>
        /// <param name="numBands">The number of bands to create in the raster.</param>
        /// <param name="dataType">The data type to use for the raster.</param>
        /// <param name="options">The options to be used.</param>
        /// <returns>A new IRaster.</returns>
        IRasterLayer Create(string name, string driverCode, int xSize, int ySize, int numBands, Type dataType, string[] options);
    }
}