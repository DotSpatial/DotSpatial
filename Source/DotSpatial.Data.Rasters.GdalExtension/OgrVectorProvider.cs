// ********************************************************************************************************
// Product Name: DotSpatial.Gdal
// Description:  This is a data extension for the System.Spatial framework.
// ********************************************************************************************************
// The contents of this file are subject to the Gnu Lesser General Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from a plugin for MapWindow version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/1/2009 11:39:44 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |     Name          |    Date     |              Comments
// |-------------------|-------------|-------------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using NetTopologySuite.IO;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// OgrVectorProvider
    /// </summary>
    public class OgrVectorProvider : IVectorProvider
    {
        #region Constructors

        static OgrVectorProvider()
        {
            GdalConfiguration.ConfigureOgr();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the description of the Vector Provider.
        /// </summary>
        public string Description => "GDAL/OGR Vector";

        /// <summary>
        /// Gets the dialog filter to use when opening a file
        /// </summary>
        public string DialogReadFilter => "OGR Vectors|*.shp;*.kml;*.dxf";

        /// <summary>
        /// Gets the dialog filter to use when saving to a file.
        /// </summary>
        public string DialogWriteFilter => string.Empty;

        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        public string Name => "Ogr Vector Provider";

        /// <summary>
        /// Gets or sets the progress handler that gets updated with progress information.
        /// </summary>
        public IProgressHandler ProgressHandler { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="featureType">The feature type.</param>
        /// <param name="inRam">Indicates whether the feature set is in ram.</param>
        /// <param name="progressHandler">The progress handler.</param>
        /// <returns>The created feature set.</returns>
        public IFeatureSet CreateNew(string fileName, FeatureType featureType, bool inRam, IProgressHandler progressHandler)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the feature type.
        /// </summary>
        /// <param name="fileName">File whose feature type should be returned.</param>
        /// <returns>The feature type.</returns>
        public FeatureType GetFeatureType(string fileName)
        {
            using (var reader = new OgrDataReader(fileName))
            {
                return reader.GetFeatureType();
            }
        }

        /// <summary>
        /// Opens the specified file
        /// </summary>
        /// <param name="fileName">Name of the file that gets opened.</param>
        /// <returns>The opened file as IFeatureSet.</returns>
        public IFeatureSet Open(string fileName)
        {
            IFeatureSet fs = new FeatureSet();
            fs.Name = Path.GetFileNameWithoutExtension(fileName);
            fs.Filename = fileName;
            using (var reader = new OgrDataReader(fileName))
            {
                // skip the geometry column which is always column 0
                for (int i = 1; i < reader.FieldCount; i++)
                {
                    string sFieldName = reader.GetName(i);
                    Type type = reader.GetFieldType(i);

                    int uniqueNumber = 1;
                    string uniqueName = sFieldName;
                    while (fs.DataTable.Columns.Contains(uniqueName))
                    {
                        uniqueName = sFieldName + uniqueNumber;
                        uniqueNumber++;
                    }

                    fs.DataTable.Columns.Add(new DataColumn(uniqueName, type));
                }

                var wkbReader = new WKBReader();
                while (reader.Read())
                {
                    var wkbGeometry = (byte[])reader["Geometry"];

                    var geometry = wkbReader.Read(wkbGeometry);

                    IFeature feature = new Feature(geometry);
                    feature.DataRow = fs.DataTable.NewRow();
                    for (int i = 1; i < reader.FieldCount; i++)
                    {
                        object value = reader[i] ?? DBNull.Value;

                        feature.DataRow[i - 1] = value;
                    }

                    fs.Features.Add(feature);
                }

                try
                {
                    fs.Projection = reader.GetProj4ProjectionInfo();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                }
            }

            return fs;
        }

        /// <summary>
        /// Opens the given file.
        /// </summary>
        /// <param name="fileName">File t hat gets opened.</param>
        /// <returns>Content of the file as data set.</returns>
        IDataSet IDataProvider.Open(string fileName)
        {
            return Open(fileName);
        }

        #endregion
    }
}