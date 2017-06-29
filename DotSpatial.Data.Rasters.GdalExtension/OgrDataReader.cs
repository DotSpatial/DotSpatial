// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using DotSpatial.Data.Rasters.GdalExtension;
using DotSpatial.Projections;
using DotSpatial.Topology;
using OSGeo.OGR;
using OSGeo.OSR;
using Geometry = OSGeo.OGR.Geometry;

namespace DotSpatial.Data
{
    /// <summary>
    ///  OgrDatareader provide readonly forward only access to OsGeo data sources accessible though the OGR project
    /// </summary>
    public class OgrDataReader : IDataReader
    {
        #region private fields

        private OSGeo.OGR.Feature _currentFeature;
        private const string FidFieldName = "FID";
        private const int FidFieldOrdinal = 1;
        private const string GeometryFieldName = "Geometry";
        private const int GeometryFieldOrdinal = 0;
        private readonly int _iFieldCount;
        // CGX private -> protected
        protected readonly DataSource _ogrDataSource;
        private readonly FeatureDefn _ogrFeatureDefinition;
        private readonly Layer _ogrLayer;
        private DS_DataTable _schemaTable; // CGX AERO GLZ
        private bool bClosed = true;

        #endregion

        #region constructors

        static OgrDataReader()
        {
            GdalConfiguration.ConfigureOgr();
        }

        public OgrDataReader(string sDataSource)
            :this(sDataSource, null)
        {
        }

        public OgrDataReader(string sDataSource, string sLayer)
        {
            _ogrDataSource = Ogr.Open(sDataSource, 0);
            _ogrLayer = sLayer != null
                ? _ogrDataSource.GetLayerByName(sLayer)
                : _ogrDataSource.GetLayerByIndex(0);

            _ogrFeatureDefinition = _ogrLayer.GetLayerDefn();
            _iFieldCount = _ogrFeatureDefinition.GetFieldCount();
            BuildSchemaTable();
            _currentFeature = null;
            bClosed = false;
        }

        #endregion

        #region Public Methods

        public FeatureType GetFeatureType()
        {
            wkbGeometryType geomType = _ogrFeatureDefinition.GetGeomType();
            return TranslateOgrGeometryType(geomType);
        }

        public ProjectionInfo GetProj4ProjectionInfo()
        {
            string sProj4String;

            //SpatialReference osrSpatialref = _ogrLayer.GetSpatialRef();
            // CGX
            OSGeo.OSR.SpatialReference osrSpatialref = _ogrLayer.GetSpatialRef();
            osrSpatialref.ExportToProj4(out sProj4String);
            return ProjectionInfo.FromProj4String(sProj4String);
        }

        #endregion

        #region private methods

        private byte[] GetGeometry()
        {
            Geometry ogrGeometry = _currentFeature.GetGeometryRef();
            ogrGeometry.FlattenTo2D();
            Byte[] wkbGeometry = new Byte[ogrGeometry.WkbSize()];

            ogrGeometry.ExportToWkb(wkbGeometry, wkbByteOrder.wkbXDR);

            return wkbGeometry;

            ////            System.Data.SqlTypes.SqlBytes x = new System.Data.SqlTypes.SqlBytes(wkbGeometry);
            ////            Microsoft.SqlServer.Types.SqlGeometry c = Microsoft.SqlServer.Types.SqlGeometry.STGeomFromWKB(x, 1111);
            ////             Microsoft.SqlServer.Types.SqlGeometry q = c.STExteriorRing();
            ////            MemoryStream ms = new MemoryStream( wkbGeometry );
            ////            BinaryReader rdr = new BinaryReader(ms);
            ////            Microsoft.SqlServer.Types.SqlGeometry g = new Microsoft.SqlServer.Types.SqlGeometry();
            ////            g.Read( rdr );
            //return geometry;
        }

        private Type TranslateOgrType(FieldType ogrType)
        {
            switch (ogrType)
            {
                case FieldType.OFTInteger:
                    return typeof(Int32);

                case FieldType.OFTReal:
                    return typeof(Double);

                case FieldType.OFTWideString:
                case FieldType.OFTString:
                    return typeof(String);

                case FieldType.OFTBinary:
                    return typeof(Byte[]);

                case FieldType.OFTDate:
                case FieldType.OFTTime:
                case FieldType.OFTDateTime:
                    return typeof(DateTime);

                case FieldType.OFTIntegerList:
                case FieldType.OFTRealList:
                case FieldType.OFTStringList:
                case FieldType.OFTWideStringList:
                    throw new NotSupportedException("Type not supported: " + ogrType.ToString());

                default:
                    throw new NotSupportedException("Type not supported: " + ogrType.ToString());
            }
        }

