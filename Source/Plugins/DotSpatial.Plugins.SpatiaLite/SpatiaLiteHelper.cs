// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using DotSpatial.Data;
using DotSpatial.Projections;
using NetTopologySuite.IO;

namespace DotSpatial.Plugins.SpatiaLite
{
    /// <summary>
    /// Helper class with SpatiaLite specific operations.
    /// </summary>
    public class SpatiaLiteHelper
    {
        #region Fields

        private readonly bool _version4Plus;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatiaLiteHelper"/> class.
        /// This is used to hide the default constructor because Open should be used so validation checks can be run before creating a SpatiaLiteHelper.
        /// </summary>
        private SpatiaLiteHelper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatiaLiteHelper"/> class.
        /// </summary>
        /// <param name="connectionString">Connectionstring used to work with the database.</param>
        /// <param name="version4Plus">Indicates that the this is a spatialite database of version 4 or higher.</param>
        private SpatiaLiteHelper(string connectionString, bool version4Plus)
            : this()
        {
            ConnectionString = connectionString;
            _version4Plus = version4Plus;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the connection string of the connected database.
        /// </summary>
        public string ConnectionString { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Trys to open the database of the given connection string. This fails if either the spatialite version or the geometry_columns table can't be found.
        /// </summary>
        /// <param name="connectionString">Connectionstring used to work with the database.</param>
        /// <param name="error">The variable that is used to return the error message. This is empty if there was no error.</param>
        /// <returns>Null on error otherwise the SpatiaLiteHelper connected to the database.</returns>
        public static SpatiaLiteHelper Open(string connectionString, out string error)
        {
            if (!CheckSpatiaLiteSchema(connectionString))
            {
                error = Resources.CouldNotFindTheGeometryColumnsTable;
                return null;
            }

            if (!GetSpatialVersion(connectionString, out bool version4Plus))
            {
                error = Resources.CouldNotFindTheSpatiaLiteVersion;
                return null;
            }

            error = string.Empty;
            return new SpatiaLiteHelper(connectionString, version4Plus);
        }

        /// <summary>
        /// Sets the environment variables so that SpatiaLite can find the dll's.
        /// </summary>
        /// <returns>true if successful.</returns>
        public static bool SetEnvironmentVars()
        {
            try
            {
                var pathVar = Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.User);
                var sqLitePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (sqLitePath == null) return false;

                var pathVariableExists = false;
                if (!string.IsNullOrEmpty(pathVar))
                {
                    if (pathVar.ToLower().Contains(sqLitePath.ToLower() + ";")) pathVariableExists = true;
                }

                if (!pathVariableExists)
                {
                    pathVar = pathVar + ";" + sqLitePath;
                    Environment.SetEnvironmentVariable("path", pathVar, EnvironmentVariableTarget.User);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return false;
            }

            return false;
        }

        /// <summary>
        /// Finds all column names in the database table.
        /// </summary>
        /// <param name="tableName">table name.</param>
        /// <returns>list of all column names.</returns>
        public List<string> GetColumnNames(string tableName)
        {
            var columnNameList = new List<string>();

            var qry = $"PRAGMA table_info({tableName})";
            using (var cmd = CreateCommand(ConnectionString, qry))
            {
                cmd.Connection.Open();
                var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    columnNameList.Add(r["name"].ToString());
                }

                cmd.Connection.Close();
            }

            return columnNameList;
        }

        /// <summary>
        /// Finds a list of all valid geometry columns in the database.
        /// </summary>
        /// <returns>the list of geometry columns.</returns>
        public List<GeometryColumnInfo> GetGeometryColumns()
        {
            if (_version4Plus)
            {
                var lst = new List<GeometryColumnInfo>();
                var sql = "SELECT f_table_name, f_geometry_column, geometry_type, srid FROM geometry_columns";
                using (var cmd = CreateCommand(ConnectionString, sql))
                {
                    cmd.Connection.Open();

                    var r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        var gci = new GeometryColumnInfo
                        {
                            TableName = Convert.ToString(r["f_table_name"]),
                            GeometryColumnName = Convert.ToString(r["f_geometry_column"]),
                            Srid = Convert.ToInt32(r["srid"]),
                            SpatialIndexEnabled = false
                        };


                        if (int.TryParse(r["geometry_type"].ToString(), out int geometryType))
                        {
                            AssignGeometryTypeStringAndCoordDimension(geometryType, gci);
                        }

                        lst.Add(gci);
                    }

                    cmd.Connection.Close();
                }

                return lst;
            }
            else
            {
                var lst = new List<GeometryColumnInfo>();
                var sql = "SELECT f_table_name, f_geometry_column, type, coord_dimension, srid, spatial_index_enabled FROM geometry_columns";
                using (var cmd = CreateCommand(ConnectionString, sql))
                {
                    cmd.Connection.Open();

                    var r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        var gci = new GeometryColumnInfo
                        {
                            TableName = Convert.ToString(r["f_table_name"]),
                            GeometryColumnName = Convert.ToString(r["f_geometry_column"]),
                            GeometryType = Convert.ToString(r["type"]),
                            Srid = Convert.ToInt32(r["srid"]),
                            SpatialIndexEnabled = false
                        };

                        try
                        {
                            gci.CoordDimension = Convert.ToInt32(r["coord_dimension"]);
                        }
                        catch (Exception)
                        {
                            var coords = Convert.ToString(r["coord_dimension"]);
                            switch (coords)
                            {
                                case "XY":
                                    gci.CoordDimension = 2;
                                    break;
                                case "XYZ":
                                case "XYM":
                                    gci.CoordDimension = 3;
                                    break;
                                case "XYZM":
                                    gci.CoordDimension = 4;
                                    break;
                            }
                        }

                        lst.Add(gci);
                    }

                    cmd.Connection.Close();
                }

                return lst;
            }
        }

