// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2008 9:25:23 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    public interface IVectorLayerProvider : ILayerProvider
    {
        /// <summary>
        /// This create new method implies that this provider has the priority for creating a new file.
        /// An instance of the dataset should be created and then returned.  By this time, the fileName
        /// will already be checked to see if it exists, and deleted if the user wants to overwrite it.
        /// </summary>
        /// <param name="fileName">The string fileName for the new instance</param>
        /// <param name="featureType">Point, Line, Polygon etc.  Sometimes this will be specified, sometimes it will be "Unspecified"</param>
        /// <param name="inRam">Boolean, true if the dataset should attempt to store data entirely in ram</param>
        ///<param name="container">The container for this layer.  This can be null.</param>
        /// <param name="progressHandler">An IProgressHandler for status messages.</param>
        /// <returns>An IRaster</returns>
        IFeatureLayer CreateNew(string fileName, FeatureType featureType, bool inRam, ICollection<ILayer> container, IProgressHandler progressHandler);
    }
}