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
using DotSpatial.Data.Rasters.GdalExtension;
using DotSpatial.Topology;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Data.OgrExtension
{
    /// <summary>
    /// OgrVectorProvider
    /// </summary>
    public class OgrVectorProvider : IVectorProvider
    {
        #region Private Variables

        private IProgressHandler _prog;

        #endregion

        #region Constructors

        static OgrVectorProvider()
        {
            GdalConfiguration.ConfigureOgr();
        }

        #endregion

        #region IVectorProvider Members

        /// <summary>
        /// Description of the Vector Provider
        /// </summary>
        public string Description
        {
            get { return "GDAL/OGR Vector"; }
        }

        /// <summary>
        /// The dialog filter to use when opening a file
        /// </summary>
        public string DialogReadFilter
        {
            get { return "OGR Vectors|*.shp;*.kml;*.dxf"; }
        }

        /// <summary>
        /// The dialog filter to use when saving to a file
        /// </summary>
        public string DialogWriteFilter
        {
            get { return String.Empty; }
        }

        /// <summary>
        /// Updated with progress information
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get { return _prog; }
            set { _prog = value; }
        }

        /// <summary>
        /// The name of the provider
        /// </summary>
        public string Name
        {
            get { return "Ogr Vector Provider"; }
        }

        IDataSet IDataProvider.Open(string fileName)
        {
            return Open(fileName);
        }

        public IFeatureSet CreateNew(string fileName, FeatureType featureType, bool inRam, IProgressHandler progressHandler)
        {
            throw new NotImplementedException();
        }

        public FeatureType GetFeatureType(string fileName)
        {
            using (var reader = new OgrDataReader(fileName))
            {
                return reader.GetFeatureType();
            }
        }

        #endregion

        /// <summary>
        /// Opens the specified file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
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

                var wkbReader = new WkbReader();
                while (reader.Read())
                {
                    var wkbGeometry = (byte[]) reader["Geometry"];

                    var geometry = wkbReader.Read(wkbGeometry);

                    IFeature feature = new Feature(geometry);
                    feature.DataRow = fs.DataTable.NewRow();
                    for (int i = 1; i < reader.FieldCount; i++)
                    {
                        object value = reader[i];
                        if (value == null)
                        {
                            value = DBNull.Value;
                        }
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
    }
}