        private FeatureType TranslateOgrGeometryType(wkbGeometryType ogrType)
        {
            switch (ogrType)
            {
                case wkbGeometryType.wkbLineString:
                    return FeatureType.Line;

                case wkbGeometryType.wkbPolygon:
                    return FeatureType.Polygon;

                case wkbGeometryType.wkbPoint:
                    return FeatureType.Point;

                case wkbGeometryType.wkbMultiPoint:
                    return FeatureType.MultiPoint;

                default:
                    return FeatureType.Unspecified;
            }
        }

        private DataColumn AddDataColumn(IDataTable table, string columnName, Type type) // CGX AERO GLZ
        {
            DataColumn column = new DataColumn(columnName, type);
            table.Columns.Add(column);
            return column;
        }

        private void AddDataColumn(IDataTable table, string columnName, Type type, object defaultValue) // CGX AERO GLZ
        {
            DataColumn column = AddDataColumn(table, columnName, type);
            column.DefaultValue = defaultValue;
        }

        private IDataRow AddDataRow(IDataTable table, string name, int ordinal, int size, Type type, bool allowNull, bool isUnique, bool isKey) // CGX AERO GLZ
        {
            IDataRow row = table.NewRow(); // CGX AERO GLZ
            row[SchemaTableColumn.ColumnName] = name;
            row[SchemaTableColumn.ColumnOrdinal] = ordinal;
            row[SchemaTableColumn.DataType] = type;
            row[SchemaTableColumn.ColumnSize] = size;
            row[SchemaTableColumn.AllowDBNull] = allowNull;
            row[SchemaTableColumn.IsUnique] = isUnique;
            row[SchemaTableColumn.IsKey] = isKey;
            row[SchemaTableColumn.BaseColumnName] = row[SchemaTableColumn.ColumnName];
            table.Rows.Add(row);
            return row;
        }

        private IDataTable BuildSchemaTable() // CGX AERO GLZ
        {
            _schemaTable = new DS_DataTable("SchemaTable"); // CGX AERO GLZ
            _schemaTable.Locale = CultureInfo.InvariantCulture;

            AddDataColumn(_schemaTable, SchemaTableColumn.ColumnName, typeof(string));
            AddDataColumn(_schemaTable, SchemaTableColumn.ColumnOrdinal, typeof(int));
            AddDataColumn(_schemaTable, SchemaTableColumn.ColumnSize, typeof(int), -1);
            AddDataColumn(_schemaTable, SchemaTableColumn.NumericPrecision, typeof(short));
            AddDataColumn(_schemaTable, SchemaTableColumn.NumericScale, typeof(short));
            AddDataColumn(_schemaTable, SchemaTableColumn.DataType, typeof(Type));
            AddDataColumn(_schemaTable, SchemaTableColumn.ProviderType, typeof(int));
            AddDataColumn(_schemaTable, SchemaTableColumn.IsLong, typeof(bool), false);
            AddDataColumn(_schemaTable, SchemaTableColumn.AllowDBNull, typeof(bool), true);
            AddDataColumn(_schemaTable, SchemaTableColumn.IsUnique, typeof(bool), false);
            AddDataColumn(_schemaTable, SchemaTableColumn.IsKey, typeof(bool), false);
            AddDataColumn(_schemaTable, SchemaTableColumn.BaseSchemaName, typeof(string));
            AddDataColumn(_schemaTable, SchemaTableColumn.BaseTableName, typeof(string), String.Empty);
            AddDataColumn(_schemaTable, SchemaTableColumn.BaseColumnName, typeof(string));

            AddDataRow(_schemaTable, GeometryFieldName, GeometryFieldOrdinal, 1, typeof(byte[]), true, false, false);
            AddDataRow(_schemaTable, FidFieldName, FidFieldOrdinal, 1, typeof(long), false, true, true);

            for (int i = 0; i < _iFieldCount; i++)
            {
                FieldDefn f = _ogrFeatureDefinition.GetFieldDefn(i);

                AddDataRow(_schemaTable, f.GetName(), i + 2, f.GetWidth(), TranslateOgrType(f.GetFieldType()), true, false, false);
            }

            _schemaTable.AcceptChanges();

            return _schemaTable;
        }

        #endregion

        #region IDataReader interface members

        /// <summary>
        /// Closes the reader.
        /// </summary>
        public void Close()
        {
            bClosed = true;
        }

        /// <summary>
        /// Gets the integer depth, which is always 0.
        /// </summary>
        public int Depth
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the DataTable with the schema.
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchemaTable() // CGX AERO GLZ
        {
            return _schemaTable.Table; // CGX AERO GLZ
        }

