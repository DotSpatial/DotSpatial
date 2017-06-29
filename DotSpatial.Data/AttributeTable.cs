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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2009?
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name             |   Date    |   Description
// -------------------------------------------------------------------------------------------------
// Aerosol          | 3/3/2010  |  Addressed some indexing/formatting problems
// Kyle Ellison     |11/03/2010 | Added method to retrieve a page of data for a single column and consolidated parsing code
// Kyle Ellison     |12/08/2010 | Added ability to edit multiple rows in one call for performance
// Kyle Ellison     |12/10/2010 | Added method to retrieve multiple disparate rows in one call for performance
// Arnold Engelmann | 1/18/2013 | Added support for LDID code in DBF file.
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DotSpatial.Data
{
    /// <summary>
    /// A class for controlling the attribute Table related information for a shapefile.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AttributeTable
    {
        /// <summary>
        /// Occurs after content has been loaded into the attribute data.
        /// </summary>
        public event EventHandler AttributesFilled;

        #region Fields

        // Constant for the size of a record
        private const int FILE_DESCRIPTOR_SIZE = 32;
        private bool _attributesPopulated;
        private List<Field> _columns;
        private IDataTable _dataTable; // CGX AERO GLZ
        private List<int> _deletedRows;
        private string _fileName;
        private byte _fileType;
        private bool _hasDeletedRecords;
        private int _headerLength;
        private bool _loaded;
        private int _numFields;
        private int _numRecords;
        private long[] _offsets;
        private IProgressHandler _progressHandler;
        private ProgressMeter _progressMeter;
        private int _recordLength;
        private byte _ldid;
        private Encoding _encoding;
        private DateTime _updateDate;
        private BinaryWriter _writer;
        /// <summary>
        /// Indicates that the Fill methode is called from inside itself.
        /// </summary>
        private bool _isFilling;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of an attribute Table with no file reference
        /// </summary>
        public AttributeTable()
        {
            _deletedRows = new List<int>();
            _fileType = 0x03;
            _encoding = Encoding.Default;
            _ldid = DbaseLocaleRegistry.GetLanguageDriverId(_encoding);
            _progressHandler = DataManager.DefaultDataManager.ProgressHandler;
            _progressMeter = new ProgressMeter(_progressHandler);
            _dataTable = new DS_DataTable(); // CGX AERO GLZ
            _columns = new List<Field>();
        }

        /// <summary>
        /// Creates a new AttributeTable with the specified fileName, or opens
        /// an existing file with that name.
        /// </summary>
        /// <param name="fileName"></param>
        public AttributeTable(string fileName)
            : this()
        {
            Open(fileName);
        }

        #endregion

        /// <summary>
        /// Reads just the content requested in order to satisfy the paging ability of VirtualMode for the DataGridView
        /// </summary>
        /// <param name="lowerPageBoundary"></param>
        /// <param name="rowsPerPage"></param>
        /// <returns></returns>
        public IDataTable SupplyPageOfData(int lowerPageBoundary, int rowsPerPage) // CGX AERO GLZ
        {
            using (var myReader = GetBinaryReader())
            {
                FileInfo fi = new FileInfo(_fileName);

                // Encoding appears to be ASCII, not Unicode
                myReader.BaseStream.Seek(_headerLength + 1, SeekOrigin.Begin);
                if ((int)fi.Length == _headerLength)
                {
                    // The file is empty, so we are done here
                    return null;
                }
                int maxRawRow = (int)((fi.Length - (HeaderLength + 1)) / _recordLength);
                int strt = GetFileIndex(lowerPageBoundary);
                int end = GetFileIndex(lowerPageBoundary + rowsPerPage);
                int rawRows = end - strt;
                int length = rawRows * _recordLength;
                long offset = strt * _recordLength;

                myReader.BaseStream.Seek(offset, SeekOrigin.Current);
                byte[] byteContent = myReader.ReadBytes(length);
                IDataTable result = new DS_DataTable(); // CGX AERO GLZ
                foreach (Field field in _columns)
                {
                    result.Columns.Add(new Field(field.ColumnName, field.TypeCharacter, field.Length, field.DecimalCount));
                }

                int start = 0;
                for (int row = lowerPageBoundary; row < lowerPageBoundary + rowsPerPage; row++)
                {
                    if (row > maxRawRow) break;
                    result.Rows.Add(ReadTableRow(GetFileIndex(row) - strt, start, byteContent, result));
                    start += _recordLength;
                }
                return result;
            }
        }

        /// <summary>
        /// Reads just the content requested.
        /// </summary>
        /// <param name="lowerPageBoundary">starting row</param>
        /// <param name="rowsPerPage">number of rows to return</param>
        /// <param name="fieldName">field for which data is to be returned</param>
        /// <returns></returns>
        public object[] SupplyPageOfData(int lowerPageBoundary, int rowsPerPage, string fieldName)
        {
            using (var myStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 100000))
            {
                FileInfo fi = new FileInfo(_fileName);

                // Encoding appears to be ASCII, not Unicode
                if ((int)fi.Length == _headerLength)
                {
                    // The file is empty, so we are done here
                    return null;
                }
                int maxRawRow = (int)((fi.Length - (HeaderLength + 1)) / _recordLength);

                Field field = _columns[_dataTable.Columns[fieldName].Ordinal];
                int column = _dataTable.Columns[fieldName].Ordinal;
                int fieldLength = _columns[column].Length;
                int columnOffset = GetColumnOffset(column);

                char[] characterContent = new char[fieldLength];

                object[] result = new object[rowsPerPage];
                int outRow = 0;
                byte[] byteContent = new byte[fieldLength];
                for (int row = lowerPageBoundary; row < lowerPageBoundary + rowsPerPage; row++)
                {
                    if (row > maxRawRow) break;
                    long offset = _headerLength + 1 + _recordLength * GetFileIndex(row) + columnOffset;
                    myStream.Seek(offset, SeekOrigin.Begin);
                    myStream.Read(byteContent, 0, fieldLength);
                    _encoding.GetChars(byteContent, 0, fieldLength, characterContent, 0);
                    result[outRow++] = ParseColumn(field, row, characterContent, null);
                }
                return result;
            }
        }

        /// <summary>
        /// Reads just the contents requested.  Faster than returning the entire record if you have lots of attributes but only want a few.
        /// </summary>
        /// <param name="lowerPageBoundary">starting row</param>
        /// <param name="rowsPerPage">number of rows to return</param>
        /// <param name="fieldNames">fields for which data is to be returned</param>
        /// <returns></returns>
        public object[,] SupplyPageOfData(int lowerPageBoundary, int rowsPerPage, IEnumerable<string> fieldNames)
        {
            using (var myStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 100000))
            {
                var fi = new FileInfo(_fileName);

                if ((int)fi.Length == _headerLength)
                {
                    // The file is empty, so we are done here
                    return null;
                }
                var maxRawRow = (int)((fi.Length - (HeaderLength + 1)) / _recordLength);

                // Set up before looping over the rows.
                var fieldNamesArray = fieldNames.ToArray();
                var numFields = fieldNamesArray.Length;
                var fields = new Field[numFields];
                var result = new object[rowsPerPage, numFields];
                var outRow = 0;
                var byteContent = new byte[numFields][];
                var characterContent = new char[numFields][];
                var columnOffsets = new int[numFields];
                var columnList = new List<KeyValuePair<int, int>>();
                for (var fieldNumber = 0; fieldNumber < numFields; fieldNumber++)
                {
                    var field = _columns[_dataTable.Columns[fieldNamesArray[fieldNumber]].Ordinal];
                    fields[fieldNumber] = field;
                    byteContent[fieldNumber] = new byte[field.Length];
                    characterContent[fieldNumber] = new char[field.Length];
                    var column = _dataTable.Columns[fieldNamesArray[fieldNumber]].Ordinal;
                    columnList.Add(new KeyValuePair<int, int>(column, fieldNumber));
                    columnOffsets[fieldNumber] = GetColumnOffset(column);
                }
                columnList.Sort(CompareKvpByKey); // We want to read the attributes in order for each row because it is faster.

                for (var row = lowerPageBoundary; row < lowerPageBoundary + rowsPerPage; row++)
                {
                    if (row > maxRawRow)
                    {
                        if (outRow < rowsPerPage)
                        {
                            var partialResult = new object[outRow, numFields];
                            Array.Copy(result, partialResult, outRow * numFields);
                            return partialResult;
                        }
                    }
                    var fileIndex = GetFileIndex(row);
                    foreach (KeyValuePair<int, int> columnFieldNumberPair in columnList)
                    {
                        int fieldNumber = columnFieldNumberPair.Value;
                        long offset = _headerLength + 1 + _recordLength * fileIndex + columnOffsets[fieldNumber];
                        myStream.Seek(offset, SeekOrigin.Begin);
                        var field = fields[fieldNumber];
                        myStream.Read(byteContent[fieldNumber], 0, field.Length);
                        _encoding.GetChars(byteContent[fieldNumber], 0, field.Length, characterContent[fieldNumber], 0);
                        result[outRow, fieldNumber] = ParseColumn(field, row, characterContent[fieldNumber], null);
                    }
                    outRow++;
                }
                return result;
            }
        }

        /// <summary>
        /// Reads just the content requested in order to satisfy the paging ability of VirtualMode for the DataGridView
        /// </summary>
        /// <param name="startRow">The 0 based integer index of the start row</param>
        /// <param name="dataValues">The DataTable with the 0 row corresponding to the start row.  If this exceeds the size of the data table, it will add rows.</param>
        public void SetAttributes(int startRow, IDataTable dataValues) // CGX AERO GLZ
        {
            for (int row = 0; row < dataValues.Rows.Count; row++)
            {
                int index = row + startRow;
                if (index < NumRecords)
                {
                    Edit(index, dataValues.Rows[row]);
                }
                else
                {
                    AddRow(dataValues.Rows[row]);
                }
            }
            using (var bw = GetBinaryWriter())
            {
                WriteHeader(bw);
            }
        }

        private BinaryWriter GetBinaryWriter()
        {
            return new BinaryWriter(new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 1000000));
        }

        private BinaryReader GetBinaryReader()
        {
            return new BinaryReader(new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 100000));
        }

        /// <summary>
        /// Get a DataTable containing the specified rows
        /// </summary>
        /// <param name="rowNumbers"></param>
        /// <returns></returns>
        public IDataTable GetAttributes(IEnumerable<int> rowNumbers) // CGX AERO GLZ
        {
            using (var myReader = GetBinaryReader())
            {
                FileInfo fi = new FileInfo(_fileName);

                // Encoding appears to be ASCII, not Unicode
                myReader.BaseStream.Seek(_headerLength + 1, SeekOrigin.Begin);
                if ((int)fi.Length == _headerLength)
                {
                    // The file is empty, so we are done here
                    return null;
                }

                IDataTable result = new DS_DataTable(); // CGX AERO GLZ

                foreach (Field field in _columns)
                {
                    result.Columns.Add(new Field(field.ColumnName, field.TypeCharacter, field.Length, field.DecimalCount));
                }

                int maxRawRow = (int)((fi.Length - (HeaderLength + 1)) / _recordLength);

                foreach (int rowNumber in rowNumbers)
                {
                    int rawRow = GetFileIndex(rowNumber);
                    if (rawRow > maxRawRow) break;

                    myReader.BaseStream.Seek(_headerLength + 1 + rawRow * _recordLength, SeekOrigin.Begin);
                    byte[] byteContent = myReader.ReadBytes(_recordLength);
                    result.Rows.Add(ReadTableRow(rawRow, 0, byteContent, result));
                }

                return result;
            }
        }

        /// <summary>
        /// Accounts for deleted rows and returns the index as it appears in the file
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private int GetFileIndex(int rowIndex)
        {
            int count = 0;
            if (_deletedRows == null || _deletedRows.Count == 0) return rowIndex;
            foreach (int row in _deletedRows)
            {
                if (row <= rowIndex + count)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return rowIndex + count;
        }

        /// <summary>
        /// Accounts for deleted rows and adjusts a file index to a row index
        /// </summary>
        /// <param name="fileIndex"></param>
        /// <returns></returns>
        public int GetRowIndexFromFileIndex(int fileIndex)
        {
            if (_deletedRows == null || _deletedRows.Count == 0) return fileIndex;

            int count = _deletedRows.Count(row => row < fileIndex);
            return fileIndex - count;
        }

        /// <summary>
        /// Saves the new row to the data source and updates the file with new content.
        /// </summary>
        /// <param name="values">The values organized against the dictionary of field names.</param>
        public virtual void AddRow(Dictionary<string, object> values)
        {
            // Step 1) Modify the file structure to allow an additional row
            _numRecords += 1;
            using (var bw = GetBinaryWriter())
            {
                WriteHeader(bw);
                int rawRow = GetFileIndex(_numRecords - 1);
                bw.BaseStream.Seek(_headerLength + _recordLength * rawRow, SeekOrigin.Begin);
                byte[] blank = new byte[_recordLength];
                _writer.Write(blank); // the deleted flag
            }
            // Step 2) Re-use the insert code to insert the values for the new row.
            Edit(_numRecords - 1, values);
        }

        /// <summary>
        /// Removes the row at the specified row index from both the table in memory
        /// and by marking it as deleted in the file.  This should work even in cases
        /// where the file is very large and working as an AttributeSource.
        /// </summary>
        /// <param name="index"></param>
        public virtual void RemoveRowAt(int index)
        {
            // If the data table is loaded with values, remove the member from the table
            if (_attributesPopulated)
            {
                if (Table.Rows.Count > index) Table.Rows.RemoveAt(index);
            }

            // Modify the header with the new row count
            _numRecords -= 1;
            using (var bw = GetBinaryWriter())
            {
                WriteHeader(bw);

                // Identify the matching row index in the file by accounting for existing deleted rows.
                int rawRow = GetFileIndex(index);

                // Add the file row to the "deleted" list.  Future row index access will skip the deleted row.
                _deletedRows.Add(rawRow);
                _deletedRows.Sort();

                // Write an * to mark the row as deleted so that it doesn't appear.
                bw.BaseStream.Seek(_headerLength + _recordLength * rawRow, SeekOrigin.Begin);
                bw.Write("*"); // the deleted flag
            }

            // Re-calculate row offsets stored in _offsets
            if (null != _offsets)
                GetRowOffsets();
        }

        /// <summary>
        /// Saves the new row to the data source and updates the file with new content.
        /// </summary>
        /// <param name="values">The values organized against the dictionary of field names.</param>
        public virtual void AddRow(IDataRow values) // CGX AERO GLZ
        {
            // Test to see if the file exists yet.  If not, simply create it with the expectation
            // that there will simply be one row in the data table.
            if (!File.Exists(_fileName))
            {
                _dataTable = new DS_DataTable(); // CGX AERO GLZ
                foreach (DataColumn col in values.Table.Columns)
                {
                    _dataTable.Columns.Add(col.ColumnName, col.DataType, col.Expression);
                }
                IDataRow newRow = _dataTable.NewRow(); // CGX AERO GLZ
                newRow.ItemArray = values.ItemArray;
                _dataTable.Rows.Add(newRow);
                SaveAs(_fileName, true);
                return;
            }

            // Step 1) Modify the file structure to allow an additional row
            _numRecords += 1;
            using (var bw = GetBinaryWriter())
            {
                WriteHeader(bw);
                int rawRow = GetFileIndex(_numRecords - 1);
                bw.BaseStream.Seek(_headerLength + _recordLength * rawRow, SeekOrigin.Begin);
                byte[] blank = new byte[_recordLength];
                bw.Write(blank); // the deleted flag
            }
            // Step 2) Re-use the insert code to insert the values for the new row.
            Edit(_numRecords - 1, values);
        }

        /// <summary>
        /// saves a single row to the data source.
        /// </summary>
        /// <param name="index">the integer row (or FID) index</param>
        /// <param name="values">The object array holding the new values to store.</param>
        public virtual void Edit(int index, Dictionary<string, object> values)
        {
            NumberConverter[] ncs = BeginEdit();
            try
            {
                OverwriteDataRow(index, values, ncs);
            }
            finally
            {
                EndEdit();
            }
        }

        /// <summary>
        /// saves a single row to the data source.
        /// </summary>
        /// <param name="index">the integer row (or FID) index</param>
        /// <param name="values">The object array holding the new values to store.</param>
        public virtual void Edit(int index, IDataRow values) // CGX AERO GLZ
        {
            NumberConverter[] ncs = BeginEdit();
            try
            {
                OverwriteDataRow(index, values, ncs);
            }
            finally
            {
                EndEdit();
            }
        }

        /// <summary>
        /// saves a collection of rows to the data source
        /// </summary>
        /// <param name="indexDataRowPairs"></param>
        public virtual void Edit(IEnumerable<KeyValuePair<int, IDataRow>> indexDataRowPairs) // CGX AERO GLZ
        {
            NumberConverter[] ncs = BeginEdit();
            try
            {
                foreach (KeyValuePair<int, IDataRow> indexedDataRow in indexDataRowPairs) // CGX AERO GLZ
                {
                    OverwriteDataRow(indexedDataRow.Key, indexedDataRow.Value, ncs);
                }
            }
            finally
            {
                EndEdit();
            }
        }

        ///<summary>
        /// Edit the specified rows via a client supplied callback
        ///</summary>
        ///<param name="indices">rows to edit</param>
        ///<param name="rowCallback">client supplied callback</param>
        public virtual void Edit(IEnumerable<int> indices, RowEditEvent rowCallback)
        {
            var fi = new FileInfo(_fileName);
            if ((int)fi.Length == _headerLength)
            {
                // The file is empty, so just return
                return;
            }

            foreach (Field field in _columns)
            {
                field.NumberConverter = new NumberConverter(field.Length, field.DecimalCount);
            }

            var fields = new Fields(_columns);
            using (var myStream = new FileStream(_fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, _recordLength))
            {
                var e = new RowEditEventArgs(_recordLength - 1, fields);
                foreach (int rowIndex in indices)
                {
                    int strt = GetFileIndex(rowIndex);
                    long offset = _headerLength + 1 + strt * _recordLength;
                    myStream.Seek(offset, SeekOrigin.Begin);
                    myStream.Read(e.ByteContent, 0, _recordLength - 1);
                    e.RowNumber = rowIndex;
                    e.Modified = false;
                    if (rowCallback(e))
                        return;
                    if (e.Modified)
                    {
                        myStream.Seek(offset, SeekOrigin.Begin);
                        myStream.Write(e.ByteContent, 0, _recordLength - 1);
                    }
                }
            }
        }

        /// <summary>
        /// Setup before calls to OverwriteDataRow
        /// </summary>
        /// <returns></returns>
        private NumberConverter[] BeginEdit()
        {
            NumberConverter[] ncs = new NumberConverter[_columns.Count];
            for (int i = 0; i < _columns.Count; i++)
            {
                Field fld = _columns[i];
                ncs[i] = new NumberConverter(fld.Length, fld.DecimalCount);
            }
            //overridden in sub-classes
            _writer = GetBinaryWriter();
            return ncs;
        }

        /// <summary>
        /// Cleanup after finished calling OverwriteDataRow
        /// </summary>
        private void EndEdit()
        {
            _writer.Flush();
            _writer.Close();
        }

        /// <summary>
        /// Common code for editing an existing row in the data source
        /// </summary>
        /// <param name="index"></param>
        /// <param name="values"></param>
        /// <param name="ncs"></param>
        private void OverwriteDataRow(int index, object values, NumberConverter[] ncs)
        {
            // We allow passing in either DataRow values or Dictionary, so figure out which
            Dictionary<string, object> dataDictionary = null;
            var dataRow = values as DS_DataRow; // CGX AERO GLZ
            if (null == dataRow)
                dataDictionary = (Dictionary<string, object>)values;

            int rawRow = GetFileIndex(index);
            _writer.Seek(_headerLength + _recordLength * rawRow, SeekOrigin.Begin);
            _writer.Write((byte)0x20); // the deleted flag
            int len = _recordLength - 1;
            for (int fld = 0; fld < _columns.Count; fld++)
            {
                string name = _columns[fld].ColumnName;
                var columnValue = dataRow != null ? dataRow[name] : dataDictionary[name];
                WriteColumnValue(columnValue, fld, ncs);
                len -= _columns[fld].Length;
            }
            // If, for some reason the column lengths don't add up to the total record length, fill with spaces.
            if (len > 0) WriteSpaces(len);
        }

        private void WriteColumnValue(object columnValue, int fld, NumberConverter[] ncs)
        {
            if (columnValue == null || columnValue is DBNull)
                WriteSpaces(_columns[fld].Length);
            else if (columnValue is decimal)
                _writer.Write(ncs[fld].ToChar((decimal)columnValue));
            else if (columnValue is double)
            {
                //Write((double)columnValue, _columns[fld].Length, _columns[fld].DecimalCount);
                char[] test = ncs[fld].ToChar((double)columnValue);
                _writer.Write(test);
            }
            else if (columnValue is float)
            {
                //Write((float)columnValue, _columns[fld].Length, _columns[fld].DecimalCount);
                Field currentField = _columns[fld];
                if (currentField.TypeCharacter == 'F')
                {
                    string val = ((float)columnValue).ToString();
                    Write(val, currentField.Length);
                }
                else
                {
                    char[] test = ncs[fld].ToChar((float)columnValue);
                    _writer.Write(test);
                }
            }
            else if (columnValue is int || columnValue is short || columnValue is long || columnValue is byte)
                Write(Convert.ToInt64(columnValue), _columns[fld].Length, _columns[fld].DecimalCount);
            else if (columnValue is bool)
                Write((bool)columnValue);
            else if (columnValue is string)
            {
                int length = _columns[fld].Length;
                Write((string)columnValue, length);
            }
            else if (columnValue is DateTime)
                WriteDate((DateTime)columnValue);
            else
                Write(columnValue.ToString(), _columns[fld].Length);
        }

        /// <summary>
        /// Read a single dbase record
        /// </summary>
        /// <returns>Returns an IFeature with information appropriate for the current row in the Table</returns>
        private IDataRow ReadTableRow(int currentRow, int start, byte[] byteContent, IDataTable table) // CGX AERO GLZ
        {
            IDataRow result = table.NewRow(); // CGX AERO GLZ
            for (int col = 0; col < table.Columns.Count; col++)
            {
                // find the length of the field.
                Field currentField = table.Columns[col] as Field;
                if (currentField == null)
                {
                    // somehow the field is not a valid Field
                    return result;
                }

                // read the data.
                int len = currentField.Length;
                if (start + currentField.Length > byteContent.Length)
                {
                    len = byteContent.Length - start;
                }
                if (len < 0) return result;
                char[] cBuffer = _encoding.GetChars(byteContent, start, len);

                start += currentField.Length;

                if (IsNull(cBuffer)) continue;

                result[currentField.ColumnName] = ParseColumn(currentField, currentRow, cBuffer, table);
            }

            return result;
        }

        private static bool HasEof(Stream fs, long length)
        {
            fs.Seek(length, SeekOrigin.Begin);
            return 0x1a == fs.ReadByte();
        }

        #region Methods

        /// <summary>
        /// Reads all the information from the file, including the vector shapes and the database component.
        /// </summary>
        public void Open(string fileName)
        {
            // Open and search for deleted records
            Open(fileName, null);
        }

        /// <summary>
        /// Reads the header and if deletedRows is null, searches file for deletedRows if file size indicates possibility of deleted rows.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="deletedRows"></param>
        public void Open(string fileName, List<int> deletedRows)
        {
            Contract.Requires(!String.IsNullOrEmpty(fileName), "fileName is null or empty.");

            _fileName = Path.ChangeExtension(fileName, ".dbf");
            Contract.Assert(File.Exists(_fileName), "The dbf file for this shapefile was not found.");

            _attributesPopulated = false; // we had a file, but have not read the dbf content into memory yet.
            _dataTable = new DS_DataTable(); // CGX AERO GLZ

            using (var myReader = GetBinaryReader())
            {
                ReadTableHeader(myReader); // based on the header, set up the fields information etc.

                // If the deleted rows were passed in, we don't need to look for them
                if (null != deletedRows)
                {
                    _deletedRows = deletedRows;
                    _hasDeletedRecords = _deletedRows.Count > 0;
                    return;
                }
                FileInfo fi = new FileInfo(_fileName);
                long length = _headerLength + _numRecords * _recordLength;
                long pos = myReader.BaseStream.Position;
                if (HasEof(myReader.BaseStream, length))
                    length++;

                if (fi.Length == length)
                {
                    _hasDeletedRecords = false;
                    // No deleted rows detected
                    return;
                }

                myReader.BaseStream.Seek(pos, SeekOrigin.Begin);

                _hasDeletedRecords = true;
                int count = 0;
                int row = 0;
                while (count < _numRecords)
                {
                    if (myReader.BaseStream.ReadByte() == (byte)' ')
                    {
                        count++;
                    }
                    else
                    {
                        _deletedRows.Add(row);
                    }
                    row++;
                    myReader.BaseStream.Seek(_recordLength - 1, SeekOrigin.Current);
                }
            }
        }

        private void GetRowOffsets()
        {
            var fi = new FileInfo(_fileName);

            // Encoding appears to be ASCII, not Unicode
            if ((int)fi.Length == _headerLength)
            {
                // The file is empty, so we are done here
                return;
            }

            if (_hasDeletedRecords)
            {
                int length = (int)(fi.Length - _headerLength - 1);
                int recordCount = length / _recordLength;
                _offsets = new long[_numRecords];
                int j = 0; // undeleted index
                using (var myReader = GetBinaryReader())
                {
                    for (int i = 0; i <= recordCount; i++)
                    {
                        // seek to byte
                        myReader.BaseStream.Seek(_headerLength + 1 + i * _recordLength, SeekOrigin.Begin);
                        var cb = myReader.ReadByte();
                        if (cb != '*')
                            _offsets[j] = i * _recordLength;
                        j++;
                        if (j == _numRecords) break;
                    }
                }
            }
            _loaded = true;
        }

        private int GetColumnOffset(int column)
        {
            int offset = 0;
            for (int col = 0; col < column; col++)
            {
                offset += _columns[col].Length;
            }
            return offset;
        }

        /// <summary>
        /// This populates the Table with data from the file.
        /// </summary>
        /// <param name="numRows">In the event that the dbf file is not found, this indicates how many blank rows should exist in the attribute Table.</param>
        public void Fill(int numRows)
        {
            if (_isFilling) return; // Changed by jany_ (2015-07-30) don't load again because the fill methode is called from inside the fill methode and we'd get a datatable that is filled with twice the existing records
            _attributesPopulated = false;
            _dataTable.Rows.Clear(); // if we have already loaded data, clear the data.
            _isFilling = true;
            if (File.Exists(_fileName) == false)
            {
                _numRecords = numRows;
                _dataTable.BeginLoadData();
                if (!_dataTable.Columns.Contains("FID"))
                {
                    _dataTable.Columns.Add("FID", typeof(int));
                }
                for (int row = 0; row < numRows; row++)
                {
                    IDataRow dr = _dataTable.NewRow(); // CGX AERO GLZ
                    dr["FID"] = row;
                    _dataTable.Rows.Add(dr);
                }
                _dataTable.EndLoadData();
                return;
            }

            if (!_loaded) GetRowOffsets();
            ProgressMeter = new ProgressMeter(ProgressHandler, "Reading from DBF Table...", _numRecords);
            if (_numRecords < 10000) ProgressMeter.StepPercent = 100;
            else if (_numRecords < 100000) ProgressMeter.StepPercent = 50;
            else if (_numRecords < 5000000) ProgressMeter.StepPercent = 10;
            else if (_numRecords < 10000000) ProgressMeter.StepPercent = 5;

            _dataTable.BeginLoadData();
            // Reading the Table elements as well as the shapes in a single progress loop.
            using (var myReader = GetBinaryReader())
            {
                for (int row = 0; row < _numRecords; row++)
                {
                    // --------- DATABASE --------- CurrentFeature = ReadTableRow(myReader);
                    IDataRow nextRow = null; // CGX AERO GLZ
                    try
                    {
                        nextRow = ReadTableRowFromChars(row, myReader);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        nextRow = _dataTable.NewRow();
                    }
                    finally
                    {
                        _dataTable.Rows.Add(nextRow);
                    }

                    // If a progress message needs to be updated, this will handle that.
                    ProgressMeter.CurrentValue = row;
                }
            }
            ProgressMeter.Reset();
            _dataTable.EndLoadData();

            _attributesPopulated = true;
            OnAttributesFilled();
            _isFilling = false;
        }

        /// <summary>
        /// Attempts to save the file to the path specified by the Filename property.
        /// This should be the .shp extension.
        /// </summary>
        public void Save()
        {
            if (File.Exists(Filename)) File.Delete(Filename);
            UpdateSchema();
            _writer = GetBinaryWriter();
            WriteHeader(_writer);
            WriteTable();
            _writer.Close();
        }

        /// <summary>
        /// Saves this Table to the specified fileName
        /// </summary>
        /// <param name="fileName">The string fileName to save to</param>
        /// <param name="overwrite">A boolean indicating whether or not to write over the file if it exists.</param>
        public void SaveAs(string fileName, bool overwrite)
        {
            if (Filename == fileName)
            {
                Save();
                return;
            }
            if (File.Exists(fileName))
            {
                if (overwrite == false)
                {
                    throw new IOException("File Exists and overwrite was set to false.");
                }
            }
            Filename = fileName;
            Save();
        }

        private void UpdateSchema()
        {
            List<Field> tempColumns = new List<Field>();
            _recordLength = 1; // delete character
            _numRecords = _dataTable.Rows.Count;
            _updateDate = DateTime.Now;
            _headerLength = FILE_DESCRIPTOR_SIZE + FILE_DESCRIPTOR_SIZE * _dataTable.Columns.Count + 1;
            if (_columns == null) _columns = new List<Field>();
            // Delete any fields from the columns list that are no
            // longer in the data Table.
            List<Field> removeFields = new List<Field>();
            foreach (Field fld in _columns)
            {
                if (_dataTable.Columns.Contains(fld.ColumnName) == false)
                    removeFields.Add(fld);
                else
                    tempColumns.Add(fld);
            }
            foreach (Field field in removeFields)
            {
                _columns.Remove(field);
            }

            // Add new columns that exist in the data Table, but don't have a matching field yet.

            tempColumns.AddRange(from DataColumn dc in _dataTable.Columns
                                 where !ColumnNameExists(dc.ColumnName)
                                 select dc as Field ?? new Field(dc));

            _columns = tempColumns;

            _recordLength = 1;

            // Recalculate the recordlength
            // current calculation fix proposed by Aerosol
            foreach (Field fld in Columns)
            {
                _recordLength = _recordLength + fld.Length;
            }
        }

        /// <summary>
        ///  Tests to see if the list of columns contains the specified name or not.
        /// </summary>
        /// <param name="name">Name of the column we're looking for.</param>
        /// <returns>True, if the column exists.</returns>
        private bool ColumnNameExists(string name)
        {
            return _columns.Any(fld => fld.ColumnName == name);
        }

        /// <summary>
        /// This appends the content of one datarow to a dBase file.
        /// </summary>
        /// <exception cref="ArgumentNullException">The columnValues parameter was null</exception>
        /// <exception cref="InvalidOperationException">Header records need to be written first.</exception>
        /// <exception cref="InvalidDataException">Table property of columnValues parameter cannot be null.</exception>
        public void WriteTable()
        {
            if (_dataTable == null) return;

            // _writer.Write((byte)0x20); // the deleted flag
            var ncs = new NumberConverter[_columns.Count];
            for (int i = 0; i < _columns.Count; i++)
            {
                Field fld = _columns[i];
                ncs[i] = new NumberConverter(fld.Length, fld.DecimalCount);
            }
            for (int row = 0; row < _dataTable.Rows.Count; row++)
            {
                _writer.Write((byte)0x20); // the deleted flag
                int len = _recordLength - 1;
                for (int fld = 0; fld < _columns.Count; fld++)
                {
                    string name = _columns[fld].ColumnName;
                    object columnValue = _dataTable.Rows[row][name];
                    WriteColumnValue(columnValue, fld, ncs);
                    len -= _columns[fld].Length;
                }
                // If, for some reason the column lengths don't add up to the total record length, fill with spaces.
                if (len > 0) WriteSpaces(len);
            }
        }

        /// <summary>
        /// Writes a number of spaces equal to numspaces
        /// </summary>
        /// <param name="numspaces">The integer number of spaces to write</param>
        public void WriteSpaces(int numspaces)
        {
            for (int i = 0; i < numspaces; i++)
            {
                _writer.Write(' ');
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="number"></param>
        /// <param name="length"></param>
        /// <param name="decimalCount"></param>
        public void Write(double number, int length, int decimalCount)
        {
            // write with 19 chars.
            string format = "{0:";
            for (int i = 0; i < decimalCount; i++)
            {
                if (i == 0)
                    format = format + "0.";
                format = format + "0";
            }
            format = format + "}";
            string str = String.Format(format, number);
            for (int i = 0; i < length - str.Length; i++)
                _writer.Write((byte)0x20);
            foreach (char c in str)
                _writer.Write(c);
        }

        /// <summary>
        /// Writes an integer so that it is formatted for dbf.  This is still buggy since it is possible to lose info here.
        /// </summary>
        /// <param name="number">The long value</param>
        /// <param name="length">The length of the field.</param>
        /// <param name="decimalCount">The number of digits after the decimal</param>
        public void Write(long number, int length, int decimalCount)
        {
            string str = number.ToString();
            if (str.Length > length)
                str = str.Substring(str.Length - length, length);
            for (int i = 0; i < length - str.Length; i++)
                _writer.Write((byte)0x20);
            foreach (char c in str)
                _writer.Write(c);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        public void Write(string text, int length)
        {
            // Some encodings (multi-byte encodings) for languages such as Chinese, can result in variable number of bytes per character
            // so we convert character by character until we reach the length, and pad remaining bytes with spaces.
            // Note this replaces padding suggested at http://www.mapwindow.org/phorum/read.php?13,16820

            char[] characters = text.ToCharArray();

            int totalBytes = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                byte[] charBytes = _encoding.GetBytes(characters, i, 1);
                if (totalBytes + charBytes.Length <= length)
                {
                    _writer.Write(charBytes);
                    totalBytes += charBytes.Length;
                }
                else
                {
                    break;
                }
            }

            if (totalBytes < length)
            {
                WriteSpaces(length - totalBytes);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="date"></param>
        public void WriteDate(DateTime date)
        {
            // YYYYMMDD format
            string test = date.ToString("yyyyMMdd");
            Write(test, 8);
            //_writer.Write(date.Year - 1900);
            //_writer.Write(date.Month);
            //_writer.Write(date.Day);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="flag"></param>
        public void Write(bool flag)
        {
            _writer.Write(flag ? 'T' : 'F');
        }

        /// <summary>
        /// Write the header data to the DBF file.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteHeader(BinaryWriter writer)
        {
            // write the output file type.
            writer.Write(_fileType);

            writer.Write((byte)(_updateDate.Year - 1900));
            writer.Write((byte)_updateDate.Month);
            writer.Write((byte)_updateDate.Day);

            // write the number of records in the datafile.
            writer.Write(_numRecords);

            // write the length of the header structure.
            writer.Write((short)_headerLength); // 32 + 30 * numColumns

            // write the length of a record
            writer.Write((short)_recordLength);

            // write reserved/unused bytes in the header
            for (int i = 0; i < 17; i++)
                writer.Write((byte)0);

            // write Language Driver ID (LDID)
            writer.Write(_ldid);

            // write remaining reserved/unused bytes in the header
            for (int i = 0; i < 2; i++)
                writer.Write((byte)0);

            // write all of the header records
            foreach (Field currentField in _columns)
            {
                // write the field name (can't be more than 11 bytes)
                char[] characters = currentField.ColumnName.ToCharArray();
                int totalBytes = 0;
                for (int j = 0; j < characters.Length; j++)
                {
                    byte[] charBytes = _encoding.GetBytes(characters, j, 1);
                    if (totalBytes + charBytes.Length <= 11)
                    {
                        writer.Write(charBytes);
                        totalBytes += charBytes.Length;
                    }
                    else
                    {
                        break;
                    }
                }
                while (totalBytes < 11)
                {
                    writer.Write((byte)0);
                    totalBytes++;
                }

                // write the field type
                writer.Write(currentField.TypeCharacter);

                // write the field data address, offset from the start of the record.
                writer.Write(0);

                // write the length of the field.
                writer.Write(currentField.Length);

                // write the decimal count.
                writer.Write(currentField.DecimalCount);

                // write the reserved bytes.
                for (int j = 0; j < 14; j++) writer.Write((byte)0);
            }
            // write the end of the field definitions marker
            writer.Write((byte)0x0D);
        }

        private static bool IsNull(IEnumerable<char> charArray)
        {
            return charArray.All(t => t == ' ' || t == '\0');
        }

        private static int CompareKvpByKey(KeyValuePair<int, int> x, KeyValuePair<int, int> y)
        {
            return x.Key - y.Key;
        }

        /// <summary>
        /// Read a single dbase record
        /// </summary>
        /// <returns>Returns an IFeature with information appropriate for the current row in the Table</returns>
        private IDataRow ReadTableRowFromChars(int currentRow, BinaryReader myReader) // CGX AERO GLZ
        {
            var result = _dataTable.NewRow();

            long start;
            if (_hasDeletedRecords == false)
                start = currentRow * _recordLength;
            else
                start = _offsets[currentRow];

            myReader.BaseStream.Seek(_headerLength + 1 + start, SeekOrigin.Begin);
            for (int col = 0; col < _dataTable.Columns.Count; col++)
            {
                // find the length of the field.
                var currentField = _columns[col];

                // read the data.
                var byteBuffer = myReader.ReadBytes(currentField.Length);
                char[] cBuffer = _encoding.GetChars(byteBuffer);// Modified on 20 Aug. by Andy
                if (IsNull(cBuffer)) continue;

                result[currentField.ColumnName] = ParseColumn(currentField, currentRow, cBuffer, _dataTable);
            }

            return result;
        }

        /// <summary>
        /// Parse the character data for one column into an object ready for insertion into a data row
        /// </summary>
        /// <param name="field"></param>
        /// <param name="currentRow"></param>
        /// <param name="cBuffer"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private object ParseColumn(Field field, int currentRow, char[] cBuffer, IDataTable table) // CGX AERO GLZ
        {
            // If table is null, an exception will be thrown rather than attempting to upgrade the column when a parse error occurs.
            const string parseErrString =
                "Cannot parse {0} at row {1:D}, column {2:D} ({3}) in file {4} using field type {5}, and no DataTable to upgrade column";

            // find the field type
            object tempObject = DBNull.Value;
            switch (field.TypeCharacter)
            {
                case 'L': // logical data type, one character (T, t, F, f, Y, y, N, n)

                    char tempChar = cBuffer[0];
                    if ((tempChar == 'T') || (tempChar == 't') || (tempChar == 'Y') || (tempChar == 'y'))
                        tempObject = true;
                    else tempObject = false;
                    break;

                case 'C': // character record.

                    tempObject = new string(cBuffer).Replace("\0", string.Empty).Trim();
                    break;

                case 'T':
                    throw new NotSupportedException();

                case 'D': // date data type.

                    string tempString = new string(cBuffer, 0, 4);
                    int year;
                    if (int.TryParse(tempString, out year) == false) break;
                    int month;
                    tempString = new string(cBuffer, 4, 2);
                    if (int.TryParse(tempString, out month) == false) break;
                    int day;
                    tempString = new string(cBuffer, 6, 2);
                    if (int.TryParse(tempString, out day) == false) break;

                    try
                    {
                        tempObject = new DateTime(year, month, day);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // Ignore invalid or out of range dates
                        tempObject = DBNull.Value;
                    }

                    break;

                case 'F':
                case 'B':
                case 'N': // number - ESRI uses N for doubles and floats

                    tempObject = ParseNumericColumn(field, currentRow, cBuffer, table, parseErrString);
                    break;

                default:
                    throw new NotSupportedException("Do not know how to parse Field type " + field.TypeCharacter);
            }
            return tempObject;
        }

        private object ParseNumericColumn(Field field, int currentRow, char[] cBuffer, IDataTable table, string parseErrString) // CGX AERO GLZ
        {
            string tempStr = new string(cBuffer);
            object tempObject = DBNull.Value;
            Type t = field.DataType;
            var errorMessage = new Lazy<string>(() => String.Format(parseErrString, tempStr, currentRow, field.Ordinal, field.ColumnName, _fileName, t));

            if (t == typeof(byte))
            {
                byte temp;
                if (byte.TryParse(tempStr.Trim(), out temp))
                    tempObject = temp;
                else
                {
                    // It is possible to store values larger than 255 with
                    // three characters.  Therefore, we may have to upgrade the
                    // numeric type for the entire field to short.
                    if (null == table)
                        throw new InvalidDataException(errorMessage.Value);
                    short upTest;
                    if (short.TryParse(tempStr.Trim(), out upTest))
                    {
                        // Since we were successful, we should upgrade the field to storing short values instead of byte values.
                        UpgradeColumn(field, typeof(short), currentRow, field.Ordinal, table);
                        tempObject = upTest;
                    }
                    else
                    {
                        UpgradeColumn(field, typeof(string), currentRow, field.Ordinal, table);
                        tempObject = tempStr;
                    }
                }
            }
            else if (t == typeof(short))
            {
                short temp;
                if (short.TryParse(tempStr.Trim(), out temp))
                    tempObject = temp;
                else
                {
                    if (null == table)
                        throw new InvalidDataException(errorMessage.Value);
                    int upTest;
                    if (int.TryParse(tempStr.Trim(), out upTest))
                    {
                        UpgradeColumn(field, typeof(int), currentRow, field.Ordinal, table);
                        tempObject = upTest;
                    }
                    else
                    {
                        UpgradeColumn(field, typeof(string), currentRow, field.Ordinal, table);
                        tempObject = tempStr;
                    }
                }
            }
            else if (t == typeof(int))
            {
                int temp;
                if (int.TryParse(tempStr.Trim(), out temp))
                    tempObject = temp;
                else
                {
                    if (null == table)
                        throw new InvalidDataException(errorMessage.Value);
                    long upTest;
                    if (long.TryParse(tempStr.Trim(), out upTest))
                    {
                        UpgradeColumn(field, typeof(long), currentRow, field.Ordinal, table);
                        tempObject = upTest;
                    }
                    else
                    {
                        UpgradeColumn(field, typeof(string), currentRow, field.Ordinal, table);
                        tempObject = tempStr;
                    }
                }
            }
            else if (t == typeof(long))
            {
                long temp;
                if (long.TryParse(tempStr.Trim(), out temp))
                    tempObject = temp;
                else
                {
                    if (null == table)
                        throw new InvalidDataException(errorMessage.Value);
                    UpgradeColumn(field, typeof(string), currentRow, field.Ordinal, table);
                    tempObject = tempStr;
                }
            }
            else if (t == typeof(float))
            {
                float temp;
                if (float.TryParse(tempStr, NumberStyles.Number | NumberStyles.AllowExponent, NumberConverter.NumberConversionFormatProvider, out temp))
                    tempObject = temp;
                else
                {
                    if (null == table)
                        throw new InvalidDataException(errorMessage.Value);
                    double upTest;
                    if (double.TryParse(tempStr, NumberStyles.Number | NumberStyles.AllowExponent, NumberConverter.NumberConversionFormatProvider, out upTest))
                    {
                        UpgradeColumn(field, typeof(double), currentRow, field.Ordinal, table);
                        tempObject = upTest;
                    }
                    else
                    {
                        UpgradeColumn(field, typeof(string), currentRow, field.Ordinal, table);
                        tempObject = tempStr;
                    }
                }
            }
            else if (t == typeof(double))
            {
                double temp;
                if (double.TryParse(tempStr, NumberStyles.Number | NumberStyles.AllowExponent, NumberConverter.NumberConversionFormatProvider, out temp))
                    tempObject = temp;
                else if (String.IsNullOrWhiteSpace(tempStr)) //handle case when value is NULL
                    tempObject = DBNull.Value;
                else
                {
                    if (null == table)
                        throw new InvalidDataException(errorMessage.Value);
                    decimal upTest;
                    if (decimal.TryParse(tempStr, NumberStyles.Number | NumberStyles.AllowExponent, NumberConverter.NumberConversionFormatProvider, out upTest))
                    {
                        UpgradeColumn(field, typeof(decimal), currentRow, field.Ordinal, table);
                        tempObject = upTest;
                    }
                    else
                    {
                        UpgradeColumn(field, typeof(string), currentRow, field.Ordinal, table);
                        tempObject = tempStr;
                    }
                }
            }
            else if (t == typeof(decimal))
            {
                decimal temp;
                if (decimal.TryParse(tempStr, NumberStyles.Number | NumberStyles.AllowExponent, NumberConverter.NumberConversionFormatProvider, out temp))
                    tempObject = temp;
                else
                {
                    if (null == table)
                        throw new InvalidDataException(errorMessage.Value);
                    UpgradeColumn(field, typeof(string), currentRow, field.Ordinal, table);
                    tempObject = tempStr;
                }
            }
            return tempObject;
        }
        /// <summary>
        /// Read the header data from the DBF file.
        /// </summary>
        /// <param name="reader">BinaryReader containing the header.</param>
        private void ReadTableHeader(BinaryReader reader)
        {
            // type of reader.
            _fileType = reader.ReadByte();
            if (_fileType != 0x03)
                throw new NotSupportedException("Unsupported DBF reader Type " + _fileType);

            // parse the update date information.
            int year = reader.ReadByte();
            int month = reader.ReadByte();
            int day = reader.ReadByte();

            try
            {
                _updateDate = new DateTime(year + 1900, month, day);
            }
            catch
            {
                // If the Update Date in the header is not in correct format, just use the modify time of the file
                _updateDate = new FileInfo(_fileName).LastWriteTime;
            }

            // read the number of records.
            _numRecords = reader.ReadInt32();

            // read the length of the header structure.
            _headerLength = reader.ReadInt16();

            // read the length of a record
            _recordLength = reader.ReadInt16();

            // skip reserved/unused bytes in the header.
            reader.ReadBytes(17);

            // read Language Driver ID (LDID)
            LanguageDriverId = reader.ReadByte();

            // skip remaining reserved/unused bytes in header
            reader.ReadBytes(2);

            // calculate the number of Fields in the header
            _numFields = (_headerLength - FILE_DESCRIPTOR_SIZE - 1) / FILE_DESCRIPTOR_SIZE;

            _columns = new List<Field>();

            for (int i = 0; i < _numFields; i++)
            {
                // read the field name
                string name = _encoding.GetString(reader.ReadBytes(11));
                int nullPoint = name.IndexOf((char)0);
                if (nullPoint != -1)
                    name = name.Substring(0, nullPoint);

                // read the field type
                char code = (char)reader.ReadByte();

                // read the field data address, offset from the start of the record.
                int dataAddress = reader.ReadInt32();

                // read the field length in bytes
                byte tempLength = reader.ReadByte();

                // read the field decimal count in bytes
                byte decimalcount = reader.ReadByte();

                // read the reserved bytes.
                //reader.skipBytes(14);
                reader.ReadBytes(14);
                int j = 1;
                string tempName = name;
                while (_dataTable.Columns.Contains(tempName))
                {
                    tempName = name + j;
                    j++;
                }
                name = tempName;
                Field myField = new Field(name, code, tempLength, decimalcount) { DataAddress = dataAddress };

                _columns.Add(myField); // Store fields accessible by an index
                _dataTable.Columns.Add(myField);
            }

            // Last byte is a marker for the end of the field definitions.
            reader.ReadBytes(1);
        }

        private void SetTextEncoding()
        {
            // A .cpg file will override any language specification in the DBF header.
            bool cpgSet = false;
            if (!string.IsNullOrEmpty(_fileName))
            {
                try
                {
                    string cpgFileName = Path.ChangeExtension(_fileName, ".cpg");
                    if (File.Exists(cpgFileName))
                    {
                        using (StreamReader reader = new StreamReader(cpgFileName))
                        {
                            string codePageText = reader.ReadLine();
                            int codePage;
                            if (int.TryParse(codePageText, NumberStyles.Integer, CultureInfo.InvariantCulture, out codePage))
                            {
                                _encoding = Encoding.GetEncoding(codePage);
                                cpgSet = true;
                            }
                            else if (string.Compare(codePageText, "UTF-8", true, CultureInfo.InvariantCulture) == 0)
                            {
                                _encoding = Encoding.UTF8;
                                cpgSet = true;
                            }
                        }
                    }
                }
                catch
                {
                    cpgSet = false;
                }
            }

            // If .cpg file does not exist or cannot be read, then we revert to LDID in DBF header.
            // (This is the normal behavior, .cpg files are rare.)
            if (!cpgSet)
            {
                // Unless specifically configured otherwise, ArcGIS will read shapefiles with 0x57 using the encoding returned by
                // Encoding.Default. Since this is the behavior most people will expect, we match it here.
                if (_ldid == 0x57)
                {
                    _encoding = Encoding.Default;
                }
                else
                {
                    _encoding = DbaseLocaleRegistry.GetEncoding(_ldid);
                }
            }
        }

        /// <summary>
        /// This systematically copies all the existing values to a new data column with the same properties,
        /// but with a new data type.  Values that cannot convert will be set to null.
        /// </summary>
        /// <param name="oldDataColumn">The old data column to update</param>
        /// <param name="newDataType">The new data type that the column should become</param>
        /// <param name="currentRow">The row up to which values should be changed for</param>
        /// <param name="columnIndex">The column index of the field being changed</param>
        /// <param name="table"> The Table to apply this strategy to.</param>
        /// <returns>An integer list showing the index values of the rows where the conversion failed.</returns>
        public List<int> UpgradeColumn(Field oldDataColumn, Type newDataType, int currentRow, int columnIndex, IDataTable table) // CGX AERO GLZ
        {
            List<int> failureList = new List<int>();
            object[] newValues = new object[table.Rows.Count];
            string name = oldDataColumn.ColumnName;
            Field dc = new Field(oldDataColumn.ColumnName, newDataType)
                           {
                               Length = oldDataColumn.Length,
                               DecimalCount = oldDataColumn.DecimalCount
                           };
            for (int row = 0; row < currentRow; row++)
            {
                try
                {
                    if (table.Rows[row][name] is DBNull)
                        newValues[row] = null;
                    else
                    {
                        object obj = _dataTable.Rows[row][name];

                        object newObj = Convert.ChangeType(obj, newDataType);
                        newValues[row] = newObj;
                    }
                }
                catch
                {
                    failureList.Add(row);
                }
            }

            int ord = oldDataColumn.Ordinal;
            table.Columns.Remove(oldDataColumn);
            table.Columns.Add(dc);
            dc.SetOrdinal(ord);
            _columns[columnIndex] = dc;
            for (int row = 0; row < currentRow; row++)
            {
                if (newValues[row] == null)
                    table.Rows[row][name] = DBNull.Value;
                else
                    table.Rows[row][name] = newValues[row];
            }
            return failureList;
        }

        #endregion

        #region Properties

        /// <summary>
        /// gets or sets whether the Attributes have been populated.  If data was "opened" from a file,
        /// and a query is made to the DataTable while _attributesPopulated is false, then
        /// a Fill method will be called automatically
        /// </summary>
        public bool AttributesPopulated
        {
            get { return _attributesPopulated; }
            set { _attributesPopulated = value; }
        }

        /// <summary>
        /// The byte length of the header
        /// </summary>
        public int HeaderLength
        {
            get { return _headerLength; }
        }

        /// <summary>
        /// The columns
        /// </summary>
        public List<Field> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        /// <summary>
        /// The file type
        /// </summary>
        public byte FileType
        {
            get { return _fileType; }
        }

        /// <summary>
        /// The fileName of the dbf file
        /// </summary>
        public string Filename
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// Number of records
        /// </summary>
        public int NumRecords
        {
            get { return _numRecords; }
        }

        /// <summary>
        /// Gets or sets the progress handler for this Attribute Table.
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get { return _progressHandler; }
            set
            {
                _progressHandler = value;
                _progressMeter = new ProgressMeter(_progressHandler);
            }
        }

        /// <summary>
        /// Gets or sets the progress meter that is directly tied to the progress handler
        /// </summary>
        protected ProgressMeter ProgressMeter
        {
            get { return _progressMeter; }
            set { _progressMeter = value; }
        }

        /// <summary>
        /// The byte length of each record
        /// </summary>
        public int RecordLength
        {
            get { return _recordLength; }
        }

        /// <summary>
        /// Gets the language driver ID (LDID) for this file
        /// </summary>
        public byte LanguageDriverId
        {
            get
            {
                return _ldid;
            }
            private set
            {
                _ldid = value;
                SetTextEncoding();
            }
        }

        /// <summary>
        /// Gets the encoding used for text-based data and column names (based on LDID)
        /// </summary>
        public Encoding Encoding
        {
            get { return _encoding; }
        }

        /// <summary>
        /// DataSet
        /// </summary>
        public IDataTable Table // CGX AERO GLZ
        {
            get
            {
                if (!_attributesPopulated && !string.IsNullOrEmpty(Filename))
                    Fill(_numRecords);
                return _dataTable;
            }

            set { _dataTable = value; }
        }

        /// <summary>
        /// Last date written to
        /// </summary>
        public DateTime UpdateDate
        {
            get { return _updateDate; }
        }

        /// <summary>
        /// Gets the list of raw row numbers that have been marked for deletion
        /// </summary>
        public List<int> DeletedRows
        {
            get { return _deletedRows; }
        }

        /// <summary>
        /// Fires the AttributesFilled event
        /// </summary>
        protected virtual void OnAttributesFilled()
        {
            var h = AttributesFilled;
            if (h != null) h(this, EventArgs.Empty);
        }

        #endregion
    }
}