        /// <summary>
        /// Finds all table names in the SpatiaLite database.
        /// </summary>
        /// <returns>a list of all table names.</returns>
        public List<string> GetTableNames()
        {
            var tableNameList = new List<string>();

            using (var cnn = new SQLiteConnection(ConnectionString))
            {
                var qry = "SELECT name FROM sqlite_master WHERE type = 'table'";
                using var cmd = new SQLiteCommand(qry, cnn);
                cmd.Connection.Open();
                var r = cmd.ExecuteReader();
                while (r.Read())
                {
                    tableNameList.Add(r[0].ToString());
                }

                cmd.Connection.Close();
            }

            return tableNameList;
        }

        /// <summary>
        /// Reads the complete feature set from the database.
        /// </summary>
        /// <param name="featureSetInfo">information about the table.</param>
        /// <returns>the resulting feature set.</returns>
        public IFeatureSet ReadFeatureSet(GeometryColumnInfo featureSetInfo)
        {
            var sql = $"SELECT * FROM '{featureSetInfo.TableName}'";
            return ReadFeatureSet(featureSetInfo, sql);
        }

        /// <summary>
        /// Reads the complete feature set from the database.
        /// </summary>
        /// <param name="featureSetInfo">information about the table.</param>
        /// <param name="sql">the sql query.</param>
        /// <returns>the resulting feature set.</returns>
        public IFeatureSet ReadFeatureSet(GeometryColumnInfo featureSetInfo, string sql)
        {
            var fType = GetGeometryType(featureSetInfo.GeometryType);
            SpatiaLiteFeatureSet fs = new (fType)
            {
                IndexMode = true, // setting the initial index mode..
                Name = featureSetInfo.TableName,
                Filename = SqLiteHelper.GetSqLiteFileName(ConnectionString),
                LayerName = featureSetInfo.TableName
            };

            using var cmd = CreateCommand(ConnectionString, sql);
            cmd.Connection.Open();

            var wkbr = new GaiaGeoReader();

            var rdr = cmd.ExecuteReader();

            var columnNames = PopulateTableSchema(fs, featureSetInfo.GeometryColumnName, rdr);
            while (rdr.Read())
            {
                var wkb = rdr[featureSetInfo.GeometryColumnName] as byte[];
                var geom = wkbr.Read(wkb);

                var newFeature = fs.AddFeature(geom);

                // populate the attributes
                foreach (var colName in columnNames)
                {
                    newFeature.DataRow[colName] = rdr[colName];
                }
            }

            cmd.Connection.Close();

            // assign projection
            if (featureSetInfo.Srid > 0)
            {
                var proj = ProjectionInfo.FromEpsgCode(featureSetInfo.Srid);
                fs.Projection = proj;
            }

            return fs;
        }

        /// <summary>
        /// Reads the feature set with the given name from the database.
        /// </summary>
        /// <param name="tableName">Name of the table whose content should be loaded.</param>
        /// <returns>Null if the table wasn't found otherwise the tables content.</returns>
        public IFeatureSet ReadFeatureSet(string tableName)
        {
            var geos = GetGeometryColumns();
            var geo = geos.FirstOrDefault(_ => _.TableName == tableName);

            return geo == null ? null : ReadFeatureSet(geo);
        }