        /// <summary>
        /// Gets a value indicating whether this reader is closed.
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return bClosed;
            }
        }

        /// <summary>
        /// Gets a value indicating the next result?  This is always false for some reason.
        /// </summary>
        /// <returns></returns>
        public bool NextResult()
        {
            return false;
        }

        /// <summary>
        /// This method tries to read another value and returns false if it fails.
        /// </summary>
        /// <returns>Boolean</returns>
        public bool Read()
        {
            _currentFeature = _ogrLayer.GetNextFeature();
            return _currentFeature != null;
        }

        /// <summary>
        /// The integer number of records affected.  (This always returns -1)
        /// </summary>
        public int RecordsAffected
        {
            get { return -1; }
        }

        /// <summary>
        /// This disposes the underlying data source.
        /// </summary>
        public void Dispose()
        {
            _ogrDataSource.Dispose();
        }

        /// <summary>
        /// Gets the integer number of fields in the table.  In this case the number of attribute fields plus 2.
        /// </summary>
        public int FieldCount
        {
            get
            {
                return _iFieldCount + 2;
            }
        }

        /// <summary>
        /// Returns the i'th field cast to a boolean value.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool GetBoolean(int i)
        {
            return _currentFeature.GetFieldAsInteger(i - 2) != 0;
        }

        /// <summary>
        /// Returns the i'th field cast to a DateTime.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public DateTime GetDateTime(int i)
        {
            return GetFieldAsDateTime(i - 2);
        }

        public string GetDataTypeName(int i)
        {
            if (i == 1)
            {
                return typeof(Int32).ToString();
            }
            if (i == 0)
            {
                return typeof(byte[]).ToString();
            }
            return _ogrFeatureDefinition.GetFieldDefn(i - 2).GetFieldType().ToString();
        }

        public double GetDouble(int i)
        {
            return _currentFeature.GetFieldAsDouble(i - 2);
        }

        public Type GetFieldType(int i)
        {
            if (i == 1)
            {
                return typeof(Int32);
            }
            else if (i == 0)
            {
                return typeof(byte[]);
            }
            else
            {
                return TranslateOgrType(_ogrFeatureDefinition.GetFieldDefn(i - 2).GetFieldType());
            }
        }

        public int GetInt32(int i)
        {
            return 1;
        }

        public string GetName(int i)
        {
            return (string)_schemaTable.Rows[i][SchemaTableColumn.ColumnName];
        }

        public int GetOrdinal(string name)
        {
            for (int i = 0; i < _schemaTable.Rows.Count; i++)
            {
                if (_schemaTable.Rows[i][SchemaTableColumn.ColumnName].ToString().ToLower() == name.ToLower())
                {
                    return i;
                };
            }
            return -1;
        }

        public string GetString(int i)
        {
            return _currentFeature.GetFieldAsString(i - 2);
        }

        public object GetValue(int i)
        {
            if (i == FidFieldOrdinal)
            {
                long fid = _currentFeature.GetFID();
                return fid;
            }

            if (i == GeometryFieldOrdinal)
            {
                return GetGeometry();
            }

            switch (_ogrFeatureDefinition.GetFieldDefn(i - 2).GetFieldType())
            {
                case FieldType.OFTString:
                    return _currentFeature.GetFieldAsString(i - 2);

                case FieldType.OFTInteger:
                    return _currentFeature.GetFieldAsInteger(i - 2);

                case FieldType.OFTDateTime:
                    return GetFieldAsDateTime(i);

                case FieldType.OFTReal:
                    return _currentFeature.GetFieldAsDouble(i - 2);

                default:
                    return null;
            }
        }

        public int GetValues(object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = GetValue(i);
            }

            return values.Length;
        }

        public bool IsDBNull(int i)
        {
            if (i < 2)
            {
                return false;
            }
            return _currentFeature.IsFieldSet(i - 2);
        }

        public object this[string name]
        {
            get
            {
                int i = GetOrdinal(name);
                return this[i];
            }
        }

        public object this[int i]
        {
            get
            {
                return GetValue(i);
            }
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }
        
        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        private DateTime GetFieldAsDateTime(int i)
        {
            int year;
            int month;
            int day;

            int h;
            int m;
            float s;
            int flag;

            _currentFeature.GetFieldAsDateTime(i - 2, out year, out month, out day, out h, out m, out s, out flag);
            try
            {
                return new DateTime(year, month, day, h, m, (int)s);
            }
            catch (ArgumentOutOfRangeException)
            {
                return DateTime.MinValue;
            }
        }

        #endregion
    }
}