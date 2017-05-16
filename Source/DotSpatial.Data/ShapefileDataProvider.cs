// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// Provider for opening shapefiles.
    /// </summary>
    public class ShapefileDataProvider : IVectorProvider
    {
        #region Properties

        /// <summary>
        /// Gets a basic description that will fall next to your plugin in the Add Other Data dialog.
        /// This will only be shown if your plugin does not supply a DialogReadFilter.
        /// </summary>
        public virtual string Description => "This data provider gives a simple version of .shp reading for now.";

        /// <summary>
        /// Gets a dialog read filter that lists each of the file type descriptions and file extensions, delimeted
        /// by the | symbol. Each will appear in DotSpatial's open file dialog filter, preceded by the name provided
        /// on this object.
        /// </summary>
        public virtual string DialogReadFilter => "Shapefiles (*.shp)|*.shp;*.shx;*.dbf";

        /// <summary>
        /// Gets a dialog filter that lists each of the file type descriptions and extensions for a Save File Dialog.
        /// Each will appear in DotSpatial's open file dialog filter, preceded by the name provided on this object.
        /// </summary>
        public virtual string DialogWriteFilter => "Shapefiles (*.shp)|*.shp";

        /// <summary>
        /// Gets a preferably short name that identifies this data provider. Example might be GDAL.
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        public virtual string Name => "DotSpatial.Shapefile";

        /// <summary>
        /// Gets or sets the progress handler
        /// </summary>
        public IProgressHandler ProgressHandler { get; set; }

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
        /// <param name="progressHandler">An IProgressHandler for status messages.</param>
        /// <returns>An IRaster</returns>
        public virtual IFeatureSet CreateNew(string fileName, FeatureType featureType, bool inRam, IProgressHandler progressHandler)
        {
            switch (featureType)
            {
                case FeatureType.Point:
                    return new PointShapefile
                           {
                               Filename = fileName
                           };
                case FeatureType.Line:
                    return new LineShapefile
                           {
                               Filename = fileName
                           };
                case FeatureType.Polygon:
                    return new PolygonShapefile
                           {
                               Filename = fileName
                           };
                case FeatureType.MultiPoint:
                    return new MultiPointShapefile
                           {
                               Filename = fileName
                           };
                default: return null;
            }
        }

        /// <summary>
        /// This tests the specified file in order to determine what type of vector the file contains.
        /// This returns unspecified if the file format is not supported by this provider.
        /// </summary>
        /// <param name="fileName">The string fileName to test</param>
        /// <returns>A FeatureType clarifying what sort of features are stored on the data type.</returns>
        public virtual FeatureType GetFeatureType(string fileName)
        {
            ShapefileHeader sh = new ShapefileHeader(fileName);
            if (sh.ShapeType == ShapeType.Polygon || sh.ShapeType == ShapeType.PolygonM || sh.ShapeType == ShapeType.PolygonZ)
            {
                return FeatureType.Polygon;
            }

            if (sh.ShapeType == ShapeType.PolyLine || sh.ShapeType == ShapeType.PolyLineM || sh.ShapeType == ShapeType.PolyLineZ)
            {
                return FeatureType.Line;
            }

            if (sh.ShapeType == ShapeType.Point || sh.ShapeType == ShapeType.PointM || sh.ShapeType == ShapeType.PointZ)
            {
                return FeatureType.Point;
            }

            if (sh.ShapeType == ShapeType.MultiPoint || sh.ShapeType == ShapeType.MultiPointM || sh.ShapeType == ShapeType.MultiPointZ)
            {
                return FeatureType.MultiPoint;
            }

            return FeatureType.Unspecified;
        }

        /// <summary>
        /// This open method is only called if this plugin has been given priority for one
        /// of the file extensions supported in the DialogReadFilter property supplied by
        /// this control. Failing to provide a DialogReadFilter will result in this plugin
        /// being added to the list of DataProviders being supplied under the Add Other Data
        /// option in the file menu.
        /// </summary>
        /// <param name="fileName">A string specifying the complete path and extension of the file to open.</param>
        /// <returns>The opend feature set.</returns>
        public virtual IFeatureSet Open(string fileName)
        {
            return Shapefile.OpenFile(fileName);
        }

        /// <summary>
        /// Opens the given file.
        /// </summary>
        /// <param name="fileName">A string specifying the complete path and extension of the file to open.</param>
        /// <returns>The opened dataset.</returns>
        IDataSet IDataProvider.Open(string fileName)
        {
            return Open(fileName);
        }

        #endregion
    }
}