        /// <summary>
        /// Gets the GeometryType string and the CoordDimension from the given geometryTypeInt and assigns them to the given GeometryColumnInfo.
        /// </summary>
        /// <param name="geometryTypeInt">geometryTypeInt that indicates the GeometryType and CoordDimension that was used.</param>
        /// <param name="gci">The GeometryColumnInfo the values get assigned to.</param>
        private static void AssignGeometryTypeStringAndCoordDimension(int geometryTypeInt, GeometryColumnInfo gci)
        {
            var dimensionBase = geometryTypeInt / 1000;

            geometryTypeInt -= dimensionBase * 1000; // get only the last number

            gci.GeometryType = geometryTypeInt switch
            {
                1 => "point",
                2 => "linestring",
                3 => "polygon",
                4 => "multipoint",
                5 => "multilinestring",
                6 => "multipolygon",
                7 => "geometrycollection",
                _ => "geometry",
            };

            if (dimensionBase >= 3)
            {
                gci.CoordDimension = 4;
            }
            else if (dimensionBase >= 1)
            {
                gci.CoordDimension = 3;
            }
            else
            {
                gci.CoordDimension = 2;
            }
        }

        /// <summary>
        /// Returns true if the schema is a valid schema (has a geometry_columns table).
        /// </summary>
        /// <param name="connString">The connectionstring for the database.</param>
        /// <returns>True, if the schema has a geometry_columns table.</returns>
        private static bool CheckSpatiaLiteSchema(string connString)
        {
            var qry = "SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'geometry_columns'";

            using var cmd = CreateCommand(connString, qry);
            var result = false;
            cmd.Connection.Open();
            var obj = cmd.ExecuteScalar();
            if (obj != null) result = true;
            cmd.Connection.Close();

            return result;
        }

        /// <summary>
        /// Creates a SQLite command.
        /// </summary>
        /// <param name="conString">connection string.</param>
        /// <param name="cmdText">command text.</param>
        /// <returns>the SpatiaLite command object.</returns>
        private static SQLiteCommand CreateCommand(string conString, string cmdText)
        {
            var con = new SQLiteConnection(conString);
            return new SQLiteCommand(cmdText, con);
        }

        private static FeatureType GetGeometryType(string geometryTypeStr)
        {
            return geometryTypeStr.ToLower() switch
            {
                "point" or "multipoint" => FeatureType.Point,
                "linestring" or "multilinestring" => FeatureType.Line,
                "polygon" or "multipolygon" => FeatureType.Polygon,
                _ => FeatureType.Unspecified,
            };
        }

        /// <summary>
        /// Gets the spatialite version.
        /// </summary>
        /// <param name="connectionString">The ConnectionString used to connect to the database.</param>
        /// <param name="version4Plus">Indicates whether this database has a structur belonging to version 4.0+.</param>
        /// <returns>True, if a version indicator could be found.</returns>
        private static bool GetSpatialVersion(string connectionString, out bool version4Plus)
        {
            bool retval = false;
            version4Plus = false;

            var qry = "PRAGMA table_info(geometry_columns)";
            using (var cmd = CreateCommand(connectionString, qry))
            {
                cmd.Connection.Open();
                var r = cmd.ExecuteReader();

                while (r.Read())
                {
                    if (r["name"].ToString() == "geometry_type")
                    {
                        version4Plus = true;
                        retval = true;
                        break;
                    }

                    if (r["name"].ToString() == "type")
                    {
                        retval = true;
                        break;
                    }
                }

                cmd.Connection.Close();
            }

            return retval;
        }

        private static string[] PopulateTableSchema(IFeatureSet fs, string geomColumnName, SQLiteDataReader rdr)
        {
            var schemaTable = rdr.GetSchemaTable();
            var columnNameList = new List<string>();

            if (schemaTable != null)
            {
                foreach (DataRow r in schemaTable.Rows)
                {
                    if (r["ColumnName"].ToString() != geomColumnName)
                    {
                        var colName = Convert.ToString(r["ColumnName"]);
                        var colDataType = Convert.ToString(r["DataType"]);
                        var t = Type.GetType(colDataType);
                        if (t != null)
                        {
                            fs.DataTable.Columns.Add(colName, t);
                            columnNameList.Add(colName);
                        }
                    }
                }
            }

            return columnNameList.ToArray();
        }

        #endregion
    }
}