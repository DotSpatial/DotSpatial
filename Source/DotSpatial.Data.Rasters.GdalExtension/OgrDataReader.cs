// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using DotSpatial.Projections;
using OSGeo.OGR;
using OSGeo.OSR;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    ///  OgrDatareader provide readonly forward only access to OsGeo data sources accessible though the OGR project.
    /// </summary>
    public class OgrDataReader : IDataReader
    {
        #region Fields

        private const string FidFieldName = "FID";
        private const int FidFieldOrdinal = 1;
        private const string GeometryFieldName = "Geometry";
        private const int GeometryFieldOrdinal = 0;
        private readonly int _iFieldCount;
        protected readonly DataSource _ogrDataSource; // CGX private -> protected
        private readonly FeatureDefn _ogrFeatureDefinition;
        private readonly Layer _ogrLayer;

        private OSGeo.OGR.Feature _currentFeature;
        private DataTable _schemaTable;

        #endregion

        #region Constructors

        static OgrDataReader()
        {
            GdalConfiguration.ConfigureOgr();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OgrDataReader"/> class.
        /// </summary>
        /// <param name="sDataSource">The path of the data source file.</param>
        public OgrDataReader(string sDataSource)
            : this(sDataSource, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OgrDataReader"/> class.
        /// </summary>
        /// <param name="sDataSource">The path of the data source file.</param>
        /// <param name="sLayer">Name of the layer.</param>
        public OgrDataReader(string sDataSource, string sLayer)
        {
            _ogrDataSource = Ogr.Open(sDataSource, 0);
            _ogrLayer = sLayer != null ? _ogrDataSource.GetLayerByName(sLayer) : _ogrDataSource.GetLayerByIndex(0);

            _ogrFeatureDefinition = _ogrLayer.GetLayerDefn();
            _iFieldCount = _ogrFeatureDefinition.GetFieldCount();
            BuildSchemaTable();
            _currentFeature = null;
            IsClosed = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the integer depth, which is always 0.
        /// </summary>
        public int Depth => 0;

        /// <summary>
        /// Gets the integer number of fields in the table. In this case the number of attribute fields plus 2.
        /// </summary>
        public int FieldCount => _iFieldCount + 2;

        /// <summary>
        /// Gets a value indicating whether this reader is closed.
        /// </summary>
        public bool IsClosed { get; private set; }

        /// <summary>
        /// Gets the integer number of records affected. (This always returns -1)
        /// </summary>
        public int RecordsAffected => -1;

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the value of the field with the given name.
        /// </summary>
        /// <param name="name">Name of the field.</param>
        /// <returns>Value of the field with the given name.</returns>
        public object this[string name]
        {
            get
            {
                int i = GetOrdinal(name);
                return this[i];
            }
        }

        /// <summary>
        /// Gets the value of the field.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>Value of the field.</returns>
        public object this[int i] => GetValue(i);

        #endregion

        #region Methods

        /// <summary>
        /// Closes the reader.
        /// </summary>
        public void Close()
        {
            IsClosed = true;
        }

        /// <summary>
        /// This disposes the underlying data source.
        /// </summary>
        public void Dispose()
        {
            _ogrDataSource.Dispose();
        }

        /// <summary>
        /// Returns the i'th field cast to a boolean value.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>Field as boolean.</returns>
        public bool GetBoolean(int i)
        {
            return _currentFeature.GetFieldAsInteger(i - 2) != 0;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>Field as byte.</returns>
        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i">Index.</param>
        /// <param name="fieldOffset">The field offset.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferoffset">The buffer offset</param>
        /// <param name="length">The length.</param>
        /// <returns>Bytes as long.</returns>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>Field as byte.</returns>
        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i">Index.</param>
        /// <param name="fieldoffset">The field offset.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferoffset">The buffer offset</param>
        /// <param name="length">The length.</param>
        /// <returns>Chars as long.</returns>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i">Index.</param>
        /// <returns>The data.</returns>
        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the data type of the field.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>The data type.</returns>
        public string GetDataTypeName(int i)
        {
            if (i == 1)
            {
                return typeof(int).ToString();
            }

            if (i == 0)
            {
                return typeof(byte[]).ToString();
            }

            return _ogrFeatureDefinition.GetFieldDefn(i - 2).GetFieldType().ToString();
        }

        /// <summary>
        /// Returns the i'th field cast to a DateTime.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>Field as DateTime.</returns>
        public DateTime GetDateTime(int i)
        {
            return GetFieldAsDateTime(i - 2);
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>Field as decimal.</returns>
        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>Field as double.</returns>
        public double GetDouble(int i)
        {
            return _currentFeature.GetFieldAsDouble(i - 2);
        }

        /// <summary>
        /// Gets the feature type.
        /// </summary>
        /// <returns>The feature type of the geometry.</returns>
        public FeatureType GetFeatureType()
        {
            wkbGeometryType geomType = _ogrFeatureDefinition.GetGeomType();
            return TranslateOgrGeometryType(geomType);
        }

        /// <summary>
        /// Gets the field type.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>The field type.</returns>
        public Type GetFieldType(int i)
        {
            if (i == 1)
            {
                return typeof(int);
            }

            if (i == 0)
            {
                return typeof(byte[]);
            }

            return TranslateOgrType(_ogrFeatureDefinition.GetFieldDefn(i - 2).GetFieldType());
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>Field as float.</returns>
        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>The guid.</returns>
        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>Field as short.</returns>
        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>Field as int.</returns>
        public int GetInt32(int i)
        {
            return 1;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>Field as long.</returns>
        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>The field name.</returns>
        public string GetName(int i)
        {
            return (string)_schemaTable.Rows[i][SchemaTableColumn.ColumnName];
        }

        /// <summary>
        /// Gets the column index of the field.
        /// </summary>
        /// <param name="name">Name of the field.</param>
        /// <returns>-1 if not found, otherwise the column index.</returns>
        public int GetOrdinal(string name)
        {
            for (int i = 0; i < _schemaTable.Rows.Count; i++)
            {
                if (_schemaTable.Rows[i][SchemaTableColumn.ColumnName].ToString().ToLower() == name.ToLower())
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the projection info.
        /// </summary>
        /// <returns>The projection info.</returns>
        public ProjectionInfo GetProj4ProjectionInfo()
        {
            string sProj4String;

            //SpatialReference osrSpatialref = _ogrLayer.GetSpatialRef();
			// CGX
            OSGeo.OSR.SpatialReference osrSpatialref = _ogrLayer.GetSpatialRef();
            osrSpatialref.ExportToProj4(out sProj4String);
            return ProjectionInfo.FromProj4String(sProj4String);
        }

        /// <summary>
        /// Gets the DataTable with the schema.
        /// </summary>
        /// <returns>The data table.</returns>
        public DataTable GetSchemaTable()
        {
            return _schemaTable;
        }

        /// <summary>
        /// Gets the field as string.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>Field value as string.</returns>
        public string GetString(int i)
        {
            return _currentFeature.GetFieldAsString(i - 2);
        }

        /// <summary>
        /// Gets the value of the field.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>The field value as object.</returns>
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
                case FieldType.OFTString: return _currentFeature.GetFieldAsString(i - 2);

                case FieldType.OFTInteger: return _currentFeature.GetFieldAsInteger(i - 2);

                case FieldType.OFTDateTime: return GetFieldAsDateTime(i);

                case FieldType.OFTReal: return _currentFeature.GetFieldAsDouble(i - 2);

                default: return null;
            }
        }

        /// <summary>
        /// Gets the values for the fields from 0 to values.lengths.
        /// </summary>
        /// <param name="values">Values object that gets filled with the gotten values.</param>
        /// <returns>The length of the values object.</returns>
        public int GetValues(object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = GetValue(i);
            }

            return values.Length;
        }

        /// <summary>
        /// Checks whether the field is DBNull.
        /// </summary>
        /// <param name="i">Index of the field.</param>
        /// <returns>True, if the field is DBNull.</returns>
        public bool IsDBNull(int i)
        {
            if (i < 2)
            {
                return false;
            }

            return _currentFeature.IsFieldSet(i - 2);
        }

        /// <summary>
        /// Gets a value indicating the next result? This is always false for some reason.
        /// </summary>
        /// <returns>False.</returns>
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

        private DataColumn AddDataColumn(DataTable table, string columnName, Type type)
        {
            DataColumn column = new DataColumn(columnName, type);
            table.Columns.Add(column);
            return column;
        }

        private void AddDataColumn(DataTable table, string columnName, Type type, object defaultValue)
        {
            DataColumn column = AddDataColumn(table, columnName, type);
            column.DefaultValue = defaultValue;
        }

        private DataRow AddDataRow(DataTable table, string name, int ordinal, int size, Type type, bool allowNull, bool isUnique, bool isKey)
        {
            DataRow row = table.NewRow();
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

        private DataTable BuildSchemaTable()
        {
            _schemaTable = new DataTable("SchemaTable")
            {
                Locale = CultureInfo.InvariantCulture
            };

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
            AddDataColumn(_schemaTable, SchemaTableColumn.BaseTableName, typeof(string), string.Empty);
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

        private byte[] GetGeometry()
        {
            Geometry ogrGeometry = _currentFeature.GetGeometryRef();
            ogrGeometry.FlattenTo2D();
            byte[] wkbGeometry = new byte[ogrGeometry.WkbSize()];

            ogrGeometry.ExportToWkb(wkbGeometry, wkbByteOrder.wkbXDR);

            return wkbGeometry;

            ////            System.Data.SqlTypes.SqlBytes x = new System.Data.SqlTypes.SqlBytes(wkbGeometry);
            ////            Microsoft.SqlServer.Types.SqlGeometry c = Microsoft.SqlServer.Types.SqlGeometry.STGeomFromWKB(x, 1111);
            ////             Microsoft.SqlServer.Types.SqlGeometry q = c.STExteriorRing();
            ////            MemoryStream ms = new MemoryStream( wkbGeometry );
            ////            BinaryReader rdr = new BinaryReader(ms);
            ////            Microsoft.SqlServer.Types.SqlGeometry g = new Microsoft.SqlServer.Types.SqlGeometry();
            ////            g.Read( rdr );
            // return geometry;
        }

        private FeatureType TranslateOgrGeometryType(wkbGeometryType ogrType)
        {
            switch (ogrType)
            {
                case wkbGeometryType.wkbLineString: return FeatureType.Line;

                case wkbGeometryType.wkbPolygon: return FeatureType.Polygon;

                case wkbGeometryType.wkbPoint: return FeatureType.Point;

                case wkbGeometryType.wkbMultiPoint: return FeatureType.MultiPoint;

                default: return FeatureType.Unspecified;
            }
        }

        private Type TranslateOgrType(FieldType ogrType)
        {
            switch (ogrType)
            {
                case FieldType.OFTInteger: return typeof(int);

                case FieldType.OFTReal: return typeof(double);

                case FieldType.OFTWideString:
                case FieldType.OFTString: return typeof(string);

                case FieldType.OFTBinary: return typeof(byte[]);

                case FieldType.OFTDate:
                case FieldType.OFTTime:
                case FieldType.OFTDateTime: return typeof(DateTime);

                case FieldType.OFTIntegerList:
                case FieldType.OFTRealList:
                case FieldType.OFTStringList:
                case FieldType.OFTWideStringList: throw new NotSupportedException("Type not supported: " + ogrType.ToString());

                default: throw new NotSupportedException("Type not supported: " + ogrType.ToString());
            }
        }

        #endregion
    }
}