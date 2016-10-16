// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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

namespace DotSpatial.Data
{
    /// <summary>
    /// IFeatureProvider
    /// </summary>
    public interface IVectorProvider : IDataProvider
    {
        /// <summary>
        /// This create new method implies that this provider has the priority for creating a new file.
        /// An instance of the dataset should be created and then returned.  By this time, the fileName
        /// will already be checked to see if it exists, and deleted if the user wants to overwrite it.
        /// </summary>
        /// <param name="fileName">The string fileName for the new instance</param>
        /// <param name="featureType">Point, Line, Polygon etc.  Sometimes this will be specified, sometimes it will be "Unspecified"</param>
        /// <param name="inRam">Boolean, true if the dataset should attempt to store data entirely in ram</param>
        /// <param name="progressHandler">An IProgressHandler for status messages.</param>
        /// <returns>An IRaster</returns>
        IFeatureSet CreateNew(string fileName, FeatureType featureType, bool inRam, IProgressHandler progressHandler);

        /// <summary>
        /// This tests the specified file in order to determine what type of vector the file contains.
        /// This returns unspecified if the file format is not supported by this provider.
        /// </summary>
        /// <param name="fileName">The string fileName to test</param>
        /// <returns>A FeatureType clarifying what sort of features are stored on the data type.</returns>
        FeatureType GetFeatureType(string fileName);
    }
}