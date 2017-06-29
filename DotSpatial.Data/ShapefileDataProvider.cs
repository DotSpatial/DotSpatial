// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/21/2008 2:52:59 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// ShapefileDataProvider
    /// </summary>
    public class ShapefileDataProvider : IVectorProvider
    {
        private IProgressHandler _progressHandler;

        #region Methods

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

        IDataSet IDataProvider.Open(string fileName)
        {
            return Open(fileName);
        }

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
        public virtual IFeatureSet CreateNew(string fileName, FeatureType featureType, bool inRam, IProgressHandler progressHandler)
        {
            if (featureType == FeatureType.Point)
            {
                PointShapefile ps = new PointShapefile();
                ps.Filename = fileName;
                return ps;
            }
            else if (featureType == FeatureType.Line)
            {
                LineShapefile ls = new LineShapefile();
                ls.Filename = fileName;
                return ls;
            }
            else if (featureType == FeatureType.Polygon)
            {
                PolygonShapefile ps = new PolygonShapefile();
                ps.Filename = fileName;
                return ps;
            }
            else if (featureType == FeatureType.MultiPoint)
            {
                MultiPointShapefile mps = new MultiPointShapefile();
                mps.Filename = fileName;
                return mps;
            }

            return null;
        }

        /// <summary>
        /// This open method is only called if this plugin has been given priority for one
        /// of the file extensions supported in the DialogReadFilter property supplied by
        /// this control.  Failing to provide a DialogReadFilter will result in this plugin
        /// being added to the list of DataProviders being supplied under the Add Other Data
        /// option in the file menu.
        /// </summary>
        /// <param name="fileName">A string specifying the complete path and extension of the file to open.</param>
        /// <returns>A List of IDataSets to be added to the Map.  These can also be groups of datasets.</returns>
        public virtual IFeatureSet Open(string fileName)
        {
            return Shapefile.OpenFile(fileName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a dialog read filter that lists each of the file type descriptions and file extensions, delimeted
        /// by the | symbol.  Each will appear in DotSpatial's open file dialog filter, preceded by the name provided
        /// on this object.
        /// </summary>
        public virtual string DialogReadFilter
        {
            get { return "Shapefiles (*.shp)|*.shp;*.shx;*.dbf"; }
        }

        /// <summary>
        /// Gets a dialog filter that lists each of the file type descriptions and extensions for a Save File Dialog.
        /// Each will appear in DotSpatial's open file dialog filter, preceded by the name provided on this object.
        /// </summary>
        public virtual string DialogWriteFilter
        {
            get { return "Shapefiles (*.shp)|*.shp"; }
        }

        /// <summary>
        /// Gets a preferably short name that identifies this data provider.  Example might be GDAL.
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        public virtual string Name
        {
            get { return "DotSpatial.Shapefile"; }
        }

        /// <summary>
        /// This is a basic description that will fall next to your plugin in the Add Other Data dialog.
        /// This will only be shown if your plugin does not supply a DialogReadFilter.
        /// </summary>
        public virtual string Description
        {
            get { return "This data provider gives a simple version of .shp reading for now."; }
        }

        /// <summary>
        /// Gets or sets the progress handler
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get { return _progressHandler; }
            set { _progressHandler = value; }
        }

        #endregion
    }
}