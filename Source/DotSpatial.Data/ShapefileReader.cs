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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/6/2010 6:59:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.Data;
using System.Linq;

namespace DotSpatial.Data
{
    /// <summary>
    /// This combines the attribute table with a shape source in order to allow easy creation of
    /// FeatureTable and FeatureRow constructs.
    /// </summary>
    [Obsolete("Do not use it. This class is not supported in DotSpatial anymore.")] // Marked in 1.7
    public class ShapefileReader : IDataReader
    {
        private readonly AttributeTable _attributeTable;
        private int _fieldCount;
        private bool _isOpen;

        /// <summary>
        /// Creates a new ShapefileReader tailored to read a particular file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ShapefileReader(string filename)
        {
            Filename = filename;
            _attributeTable = new AttributeTable(filename);
            ShapefileHeader header = new ShapefileHeader(filename);
            //if (header.ShapeType == ShapeType.Polygon ||
            //    header.ShapeType == ShapeType.PolygonM ||
            //    header.ShapeType == ShapeType.PolygonZ)
            //{
            //    _shapeSource = new PolygonShapefileShapeSource(filename);
            //}
            // To Do: Implement alternate shape sources here.
            _fieldCount = -1;
        }

        /// <summary>
        /// Gets or sets the name of the shapefile.
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// Gets or sets the array of string field names to access.  If this is null or empty, then all the fields are returned.
        /// Fields listed here that do not occur in the shapefile will be ignored.
        /// </summary>
        public string[] Fields { get; set; }

        /// <summary>
        /// Gets or sets the string filter expression.  This is only the "Where" criteria, and doesn't include any other advanced SQL implmenetation.
        /// </summary>
        public string FilterExpression { get; set; }

        /// <summary>
        /// Gets or sets a string fieldname to support sorting.
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets the integer number of the maximum number of results to generate from a shapefile.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the count of members returned so far.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the long row offset where reading should begin.  This will advance
        /// </summary>
        public long Offset { get; set; }

        #region IDataReader Members

        /// <summary>
        ///
        /// </summary>
        public void Close()
        {
            _isOpen = false;
        }

        /// <summary>
        /// Shapefiles don't support nested tables.
        /// </summary>
        public int Depth
        {
            get { return 0; }
        }

        /// <inheritdoc/>
        public DataTable GetSchemaTable()
        {
            // FeatureTable automatically has a long FID and binary Geometry.
            FeatureTable result = new FeatureTable();
            foreach (Field field in _attributeTable.Columns)
            {
                if (Fields != null && Fields.Length > 0)
                {
                    // only include desired fields in our output schema.
                    if (!Fields.Contains(field.ColumnName)) continue;
                }
                result.Columns.Add(field.ColumnName, field.DataType);
            }
            return result;
        }

        /// <inheritdoc/>
        public bool IsClosed
        {
            get { return !_isOpen; }
        }

        /// <inheritdoc/>
        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Read()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Read only reader has no affect on records here.
        /// </summary>
        public int RecordsAffected
        {
            get { return 0; }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Not unmanaged, and files are not left open to require disposal here.
        }

        /// <inheritdoc/>
        public int FieldCount
        {
            get
            {
                if (_fieldCount < 0) GetFieldCount();
                return _fieldCount;
            }
        }

        /// <inheritdoc/>
        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public object GetValue(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public object this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        /// <inheritdoc/>
        public object this[int i]
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        private int GetFieldCount()
        {
            if (_attributeTable == null) return 2;
            int count = 2;
            foreach (Field field in _attributeTable.Columns)
            {
                if (Fields != null && Fields.Length > 0)
                {
                    // only include desired fields in our output schema.
                    if (Fields.Contains(field.ColumnName)) count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Resets the count, so that reading may continue.
        /// </summary>
        public void AdvancePage()
        {
            Count = 0;
        }

        /// <summary>
        /// Ensures that the files can be opened for reading.
        /// </summary>
        public void Open()
        {
            _isOpen = true;
        }
    }
}