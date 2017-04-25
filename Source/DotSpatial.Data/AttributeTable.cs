// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
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
    /// A class for controlling the attributetTable related information for a shapefile.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AttributeTable
    {
        // Constant for the size of a record
        private const int FileDescriptorSize = 32;
        private List<Field> _columns;
        private DataTable _dataTable;
        private string _fileName;
        private bool _hasDeletedRecords;

        /// <summary>
        /// Indicates that the Fill methode is called from inside itself.
        /// </summary>
        private bool _isFilling;
        private byte _ldid;
        private bool _loaded;
        private int _numFields;
        private long[] _offsets;
        private IProgressHandler _progressHandler;
        private BinaryWriter _writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeTable"/> class with no file reference.
        /// </summary>
        public AttributeTable()
        {
            DeletedRows = new List<int>();
            FileType = 0x03;
            Encoding = Encoding.Default;
            _ldid = DbaseLocaleRegistry.GetLanguageDriverId(Encoding);
            _progressHandler = DataManager.DefaultDataManager.ProgressHandler;
            ProgressMeter = new ProgressMeter(_progressHandler);
            _dataTable = new DataTable();
            _columns = new List<Field>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeTable"/> class with the specified fileName,
        /// or opens an existing file with that name.
        /// </summary>
        /// <param name="fileName">File that should be opened.</param>
        public AttributeTable(string fileName)
            : this()
        {
            Open(fileName);
        }

        /// <summary>
        /// Occurs after content has been loaded into the attribute data.
        /// </summary>
        public event EventHandler AttributesFilled;

        /// <summary>
        /// Gets or sets a value indicating whether the Attributes have been populated. If data was "opened" from a file, and a query is made
        /// to the DataTable  while _attributesPopulated is false, then a Fill method will be called automatically.
        /// </summary>
        public bool AttributesPopulated { get; set; }

        /// <summary>
        /// Gets an enumerable of the columns.
        /// </summary>
        public IEnumerable<Field> Columns => _columns;

        /// <summary>
        /// Gets the list of raw row numbers that have been marked for deletion.
        /// </summary>
        public List<int> DeletedRows { get; private set; }

        /// <summary>
        /// Gets the encoding used for text-based data and column names (based on LDID).
        /// </summary>
        public Encoding Encoding { get; private set; }

        /// <summary>
        /// Gets or sets the file name of the dbf file. If a relative path gets assigned it is changed to the absolute path including the file extension.
        /// </summary>
        public string Filename
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = Path.GetFullPath(value);
            }
        }

        /// <summary>
        /// Gets the file type.
        /// </summary>
        public byte FileType { get; private set; }

        /// <summary>
        /// Gets the byte length of the header.
        /// </summary>
        public int HeaderLength { get; private set; }

        /// <summary>
        /// Gets the language driver ID (LDID) for this file.
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
        /// Gets the number of records in this attribute table.
        /// </summary>
        public int NumRecords { get; private set; }

        /// <summary>
        /// Gets or sets the progress handler for this Attribute Table.
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get
            {
                return _progressHandler;
            }

            set
            {
                _progressHandler = value;
                ProgressMeter = new ProgressMeter(_progressHandler);
            }
        }

        /// <summary>
        /// Gets the byte length of each record.
        /// </summary>
        public int RecordLength { get; private set; }

        /// <summary>
        /// Gets or sets the table containing the whole attribute data.
        /// </summary>
        public DataTable Table
        {
            get
            {
                if (!AttributesPopulated && !string.IsNullOrEmpty(Filename)) Fill(NumRecords);
                return _dataTable;
            }

            set
            {
                _dataTable = value;
            }
        }

        /// <summary>
        /// Gets the date of the last writing.
        /// </summary>
        public DateTime UpdateDate { get; private set; }

        /// <summary>
        /// Gets or sets the progress meter that is directly tied to the progress handler.
        /// </summary>
        protected ProgressMeter ProgressMeter { get; set; }

        /// <summary>
        /// Saves the new row to the data source and updates the file with new content.
        /// </summary>
        /// <param name="values">The values organized against the dictionary of field names.</param>
        public virtual void AddRow(Dictionary<string, object> values)
        {
            // Step 1) Modify the file structure to allow an additional row
            NumRecords += 1;
            using (var bw = GetBinaryWriter())
            {
                WriteHeader(bw);
                int rawRow = GetFileIndex(NumRecords - 1);
                bw.BaseStream.Seek(HeaderLength + RecordLength * rawRow, SeekOrigin.Begin);
                byte[] blank = new byte[RecordLength];
                _writer.Write(blank); // the deleted flag
            }

            // Step 2) Re-use the insert code to insert the values for the new row.
            Edit(NumRecords - 1, values);
        }

        /// <summary>
        /// Saves the new row to the data source and updates the file with new content.
        /// </summary>
        /// <param name="values">The values organized against the dictionary of field names.</param>
        public virtual void AddRow(DataRow values)
        {
            // Test to see if the file exists yet. If not, simply create it with the expectation
            // that there will simply be one row in the data table.
            if (!File.Exists(_fileName))
            {
                _dataTable = new DataTable();
                foreach (DataColumn col in values.Table.Columns)
                {
                    _dataTable.Columns.Add(col.ColumnName, col.DataType, col.Expression);
                }

                DataRow newRow = _dataTable.NewRow();
                newRow.ItemArray = values.ItemArray;
                _dataTable.Rows.Add(newRow);
                SaveAs(_fileName, true);
                return;
            }

            // Step 1) Modify the file structure to allow an additional row
            NumRecords += 1;
            using (var bw = GetBinaryWriter())
            {
                WriteHeader(bw);
                int rawRow = GetFileIndex(NumRecords - 1);
                bw.BaseStream.Seek(HeaderLength + RecordLength * rawRow, SeekOrigin.Begin);
                byte[] blank = new byte[RecordLength];
                bw.Write(blank); // the deleted flag
            }

            // Step 2) Re-use the insert code to insert the values for the new row.
            Edit(NumRecords - 1, values);
        }

        /// <summary>
        /// Saves a single row to the data source.
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
        /// Saves a single row to the data source.
        /// </summary>
        /// <param name="index">the integer row (or FID) index</param>
        /// <param name="values">The object array holding the new values to store.</param>
        public virtual void Edit(int index, DataRow values)
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
        /// Saves a collection of rows to the data source.
        /// </summary>
        /// <param name="indexDataRowPairs">An IEnumerable that contains the DataRows that should be written along with the indices where the DataRows should be written.</param>
        public virtual void Edit(IEnumerable<KeyValuePair<int, DataRow>> indexDataRowPairs)
        {
            NumberConverter[] ncs = BeginEdit();
            try
            {
                foreach (KeyValuePair<int, DataRow> indexedDataRow in indexDataRowPairs)
                {
                    OverwriteDataRow(indexedDataRow.Key, indexedDataRow.Value, ncs);
                }
            }
            finally
            {
                EndEdit();
            }
        }

        /// <summary>
        /// Edit the specified rows via a client supplied callback.
        /// </summary>
        /// <param name="indices">rows to edit</param>
        /// <param name="rowCallback">client supplied callback</param>
        public virtual void Edit(IEnumerable<int> indices, RowEditEvent rowCallback)
        {
            if (GetFileLength() == 0) return;

            foreach (Field field in _columns)
            {
                field.NumberConverter = new NumberConverter(field.Length, field.DecimalCount);
            }

            var fields = new Fields(_columns);
            using (var myStream = new FileStream(_fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, RecordLength))
            {
                var e = new RowEditEventArgs(RecordLength - 1, fields);
                foreach (int rowIndex in indices)
                {
                    int strt = GetFileIndex(rowIndex);
                    long offset = HeaderLength + 1 + strt * RecordLength;
                    myStream.Seek(offset, SeekOrigin.Begin);
                    myStream.Read(e.ByteContent, 0, RecordLength - 1);
                    e.RowNumber = rowIndex;
                    e.Modified = false;
                    if (rowCallback(e)) return;
                    if (e.Modified)
                    {
                        myStream.Seek(offset, SeekOrigin.Begin);
                        myStream.Write(e.ByteContent, 0, RecordLength - 1);
                    }
                }
            }
        }

        /// <summary>
        /// Exports the dbf file content to a stream.
        /// </summary>
        /// <returns>A stream that contains the dbf file content.</returns>
        public Stream ExportDbfToStream()
        {
            MemoryStream dbfStream = new MemoryStream();
            UpdateSchema();

            try
            {
                using (var inMemoryStream = new MemoryStream())
                using (_writer = new BinaryWriter(inMemoryStream))
                {
                    WriteHeader(_writer);
                    WriteTable();
                    inMemoryStream.Seek(0, SeekOrigin.Begin);
                    inMemoryStream.CopyTo(dbfStream);
                    _writer.Close();
                }
            }
            finally
            {
                dbfStream.Seek(0, SeekOrigin.Begin);
            }

            return dbfStream;
        }

        /// <summary>
        /// This populates the Table with data from the file.
        /// </summary>
        /// <param name="numRows">In the event that the dbf file is not found, this indicates how many blank rows should exist in the attribute Table.</param>
        public void Fill(int numRows)
        {
            if (_isFilling)
                return;

            // Changed by jany_ (2015-07-30) don't load again because the fill methode is called from inside the fill methode and we'd get a datatable that is filled with twice the existing records
            AttributesPopulated = false;
            _dataTable.Rows.Clear(); // if we have already loaded data, clear the data.
            _isFilling = true;
            if (!File.Exists(_fileName))
            {
                NumRecords = numRows;
                _dataTable.BeginLoadData();
                if (!_dataTable.Columns.Contains("FID"))
                {
                    _dataTable.Columns.Add("FID", typeof(int));
                }

                for (int row = 0; row < numRows; row++)
                {
                    DataRow dr = _dataTable.NewRow();
                    dr["FID"] = row;
                    _dataTable.Rows.Add(dr);
                }

                _dataTable.EndLoadData();
                return;
            }

            if (!_loaded) GetRowOffsets();
            ProgressMeter = new ProgressMeter(ProgressHandler, "Reading from DBF Table...", NumRecords);
            if (NumRecords < 10000) ProgressMeter.StepPercent = 100;
            else if (NumRecords < 100000) ProgressMeter.StepPercent = 50;
            else if (NumRecords < 5000000) ProgressMeter.StepPercent = 10;
            else if (NumRecords < 10000000) ProgressMeter.StepPercent = 5;

            _dataTable.BeginLoadData();

            // Reading the Table elements as well as the shapes in a single progress loop.
            using (var myReader = GetBinaryReader())
            {
                for (int row = 0; row < NumRecords; row++)
                {
                    DataRow nextRow = null;
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
                        if (nextRow != null) _dataTable.Rows.Add(nextRow);
                    }

                    // If a progress message needs to be updated, this will handle that.
                    ProgressMeter.CurrentValue = row;
                }
            }

            ProgressMeter.Reset();
            _dataTable.EndLoadData();

            AttributesPopulated = true;
            OnAttributesFilled();
            _isFilling = false;
        }

        /// <summary>
        /// Gets a DataTable containing the specified attribute rows.
        /// </summary>
        /// <param name="rowNumbers">The numbers of the rows that should be returned.</param>
        /// <returns>A dataTable containing the specified attribute rows.</returns>
        public DataTable GetAttributes(IEnumerable<int> rowNumbers)
        {
            double fileLength = GetFileLength();
            if (fileLength == 0) return null; // The file is empty, so we are done here

            using (var myReader = GetBinaryReader())
            {
                // Encoding appears to be ASCII, not Unicode
                myReader.BaseStream.Seek(HeaderLength + 1, SeekOrigin.Begin);

                DataTable result = new DataTable();

                foreach (Field field in _columns)
                {
                    result.Columns.Add(new Field(field.ColumnName, field.TypeCharacter, field.Length, field.DecimalCount));
                }

                int maxRawRow = (int)((fileLength - (HeaderLength + 1)) / RecordLength);

                foreach (int rowNumber in rowNumbers)
                {
                    int rawRow = GetFileIndex(rowNumber);
                    if (rawRow > maxRawRow) break;

                    myReader.BaseStream.Seek(HeaderLength + 1 + rawRow * RecordLength, SeekOrigin.Begin);
                    byte[] byteContent = myReader.ReadBytes(RecordLength);
                    result.Rows.Add(ReadTableRow(rawRow, 0, byteContent, result));
                }

                return result;
            }
        }

        /// <summary>
        /// Accounts for deleted rows and adjusts a file index to a row index.
        /// </summary>
        /// <param name="fileIndex"></param>
        /// <returns></returns>
        public int GetRowIndexFromFileIndex(int fileIndex)
        {
            if (DeletedRows == null || DeletedRows.Count == 0) return fileIndex;

            int count = DeletedRows.Count(row => row < fileIndex);
            return fileIndex - count;
        }

        /// <summary>
        /// Reads all the information from the file, including the vector shapes and the database component.
        /// </summary>
        /// <param name="fileName">File that should be opened.</param>
        public void Open(string fileName)
        {
            // Open and search for deleted records
            Open(fileName, null);
        }

        /// <summary>
        /// Reads the header and if deletedRows is null, searches file for deletedRows if file size indicates possibility of deleted rows.
        /// </summary>
        /// <param name="fileName">Name of the file that should be opened.</param>
        /// <param name="deletedRows">List with the indices of the deleted rows.</param>
        public void Open(string fileName, List<int> deletedRows)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName), "fileName is null or empty.");

            Filename = Path.ChangeExtension(fileName, ".dbf");
            Contract.Assert(File.Exists(_fileName), "The dbf file for this shapefile was not found.");

            AttributesPopulated = false; // we had a file, but have not read the dbf content into memory yet.
            _dataTable = new DataTable();

            using (var myReader = GetBinaryReader())
            {
                ReadTableHeader(myReader); // based on the header, set up the fields information etc.

                // If the deleted rows were passed in, we don't need to look for them
                if (deletedRows != null)
                {
                    DeletedRows = deletedRows;
                    _hasDeletedRecords = DeletedRows.Count > 0;
                    return;
                }

                FileInfo fi = new FileInfo(_fileName);
                long length = HeaderLength + NumRecords * RecordLength;
                long pos = myReader.BaseStream.Position;
                if (HasEof(myReader.BaseStream, length)) length++;

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
                while (count < NumRecords)
                {
                    if (myReader.BaseStream.ReadByte() == (byte)' ')
                    {
                        count++;
                    }
                    else
                    {
                        DeletedRows.Add(row);
                    }

                    row++;
                    myReader.BaseStream.Seek(RecordLength - 1, SeekOrigin.Current);
                }
            }
        }

        /// <summary>
        /// Removes the row at the specified row index from both the table in memory
        /// and by marking it as deleted in the file. This should work even in cases
        /// where the file is very large and working as an AttributeSource.
        /// </summary>
        /// <param name="index">Index of the row that should be removed.</param>
        public virtual void RemoveRowAt(int index)
        {
            // If the data table is loaded with values, remove the member from the table
            if (AttributesPopulated)
            {
                if (Table.Rows.Count > index) Table.Rows.RemoveAt(index);
            }

            // Modify the header with the new row count
            NumRecords -= 1;
            using (var bw = GetBinaryWriter())
            {
                WriteHeader(bw);

                // Identify the matching row index in the file by accounting for existing deleted rows.
                int rawRow = GetFileIndex(index);

                // Add the file row to the "deleted" list. Future row index access will skip the deleted row.
                DeletedRows.Add(rawRow);
                DeletedRows.Sort();

                // Write an * to mark the row as deleted so that it doesn't appear.
                bw.BaseStream.Seek(HeaderLength + RecordLength * rawRow, SeekOrigin.Begin);
                bw.Write("*"); // the deleted flag
            }

            // Re-calculate row offsets stored in _offsets
            if (_offsets != null) GetRowOffsets();
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
        /// Saves this Table to the specified fileName.
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

            if (File.Exists(fileName) && !overwrite)
            {
                throw new IOException("File Exists and overwrite was set to false.");
            }

            Filename = fileName;
            Save();
        }

        /// <summary>
        /// Reads just the content requested in order to satisfy the paging ability of VirtualMode for the DataGridView.
        /// </summary>
        /// <param name="startRow">The 0 based integer index of the start row</param>
        /// <param name="dataValues">The DataTable with the 0 row corresponding to the start row. If this exceeds the size of the data table, it will add rows.</param>
        public void SetAttributes(int startRow, DataTable dataValues)
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

        /// <summary>
        /// Reads just the content requested in order to satisfy the paging ability of VirtualMode for the DataGridView.
        /// </summary>
        /// <param name="lowerPageBoundary">starting row</param>
        /// <param name="rowsPerPage">number of rows to return</param>
        /// <returns>A DataTable containing the data that was read.</returns>
        public DataTable SupplyPageOfData(int lowerPageBoundary, int rowsPerPage)
        {
            double fileLength = GetFileLength();
            if (fileLength == 0) return null; // The file is empty, so we are done here

            using (var myReader = GetBinaryReader())
            {
                // Encoding appears to be ASCII, not Unicode
                myReader.BaseStream.Seek(HeaderLength + 1, SeekOrigin.Begin);

                int maxRawRow = (int)((fileLength - (HeaderLength + 1)) / RecordLength);
                int strt = GetFileIndex(lowerPageBoundary);
                int end = GetFileIndex(lowerPageBoundary + rowsPerPage);
                int rawRows = end - strt;
                int length = rawRows * RecordLength;
                long offset = strt * RecordLength;

                myReader.BaseStream.Seek(offset, SeekOrigin.Current);
                byte[] byteContent = myReader.ReadBytes(length);
                DataTable result = new DataTable();
                foreach (Field field in _columns)
                {
                    result.Columns.Add(new Field(field.ColumnName, field.TypeCharacter, field.Length, field.DecimalCount));
                }

                int start = 0;
                for (int row = lowerPageBoundary; row < lowerPageBoundary + rowsPerPage; row++)
                {
                    if (row > maxRawRow) break;
                    result.Rows.Add(ReadTableRow(GetFileIndex(row) - strt, start, byteContent, result));
                    start += RecordLength;
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
        /// <returns>The data that was read.</returns>
        public object[] SupplyPageOfData(int lowerPageBoundary, int rowsPerPage, string fieldName)
        {
            double fileLength = GetFileLength();
            if (fileLength == 0) return null; // file is empty

            using (var myStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 100000))
            {
                int maxRawRow = (int)((fileLength - (HeaderLength + 1)) / RecordLength);
                Field field = _columns[_dataTable.Columns[fieldName].Ordinal];
                int column = _dataTable.Columns[fieldName].Ordinal;
                int fieldLength = _columns[column].Length;
                int columnOffset = GetColumnOffset(column);

                object[] result = new object[rowsPerPage];
                int outRow = 0;
                byte[] byteContent = new byte[fieldLength];
                for (int row = lowerPageBoundary; row < lowerPageBoundary + rowsPerPage; row++)
                {
                    if (row > maxRawRow) break;
                    long offset = HeaderLength + 1 + RecordLength * GetFileIndex(row) + columnOffset;
                    myStream.Seek(offset, SeekOrigin.Begin);

                    int current = 0;
                    while (current < fieldLength)
                    {
                        current += myStream.Read(byteContent, current, fieldLength - current);
                    }

                    result[outRow++] = ParseColumn(field, row, byteContent, 0, fieldLength, null);
                }

                return result;
            }
        }

        /// <summary>
        /// Reads just the contents requested. Faster than returning the entire record if you have lots of attributes but only want a few.
        /// </summary>
        /// <param name="lowerPageBoundary">starting row</param>
        /// <param name="rowsPerPage">number of rows to return</param>
        /// <param name="fieldNames">fields for which data is to be returned</param>
        /// <returns>The data that was read.</returns>
        public object[,] SupplyPageOfData(int lowerPageBoundary, int rowsPerPage, IEnumerable<string> fieldNames)
        {
            double fileLength = GetFileLength();
            if (fileLength == 0) return null; // file is empty

            using (var myStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 100000))
            {
                var maxRawRow = (int)((fileLength - (HeaderLength + 1)) / RecordLength);

                // Set up before looping over the rows.
                var fieldNamesArray = fieldNames.ToArray();
                var numFields = fieldNamesArray.Length;
                var fields = new Field[numFields];
                var result = new object[rowsPerPage, numFields];
                var outRow = 0;
                int largestField = 0;
                var columnOffsets = new int[numFields];
                var columnList = new List<KeyValuePair<int, int>>();
                for (var fieldNumber = 0; fieldNumber < numFields; fieldNumber++)
                {
                    var field = _columns[_dataTable.Columns[fieldNamesArray[fieldNumber]].Ordinal];
                    fields[fieldNumber] = field;
                    if (field.Length > largestField) largestField = field.Length;
                    var column = _dataTable.Columns[fieldNamesArray[fieldNumber]].Ordinal;
                    columnList.Add(new KeyValuePair<int, int>(column, fieldNumber));
                    columnOffsets[fieldNumber] = GetColumnOffset(column);
                }

                var byteContent = new byte[largestField]; // We reuse the byte storage for every single field.
                columnList.Sort(CompareKvpByKey);

                // We want to read the attributes in order for each row because it is faster.
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
                        long offset = HeaderLength + 1 + RecordLength * fileIndex + columnOffsets[fieldNumber];
                        myStream.Seek(offset, SeekOrigin.Begin);
                        var field = fields[fieldNumber];

                        int current = 0;
                        while (current < field.Length)
                        {
                            current += myStream.Read(byteContent, current, field.Length - current);
                        }

                        result[outRow, fieldNumber] = ParseColumn(field, row, byteContent, 0, field.Length, null);
                    }

                    outRow++;
                }

                return result;
            }
        }

        /// <summary>
        /// This systematically copies all the existing values to a new data column with the same properties,
        /// but with a new data type. Values that cannot convert will be set to null.
        /// </summary>
        /// <param name="oldDataColumn">The old data column to update</param>
        /// <param name="newDataType">The new data type that the column should become</param>
        /// <param name="currentRow">The row up to which values should be changed for</param>
        /// <param name="columnIndex">The column index of the field being changed</param>
        /// <param name="table"> The Table to apply this strategy to.</param>
        /// <returns>An integer list showing the index values of the rows where the conversion failed.</returns>
        public List<int> UpgradeColumn(Field oldDataColumn, Type newDataType, int currentRow, int columnIndex, DataTable table)
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
                    {
                        newValues[row] = null;
                    }
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
                if (newValues[row] == null) table.Rows[row][name] = DBNull.Value;
                else table.Rows[row][name] = newValues[row];
            }

            return failureList;
        }

        /// <summary>
        /// Writes the given number.
        /// </summary>
        /// <param name="number">Number that should be written.</param>
        /// <param name="length">Length of the data that should be written. If number is smaller than this it gets padded.</param>
        /// <param name="decimalCount">Number of places after the comma.</param>
        public void Write(double number, int length, int decimalCount)
        {
            // write with 19 chars.
            string format = "{0:";
            for (int i = 0; i < decimalCount; i++)
            {
                if (i == 0) format = format + "0.";
                format = format + "0";
            }

            format = format + "}";
            string str = string.Format(format, number);
            for (int i = 0; i < length - str.Length; i++) _writer.Write((byte)0x20);
            foreach (char c in str) _writer.Write(c);
        }

        /// <summary>
        /// Writes an integer so that it is formatted for dbf. This is still buggy since it is possible to lose info here.
        /// </summary>
        /// <param name="number">The long value</param>
        /// <param name="length">The length of the field.</param>
        /// <param name="decimalCount">The number of digits after the decimal</param>
        public void Write(long number, int length, int decimalCount)
        {
            string str = number.ToString();
            if (str.Length > length) str = str.Substring(str.Length - length, length);
            for (int i = 0; i < length - str.Length; i++) _writer.Write((byte)0x20);
            foreach (char c in str) _writer.Write(c);
        }

        /// <summary>
        /// Writes the given text with the given length.
        /// </summary>
        /// <param name="text">Text that should be written.</param>
        /// <param name="length">Number of bytes that may be written.</param>
        public void Write(string text, int length)
        {
            // Some encodings (multi-byte encodings) for languages such as Chinese, can result in variable number of bytes per character
            // so we convert character by character until we reach the length, and pad remaining bytes with spaces.
            // Note this replaces padding suggested at http://www.mapwindow.org/phorum/read.php?13,16820
            char[] characters = text.ToCharArray();

            int totalBytes = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                byte[] charBytes = Encoding.GetBytes(characters, i, 1);
                if (totalBytes + charBytes.Length > length) break;
                _writer.Write(charBytes);
                totalBytes += charBytes.Length;
            }

            if (totalBytes < length)
            {
                WriteSpaces(length - totalBytes);
            }
        }

        /// <summary>
        /// Writes the given flag.
        /// </summary>
        /// <param name="flag">Flag that should be written.</param>
        public void Write(bool flag)
        {
            _writer.Write(flag ? 'T' : 'F');
        }

        /// <summary>
        /// Writes the given date.
        /// </summary>
        /// <param name="date">Date that should be written.</param>
        public void WriteDate(DateTime date)
        {
            // YYYYMMDD format
            string test = date.ToString("yyyyMMdd");
            Write(test, 8);
        }

        /// <summary>
        /// Write the header data to the DBF file.
        /// </summary>
        /// <param name="writer">BinaryWriter used for writing.</param>
        public void WriteHeader(BinaryWriter writer)
        {
            // write the output file type.
            writer.Write(FileType);

            writer.Write((byte)(UpdateDate.Year - 1900));
            writer.Write((byte)UpdateDate.Month);
            writer.Write((byte)UpdateDate.Day);

            // write the number of records in the datafile.
            writer.Write(NumRecords);

            // write the length of the header structure.
            writer.Write((short)HeaderLength); // 32 + 30 * numColumns

            // write the length of a record
            writer.Write((short)RecordLength);

            // write reserved/unused bytes in the header
            for (int i = 0; i < 17; i++) writer.Write((byte)0);

            // write Language Driver ID (LDID)
            writer.Write(_ldid);

            // write remaining reserved/unused bytes in the header
            for (int i = 0; i < 2; i++) writer.Write((byte)0);

            // write all of the header records
            foreach (Field currentField in _columns)
            {
                // write the field name (can't be more than 11 bytes)
                char[] characters = currentField.ColumnName.ToCharArray();
                int totalBytes = 0;
                for (int j = 0; j < characters.Length; j++)
                {
                    byte[] charBytes = Encoding.GetBytes(characters, j, 1);
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
                int len = RecordLength - 1;
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
        /// Fires the AttributesFilled event.
        /// </summary>
        protected virtual void OnAttributesFilled()
        {
            AttributesFilled?.Invoke(this, EventArgs.Empty);
        }

        private static int CompareKvpByKey(KeyValuePair<int, int> x, KeyValuePair<int, int> y)
        {
            return x.Key - y.Key;
        }

        private static bool HasEof(Stream fs, long length)
        {
            fs.Seek(length, SeekOrigin.Begin);
            return fs.ReadByte() == 0x1a;
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

            // overridden in sub-classes
            _writer = GetBinaryWriter();
            return ncs;
        }

        /// <summary>
        /// Tests to see if the list of columns contains the specified name or not.
        /// </summary>
        /// <param name="name">Name of the column we're looking for.</param>
        /// <returns>True, if the column exists.</returns>
        private bool ColumnNameExists(string name)
        {
            return _columns.Any(fld => fld.ColumnName == name);
        }

        /// <summary>
        /// Cleanup after finished calling OverwriteDataRow
        /// </summary>
        private void EndEdit()
        {
            _writer.Flush();
            _writer.Close();
        }

        private BinaryReader GetBinaryReader()
        {
            return new BinaryReader(new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 100000));
        }

        /// <summary>
        /// Gets a BinaryWriter that can be used to write to the file inside _fileName.
        /// </summary>
        /// <returns>Returns a BinaryWriter that can be used to write to the file inside _fileName.</returns>
        private BinaryWriter GetBinaryWriter()
        {
            return new BinaryWriter(new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 1000000));
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
        /// Accounts for deleted rows and returns the index as it appears in the file
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private int GetFileIndex(int rowIndex)
        {
            int count = 0;
            if (DeletedRows == null || DeletedRows.Count == 0) return rowIndex;
            foreach (int row in DeletedRows)
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
        /// Gets the size, in bytes of _fileName. If the size equals HeaderLength this returns 0.
        /// </summary>
        /// <returns>Returns 0 if the size equals the HeaderLength. Otherwise the size is returned.</returns>
        private double GetFileLength()
        {
            var fi = new FileInfo(_fileName);

            // if the lengths equal the file is empty
            if ((int)fi.Length == HeaderLength) return 0;
            return fi.Length;
        }

        private void GetRowOffsets()
        {
            double fileLength = GetFileLength();
            if (fileLength == 0) return;  // The file is empty, so we are done here

            if (_hasDeletedRecords)
            {
                int length = (int)(fileLength - HeaderLength - 1);
                int recordCount = length / RecordLength;
                _offsets = new long[NumRecords];
                int j = 0; // undeleted index
                using (var myReader = GetBinaryReader())
                {
                    for (int i = 0; i <= recordCount; i++)
                    {
                        // seek to byte
                        myReader.BaseStream.Seek(HeaderLength + 1 + i * RecordLength, SeekOrigin.Begin);
                        var cb = myReader.ReadByte();
                        if (cb != '*') _offsets[j] = i * RecordLength;
                        j++;
                        if (j == NumRecords) break;
                    }
                }
            }

            _loaded = true;
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
            var dataRow = values as DataRow;
            if (dataRow == null) dataDictionary = (Dictionary<string, object>)values;

            int rawRow = GetFileIndex(index);
            _writer.Seek(HeaderLength + RecordLength * rawRow, SeekOrigin.Begin);
            _writer.Write((byte)0x20); // the deleted flag
            int len = RecordLength - 1;
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

        /// <summary>
        /// Parse the character data for one column into an object ready for insertion into a data row.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="currentRow"></param>
        /// <param name="cBuffer"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private object ParseCharacterColumn(Field field, int currentRow, char[] cBuffer, DataTable table)
        {
            // If table is null, an exception will be thrown rather than attempting to upgrade the column when a parse error occurs.
            const string ParseErrString = "Cannot parse {0} at row {1:D}, column {2:D} ({3}) in file {4} using field type {5}, and no DataTable to upgrade column";

            // find the field type
            object tempObject = DBNull.Value;
            switch (field.TypeCharacter)
            {
                case 'L': // logical data type, one character (T, t, F, f, Y, y, N, n)
                    // Symbol | Data Type | Description
                    // -------+-----------+-------------------------------------------------------
                    //   L    |   Logical | 1 byte - initialized to 0x20 (space) otherwise T or F.
                    char tempChar = cBuffer[0];
                    switch (tempChar)
                    {
                        case ' ': // 0x20
                            break; // remains DBNull
                        case 'T':
                        case 't':
                        case 'Y': // non-standard
                        case 'y': // non-standard
                            tempObject = true;
                            break;
                        default:
                            tempObject = false;
                            break;
                    }

                    break;

                case 'C': // character record.
                    // Symbol | Data Type | Description
                    // -------+-----------+----------------------------------------------------------------------------
                    //   C    | Character | All OEM code page characters - padded with blanks to the width of the field.
                    for (var i = cBuffer.Length - 1; i >= 0; --i)
                    {
                        if (cBuffer[i] != ' ')
                        {
                            tempObject = new string(cBuffer, 0, i + 1);
                            break;
                        }
                    }

                    break;

                case 'D': // date data type.
                    // Symbol | Data Type | Description
                    // -------+-----------+----------------------------------------------------------
                    //   D    |      Date | 8 bytes - date stored as a string in the format YYYYMMDD.
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
                    }

                    break;

                case 'B':
                case 'F':
                case 'G':
                case 'M':
                case 'N': // number - ESRI uses N for doubles and floats
                    // Symbol | Data Type | Description
                    // -------+-----------+----------------------------------------------------------
                    //   B    |    Binary | 10 digits representing a .DBT block number. The number is stored as a string, right justified and padded with blanks.
                    //   F    |     Float | Number stored as a string, right justified, and padded with blanks to the width of the field.
                    //   G    |       OLE | 10 digits (bytes) representing a .DBT block number. The number is stored as a string, right justified and padded with blanks.
                    //   M    |      Memo | 10 digits (bytes) representing a .DBT block number. The number is stored as a string, right justified and padded with blanks.
                    //   N    |   Numeric | Number stored as a string, right justified, and padded with blanks to the width of the field.

                    // The Binary, Memo and OLE Fields actually contain pointers to a DBT File. But these files are
                    // currently not suppoted, so we just load theses fields as numbers.
                    tempObject = ParseNumericColumn(field, currentRow, cBuffer, table, ParseErrString);
                    break;

                default:
                    throw new NotSupportedException("Do not know how to parse Field type " + field.TypeCharacter);
            }

            return tempObject;
        }

        /// <summary>
        /// This function is the main entry point of parsing the columns.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="currentRow"></param>
        /// <param name="bBuffer"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="table"></param>
        /// <returns>The parsed value of the field or <see cref="DBNull.Value"/>.</returns>
        private object ParseColumn(Field field, int currentRow, byte[] bBuffer, int startIndex, int length, DataTable table)
        {
            Debug.Assert(startIndex >= 0, "startIndex must be >= 0");
            Debug.Assert(length > 0, "length must be > 0");
            Debug.Assert(startIndex + length <= bBuffer.Length, "startIndex + length must be <= bBuffer.Length");

            object result = DBNull.Value;
            switch (field.TypeCharacter)
            {
                case 'B': // Binary
                case 'C': // Character
                case 'D': // Date
                case 'N': // Numeric
                case 'L': // Logical
                case 'M': // Memo
                case 'G': // OLE
                case 'F': // Float
                    // It is a character based column.
                    char[] cBuffer = Encoding.GetChars(bBuffer, startIndex, length);
                    result = ParseCharacterColumn(field, currentRow, cBuffer, table);
                    break;
                case '@': // Timestamp
                    // 8 bytes - two longs, first for date, second for time.
                    // The date is the number of days since 01/01/4713 BC.
                    // Time is hours * 3600000L + minutes * 60000L + Seconds * 1000L
                    // So the time are actually milliseconds
                    if (length >= 8)
                    {
                        try
                        {
                            if (!BitConverter.IsLittleEndian)
                            {
                                Array.Reverse(bBuffer, startIndex, 4);
                                Array.Reverse(bBuffer, startIndex + 4, 4);
                            }

                            int julianDays = BitConverter.ToInt32(bBuffer, startIndex);
                            int time = BitConverter.ToInt32(bBuffer, startIndex + 4);

                            // now some magic numbers to calculate the actual time.
                            // got it from Wikipedia
                            // https://en.wikipedia.org/wiki/Julian_day#Julian_or_Gregorian_calendar_from_Julian_day_number
                            int f = julianDays + 1401;
                            int e = 4 * f * 3;
                            int g = (e % 1461) / 4;
                            int h = 5 * g * 2;
                            int day = (h % 153) / 5 + 1;
                            int month = ((h / 153 + 2) % 12) + 1;
                            int year = (e / 1461) - 4716 + (12 + 2 - month) / 12;
                            DateTime actualDate = new DateTime(year, day, month, new JulianCalendar());
                            actualDate = actualDate.AddMilliseconds(time);
                            result = actualDate;
                        }
                        catch (Exception)
                        {
                            // Decoding this *** failed. No just return a null value
                        }
                    }

                    break;
                case 'I': // Long
                case '+': // Long (Autoincrement)
                    // The documentation of dBase states:
                    // 4 bytes. Leftmost bit used to indicate sign, 0 negative.
                    // Something about this is very off. It makes no sense at all to encode values like this.
                    if (length >= 4)
                    {
                        if (!BitConverter.IsLittleEndian) Array.Reverse(bBuffer, startIndex, 4);
                        result = BitConverter.ToInt32(bBuffer, startIndex);
                    }

                    break;
                case 'O': // Double
                    if (length >= 8)
                    {
                        if (!BitConverter.IsLittleEndian) Array.Reverse(bBuffer, startIndex, 8);
                        result = BitConverter.ToDouble(bBuffer, startIndex);
                    }

                    break;
                default:
                    throw new NotSupportedException("Do not know how to parse Field type " + field.TypeCharacter);
            }

            return result;
        }

        /// <summary>
        /// Parses numeric columns that contain the numbers stored as numeric values astrings, right justified and
        /// padded with blanks. Each field is expected to start with a space or a astrisk indicating if the value is
        /// valid or <c>null</c>.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="currentRow"></param>
        /// <param name="cBuffer"></param>
        /// <param name="table"></param>
        /// <param name="parseErrString"></param>
        /// <returns></returns>
        private object ParseNumericColumn(Field field, int currentRow, char[] cBuffer, DataTable table, string parseErrString)
        {
            // Data records are preceded by one byte, that is, a space (0x20) if the record is not deleted, an asterisk (0x2A)
            // By this reference, some application (e.g. quantum GIS) fill fields like this that are a null value with
            // asterisk characters. To handle cases like this, the fields with * values are filtered. Doing to like
            // this has no negative impact on handling the numbers, since number fields should never start with a
            // asterisk under normal conditions.
            if (cBuffer[0] == '*')
            {
                return DBNull.Value;
            }

            string tempStr = null;
            for (var i = 0; i < cBuffer.Length; ++i)
            {
                if (cBuffer[i] != ' ')
                {
                    tempStr = new string(cBuffer, i, cBuffer.Length - i);
                    break;
                }
            }

            if (tempStr == null)
            {
                // Removing the padding yielded remaining string. This column is empty.
                return DBNull.Value;
            }

            object tempObject = DBNull.Value;
            Type t = field.DataType;
            var errorMessage = new Lazy<string>(() => string.Format(parseErrString, tempStr, currentRow, field.Ordinal, field.ColumnName, _fileName, t));

            if (t == typeof(byte))
            {
                byte temp;
                if (byte.TryParse(tempStr, out temp))
                {
                    tempObject = temp;
                }
                else
                {
                    // It is possible to store values larger than 255 with three characters.
                    // Therefore, we may have to upgrade the numeric type for the entire field to short.
                    if (table == null) throw new InvalidDataException(errorMessage.Value);
                    short upTest;
                    if (short.TryParse(tempStr, out upTest))
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
                if (short.TryParse(tempStr, out temp))
                {
                    tempObject = temp;
                }
                else
                {
                    if (table == null) throw new InvalidDataException(errorMessage.Value);
                    int upTest;
                    if (int.TryParse(tempStr, out upTest))
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
                if (int.TryParse(tempStr, out temp))
                {
                    tempObject = temp;
                }
                else
                {
                    if (table == null) throw new InvalidDataException(errorMessage.Value);
                    long upTest;
                    if (long.TryParse(tempStr, out upTest))
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
                if (long.TryParse(tempStr, out temp))
                {
                    tempObject = temp;
                }
                else
                {
                    if (table == null) throw new InvalidDataException(errorMessage.Value);
                    UpgradeColumn(field, typeof(string), currentRow, field.Ordinal, table);
                    tempObject = tempStr;
                }
            }
            else if (t == typeof(float))
            {
                float temp;
                if (float.TryParse(tempStr, NumberStyles.Number | NumberStyles.AllowExponent, NumberConverter.NumberConversionFormatProvider, out temp))
                {
                    tempObject = temp;
                }
                else
                {
                    if (table == null) throw new InvalidDataException(errorMessage.Value);
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
                {
                    tempObject = temp;
                }
                else if (string.IsNullOrWhiteSpace(tempStr))
                {
                    tempObject = DBNull.Value; // handle case when value is NULL
                }
                else
                {
                    if (table == null) throw new InvalidDataException(errorMessage.Value);
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
                {
                    tempObject = temp;
                }
                else
                {
                    if (table == null) throw new InvalidDataException(errorMessage.Value);
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
            FileType = reader.ReadByte();
            if (FileType != 0x03) throw new NotSupportedException("Unsupported DBF reader Type " + FileType);

            // parse the update date information.
            int year = reader.ReadByte();
            int month = reader.ReadByte();
            int day = reader.ReadByte();

            try
            {
                UpdateDate = new DateTime(year + 1900, month, day);
            }
            catch
            {
                // If the Update Date in the header is not in correct format, just use the modify time of the file
                UpdateDate = new FileInfo(_fileName).LastWriteTime;
            }

            // read the number of records.
            NumRecords = reader.ReadInt32();

            // read the length of the header structure.
            HeaderLength = reader.ReadInt16();

            // read the length of a record
            RecordLength = reader.ReadInt16();

            // skip reserved/unused bytes in the header.
            reader.ReadBytes(17);

            // read Language Driver ID (LDID)
            LanguageDriverId = reader.ReadByte();

            // skip remaining reserved/unused bytes in header
            reader.ReadBytes(2);

            // calculate the number of Fields in the header
            _numFields = (HeaderLength - FileDescriptorSize - 1) / FileDescriptorSize;

            _columns = new List<Field>();

            for (int i = 0; i < _numFields; i++)
            {
                // read the field name
                string name = Encoding.GetString(reader.ReadBytes(11));
                int nullPoint = name.IndexOf((char)0);
                if (nullPoint != -1) name = name.Substring(0, nullPoint);

                // read the field type
                char code = (char)reader.ReadByte();

                // read the field data address, offset from the start of the record.
                int dataAddress = reader.ReadInt32();

                // read the field length in bytes
                byte tempLength = reader.ReadByte();

                // read the field decimal count in bytes
                byte decimalcount = reader.ReadByte();

                // read the reserved bytes.
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

        /// <summary>
        /// Read a single dbase record.
        /// </summary>
        /// <param name="currentRow">Index of the row that should be read.</param>
        /// <param name="start"></param>
        /// <param name="byteContent"></param>
        /// <param name="table"></param>
        /// <returns>Returns a DataRow with the information of the current row.</returns>
        private DataRow ReadTableRow(int currentRow, int start, byte[] byteContent, DataTable table)
        {
            DataRow result = table.NewRow();
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

                if (len <= 0) return result;

                result[currentField.ColumnName] = ParseColumn(currentField, currentRow, byteContent, start, len, table);
                start += currentField.Length;
            }

            return result;
        }

        /// <summary>
        /// Read a single dbase record.
        /// </summary>
        /// <param name="currentRow">Index of the row that should be read.</param>
        /// <param name="myReader">Reader used for reading.</param>
        /// <returns>Returns a DataRow with the information of the current row.</returns>
        private DataRow ReadTableRowFromChars(int currentRow, BinaryReader myReader)
        {
            var result = _dataTable.NewRow();

            long start;
            if (_hasDeletedRecords == false) start = currentRow * RecordLength;
            else start = _offsets[currentRow];

            myReader.BaseStream.Seek(HeaderLength + 1 + start, SeekOrigin.Begin);

            byte[] byteBuffer = null;
            for (int col = 0; col < _dataTable.Columns.Count; col++)
            {
                // find the length of the field.
                var currentField = _columns[col];

                // reusing the byte buffer, no need to spam the GC
                if (byteBuffer == null || byteBuffer.Length < currentField.Length) byteBuffer = new byte[currentField.Length];

                // read the data.
                int current = 0;
                while (current < currentField.Length)
                {
                    current += myReader.Read(byteBuffer, current, currentField.Length - current);
                }

                result[currentField.ColumnName] = ParseColumn(currentField, currentRow, byteBuffer, 0, currentField.Length, _dataTable);
            }

            return result;
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
                    if (cpgFileName != null && File.Exists(cpgFileName))
                    {
                        using (StreamReader reader = new StreamReader(cpgFileName))
                        {
                            string codePageText = reader.ReadLine();
                            int codePage;
                            if (int.TryParse(codePageText, NumberStyles.Integer, CultureInfo.InvariantCulture, out codePage))
                            {
                                Encoding = Encoding.GetEncoding(codePage);
                                cpgSet = true;
                            }
                            else if (string.Compare(codePageText, "UTF-8", true, CultureInfo.InvariantCulture) == 0)
                            {
                                Encoding = Encoding.UTF8;
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
                Encoding = _ldid == 0x57 ? Encoding.Default : DbaseLocaleRegistry.GetEncoding(_ldid);
            }
        }

        private void UpdateSchema()
        {
            List<Field> tempColumns = new List<Field>();
            RecordLength = 1; // delete character
            NumRecords = _dataTable.Rows.Count;
            UpdateDate = DateTime.Now;
            HeaderLength = FileDescriptorSize + FileDescriptorSize * _dataTable.Columns.Count + 1;
            if (_columns == null) _columns = new List<Field>();

            // Delete any fields from the columns list that are no// longer in the data Table.
            List<Field> removeFields = new List<Field>();
            foreach (Field fld in _columns)
            {
                if (!_dataTable.Columns.Contains(fld.ColumnName)) removeFields.Add(fld);
                else tempColumns.Add(fld);
            }

            foreach (Field field in removeFields)
            {
                _columns.Remove(field);
            }

            // Add new columns that exist in the data Table, but don't have a matching field yet.
            tempColumns.AddRange(
                from DataColumn dc in _dataTable.Columns
                where !ColumnNameExists(dc.ColumnName)
                select dc as Field ?? new Field(dc));

            _columns = tempColumns;
            RecordLength = 1;

            // Recalculate the recordlength
            // current calculation fix proposed by Aerosol
            foreach (Field fld in Columns)
            {
                RecordLength = RecordLength + fld.Length;
            }
        }

        private void WriteColumnValue(object columnValue, int fld, NumberConverter[] ncs)
        {
            if (columnValue == null || columnValue is DBNull)
            {
                WriteSpaces(_columns[fld].Length);
            }
            else if (columnValue is decimal)
            {
                _writer.Write(ncs[fld].ToChar((decimal)columnValue));
            }
            else if (columnValue is double)
            {
                char[] test = ncs[fld].ToChar((double)columnValue);
                _writer.Write(test);
            }
            else if (columnValue is float)
            {
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
            {
                Write(Convert.ToInt64(columnValue), _columns[fld].Length, _columns[fld].DecimalCount);
            }
            else if (columnValue is bool)
            {
                Write((bool)columnValue);
            }
            else if (columnValue is string)
            {
                int length = _columns[fld].Length;
                Write((string)columnValue, length);
            }
            else if (columnValue is DateTime)
            {
                WriteDate((DateTime)columnValue);
            }
            else
            {
                Write(columnValue.ToString(), _columns[fld].Length);
            }
        }
    }
}