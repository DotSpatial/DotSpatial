// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in January, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DotSpatial.Projections;

namespace DotSpatial.Data
{
    /// <summary>
    /// This is a generic shapefile that is inherited by other specific shapefile types.
    /// </summary>
    public abstract class Shapefile : FeatureSet
    {
        #region Private Variables

        // Stores extents and some very basic info common to all shapefiles
        private AttributeTable _attributeTable;

        #endregion
        
        #region Constructors

        /// <summary>
        /// When creating a new shapefile, this simply prevents the basic values from being null.
        /// </summary>
        protected Shapefile()
        {
            Configure();
        }

        /// <summary>
        /// Creates a new shapefile that has a specific feature type.
        /// </summary>
        /// <param name="featureType">FeatureType of the features inside this shapefile.</param>
        /// <param name="shapeType">ShapeType of the features inside this shapefile.</param>
        protected Shapefile(FeatureType featureType, ShapeType shapeType)
            : base(featureType)
        {
            Attributes = new AttributeTable();
            Header = new ShapefileHeader { FileLength = 100, ShapeType = shapeType };
        }

        /// <summary>
        /// Creates a new instance of a shapefile based on a fileName.
        /// </summary>
        /// <param name="fileName">File name the shapefile is based on.</param>
        protected Shapefile(string fileName)
        {
            base.Filename = fileName;
            Configure();
        }

        private void Configure()
        {
            Attributes = new AttributeTable();
            Header = new ShapefileHeader();
            IndexMode = true;
        }

        #endregion

        /// <summary>
        /// The buffer size is an integer value in bytes specifying how large a piece of memory can be used at any one time.
        /// Reading and writing from the disk is faster when done all at once.  The larger this number the more effective
        /// the disk management, but the more ram will be required (and the more likely to trip an out of memory error).
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int BufferSize { get; set; } = 1;

        /// <summary>
        /// Gets whether or not the attributes have all been loaded into the data table.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool AttributesPopulated
        {
            get
            {
                return _attributeTable.AttributesPopulated;
            }

            set
            {
                _attributeTable.AttributesPopulated = value;
            }
        }

        /// <summary>
        /// This re-directs the DataTable to work with the attribute Table instead.
        /// </summary>
        public override DataTable DataTable
        {
            get
            {
                return _attributeTable.Table;
            }

            set
            {
                _attributeTable.Table = value;
            }
        }

        /// <summary>
        /// A general header structure that stores some basic information about the shapefile.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ShapefileHeader Header { get; set; }

        /// <summary>
        /// Gets or sets the attribute Table used by this shapefile.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AttributeTable Attributes
        {
            get { return _attributeTable; }
            set
            {
                if (_attributeTable == value) return;
                if (_attributeTable != null)
                {
                    _attributeTable.AttributesFilled -= AttributeTableAttributesFilled;
                }

                _attributeTable = value;
                if (_attributeTable != null)
                {
                    _attributeTable.AttributesFilled += AttributeTableAttributesFilled;
                }
            }
        }

        /// <summary>
        /// Gets the count of members that match the expression
        /// </summary>
        /// <param name="expressions">The string expression to test</param>
        /// <param name="progressHandler">THe progress handler that can also cancel the counting</param>
        /// <param name="maxSampleSize">The integer maximum sample size from which to draw counts.  If this is negative, it will not be used.</param>
        /// <returns>The integer count of the members that match the expression.</returns>
        public override int[] GetCounts(string[] expressions, ICancelProgressHandler progressHandler, int maxSampleSize)
        {
            if (AttributesPopulated) return base.GetCounts(expressions, progressHandler, maxSampleSize);

            int[] counts = new int[expressions.Length];

            // The most common case would be no filter expression, in which case the count is simply the number of shapes.
            bool requiresRun = false;
            for (int iex = 0; iex < expressions.Length; iex++)
            {
                if (!string.IsNullOrEmpty(expressions[iex]))
                {
                    requiresRun = true;
                }
                else
                {
                    counts[iex] = NumRows();
                }
            }

            if (!requiresRun) return counts;

            AttributePager ap = new AttributePager(this, 5000);
            ProgressMeter pm = new ProgressMeter(progressHandler, "Calculating Counts", ap.NumPages());

            // Don't bother to use a sampling approach if the number of rows is on the same order of magnitude as the number of samples.
            if (maxSampleSize > 0 && maxSampleSize < NumRows() / 2)
            {
                DataTable sample = new DataTable();
                sample.Columns.AddRange(GetColumns());
                Dictionary<int, int> usedRows = new Dictionary<int, int>();
                int samplesPerPage = maxSampleSize / ap.NumPages();
                Random rnd = new Random(DateTime.Now.Millisecond);
                for (int page = 0; page < ap.NumPages(); page++)
                {
                    for (int i = 0; i < samplesPerPage; i++)
                    {
                        int row;
                        do
                        {
                            row = rnd.Next(ap.StartIndex, ap.StartIndex + ap.PageSize);
                        }
                        while (usedRows.ContainsKey(row));
                        usedRows.Add(row, row);
                        sample.Rows.Add(ap.Row(row).ItemArray);
                    }

                    ap.MoveNext();

                    pm.CurrentValue = page;
                    if (progressHandler.Cancel) break;
                }

                for (int i = 0; i < expressions.Length; i++)
                {
                    try
                    {
                        DataRow[] dr = sample.Select(expressions[i]);
                        counts[i] += dr.Length;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }

                pm.Reset();
                return counts;
            }

            for (int page = 0; page < ap.NumPages(); page++)
            {
                for (int i = 0; i < expressions.Length; i++)
                {
                    DataRow[] dr = ap[page].Select(expressions[i]);
                    counts[i] += dr.Length;
                }

                pm.CurrentValue = page;
                if (progressHandler.Cancel) break;
            }

            pm.Reset();
            return counts;
        }

        /// <summary>
        /// This makes the assumption that the organization of the feature list has not
        /// changed since loading the attribute content.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AttributeTableAttributesFilled(object sender, EventArgs e)
        {
            if (IndexMode) return;
            for (int fid = 0; fid < Features.Count; fid++)
            {
                if (fid < _attributeTable.Table.Rows.Count)
                {
                    Features[fid].DataRow = _attributeTable.Table.Rows[fid];
                }
            }

            SetupFeatureLookup();
        }

        /// <summary>
        /// This will return the correct shapefile type by reading the fileName.
        /// </summary>
        /// <param name="fileName">A string specifying the file with the extension .shp to open.</param>
        /// <returns>A correct shapefile object which is exclusively for reading the .shp data</returns>
        public static new Shapefile OpenFile(string fileName)
        {
            return OpenFile(fileName, DataManager.DefaultDataManager.ProgressHandler);
        }

        /// <summary>
        /// This will return the correct shapefile type by reading the fileName.
        /// </summary>
        /// <param name="fileName">A string specifying the file with the extension .shp to open.</param>
        /// <param name="progressHandler">receives progress messages and overrides the ProgressHandler on the DataManager.DefaultDataManager</param>
        /// <exception cref="NotImplementedException">Throws a NotImplementedException if the ShapeType of the loaded file is NullShape or MultiPatch.</exception>
        /// <returns>A correct shapefile object which is exclusively for reading the .shp data</returns>
        public static new Shapefile OpenFile(string fileName, IProgressHandler progressHandler)
        {
            var ext = Path.GetExtension(fileName)?.ToLower();
            if (ext != ".shp" && ext != ".shx" && ext != ".dbf")
                throw new ArgumentException(string.Format(DataStrings.FileExtensionNotSupportedByShapefileDataProvider, ext));

            string name = Path.ChangeExtension(fileName, ".shp");
            var head = new ShapefileHeader();
            head.Open(name);
            switch (head.ShapeType)
            {
                case ShapeType.MultiPatch:
                case ShapeType.NullShape:
                    throw new NotImplementedException(DataStrings.ShapeTypeNotYetSupported);

                case ShapeType.MultiPoint:
                case ShapeType.MultiPointM:
                case ShapeType.MultiPointZ:
                    var mpsf = new MultiPointShapefile();
                    mpsf.Open(name, progressHandler);
                    return mpsf;
             
                case ShapeType.Point:
                case ShapeType.PointM:
                case ShapeType.PointZ:
                    var psf = new PointShapefile();
                    psf.Open(name, progressHandler);
                    return psf;

                case ShapeType.Polygon:
                case ShapeType.PolygonM:
                case ShapeType.PolygonZ:
                    var pgsf = new PolygonShapefile();
                    pgsf.Open(name, progressHandler);
                    return pgsf;

                case ShapeType.PolyLine:
                case ShapeType.PolyLineM:
                case ShapeType.PolyLineZ:
                    var lsf = new LineShapefile();
                    lsf.Open(name, progressHandler);
                    return lsf;
            }

            return null;
        }

        /// <summary>
        /// saves a single row to the data source.
        /// </summary>
        /// <param name="index">the integer row (or FID) index</param>
        /// <param name="values">The object array holding the new values to store.</param>
        public override void Edit(int index, Dictionary<string, object> values)
        {
            _attributeTable.Edit(index, values);
        }

        /// <summary>
        /// saves a single row to the data source.
        /// </summary>
        /// <param name="index">the integer row (or FID) index</param>
        /// <param name="values">The object array holding the new values to store.</param>
        public override void Edit(int index, DataRow values)
        {
            _attributeTable.Edit(index, values);
        }

        /// <summary>
        /// Saves the new row to the data source and updates the file with new content.
        /// </summary>
        /// <param name="values">The values organized against the dictionary of field names.</param>
        public override void AddRow(Dictionary<string, object> values)
        {
            _attributeTable.AddRow(values);
        }

        /// <summary>
        /// Saves the new row to the data source and updates the file with new content.
        /// </summary>
        /// <param name="values">The values organized against the dictionary of field names.</param>
        public override void AddRow(DataRow values)
        {
            _attributeTable.AddRow(values);
        }

        /// <inheritdoc />
        public override DataTable GetAttributes(int startIndex, int numRows)
        {
            return _attributeTable.SupplyPageOfData(startIndex, numRows);
        }

        /// <summary>
        /// Converts a page of content from a DataTable format, saving it back to the source.
        /// </summary>
        /// <param name="startIndex">The 0 based integer index representing the first row in the file (corresponding to the 0 row of the data table)</param>
        /// <param name="pageValues">The DataTable representing the rows to set.  If the row count is larger than the dataset, this will add the rows instead.</param>
        public new void SetAttributes(int startIndex, DataTable pageValues)
        {
            // overridden in sub-classes
            _attributeTable.SetAttributes(startIndex, pageValues);
        }

        /// <inheritdoc />
        public override List<IFeature> SelectByAttribute(string filterExpression)
        {
            if (!_attributeTable.AttributesPopulated)
            {
                _attributeTable.Fill(_attributeTable.NumRecords);
            }

            if (FeatureLookup.Count == 0)
            {
                SetupFeatureLookup();
            }

            return base.SelectByAttribute(filterExpression);
        }

        /// <summary>
        /// Reads the attributes from the specified attribute Table.
        /// </summary>
        public override void FillAttributes()
        {
            _attributeTable.AttributesPopulated = false; // attributeTable.Table fills itself if attributes are not populated
            DataTable = _attributeTable.Table;
            base.AttributesPopulated = true;

            // Link the data rows to the vectors in this object
        }

        /// <summary>
        /// Sets up the feature lookup, if it has not been already.
        /// </summary>
        private void SetupFeatureLookup()
        {
            for (var i = 0; i < _attributeTable.Table.Rows.Count; i++)
            {
                Features[i].DataRow = _attributeTable.Table.Rows[i];
                FeatureLookup[Features[i].DataRow] = Features[i];
            }
        }

        /// <summary>
        /// This doesn't rewrite the entire header or erase the existing content.  This simply replaces the file length
        /// in the file with the new file length.  This is generally because we want to write the header first,
        /// but don't know the total length of a new file until cycling through the entire file.  It is easier, therefore
        /// to update the length after editing.
        /// </summary>
        /// <param name="fileName">A string fileName</param>
        /// <param name="length">The integer length of the file in 16-bit words</param>
        public static void WriteFileLength(string fileName, int length)
        {
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                WriteFileLength(fs, length);
            }
        }

        /// <summary>
        /// Operates on abritrary stream.
        /// This doesn't rewrite the entire header or erase the existing content.  This simply replaces the file length
        /// in the file with the new file length.  This is generally because we want to write the header first,
        /// but don't know the total length of a new file until cycling through the entire file.  It is easier, therefore
        /// to update the length after editing.
        /// Note: performs seek
        /// </summary>
        /// <param name="stream">stream to edit</param>
        /// <param name="length">The integer length of the file in 16-bit words</param>
        public static void WriteFileLength(Stream stream, int length)
        {
            stream.Seek(0, SeekOrigin.Begin);
            byte[] headerData = new byte[28];

            WriteBytes(headerData, 0, 9994, false); // Byte 0          File Code       9994        Integer     Big

            // Bytes 4 - 20 are unused
            WriteBytes(headerData, 24, length, false); // Byte 24         File Length     File Length Integer     Big
            var bw = new BinaryWriter(stream);

            // no using to avoid closing the stream 
            // Actually write our byte array to the file
            bw.Write(headerData);
        }

        /// <summary>
        /// Reads 4 bytes from the specified byte array starting with offset.
        /// If IsBigEndian = true, then this flips the order of the byte values.
        /// </summary>
        /// <param name="value">An array of bytes that is at least 4 bytes in length from the startIndex</param>
        /// <param name="startIndex">A 0 based integer index where the double value begins</param>
        /// <param name="isLittleEndian">If this is true, then the order of the bytes is reversed before being converted to a double</param>
        /// <returns>A double created from reading the byte array</returns>
        public static int ToInt(byte[] value, ref int startIndex, bool isLittleEndian)
        {
            // Some systems run BigEndian by default, others run little endian.
            // The parameter specifies the byte order that should exist on the file.
            // The BitConverter tells us what our system will do by default.
            if (isLittleEndian != BitConverter.IsLittleEndian)
            {
                // The byte order of the value to post doesn't match our system, so reverse the byte order.
                byte[] flipBytes = new byte[4];
                Array.Copy(value, startIndex, flipBytes, 0, 4);
                Array.Reverse(flipBytes);
                startIndex += 4;
                return BitConverter.ToInt32(flipBytes, 0);
            }

            startIndex += 4;
            return BitConverter.ToInt32(value, startIndex);
        }

        /// <summary>
        /// Reads 8 bytes from the specified byte array starting with offset.
        /// If IsBigEndian = true, then this flips the order of the byte values.
        /// </summary>
        /// <param name="value">An array of bytes that is at least 8 bytes in length from the startIndex</param>
        /// <param name="startIndex">A 0 based integer index where the double value begins</param>
        /// <param name="isLittleEndian">If this is true, then the order of the bytes is reversed before being converted to a double</param>
        /// <returns>A double created from reading the byte array</returns>
        public static double ToDouble(byte[] value, ref int startIndex, bool isLittleEndian)
        {
            // Some systems run BigEndian by default, others run little endian.
            // The parameter specifies the byte order that should exist on the file.
            // The BitConverter tells us what our system will do by default.
            if (isLittleEndian != BitConverter.IsLittleEndian)
            {
                byte[] flipBytes = new byte[8];
                Array.Copy(value, startIndex, flipBytes, 0, 8);
                Array.Reverse(flipBytes);
                startIndex += 8;
                return BitConverter.ToDouble(flipBytes, 0);
            }

            startIndex += 8;
            return BitConverter.ToDouble(value, startIndex);
        }

        /// <summary>
        /// Converts the double value into bytes and inserts them starting at startIndex into the destArray
        /// </summary>
        /// <param name="destArray">A byte array where the values should be written</param>
        /// <param name="startIndex">The starting index where the values should be inserted</param>
        /// <param name="value">The double value to convert</param>
        /// <param name="isLittleEndian">Specifies whether the value should be written as big or little endian</param>
        public static void WriteBytes(byte[] destArray, int startIndex, double value, bool isLittleEndian)
        {
            // Some systems run BigEndian by default, others run little endian.
            // The parameter specifies the byte order that should exist on the file.
            // The BitConverter tells us what our system will do by default.
            if (isLittleEndian != BitConverter.IsLittleEndian)
            {
                byte[] flipBytes = BitConverter.GetBytes(value);
                Array.Reverse(flipBytes);
                Array.Copy(flipBytes, 0, destArray, startIndex, 8);
            }
            else
            {
                byte[] flipBytes = BitConverter.GetBytes(value);
                Array.Copy(flipBytes, 0, destArray, startIndex, 8);
            }
        }

        /// <summary>
        /// Converts the double value into bytes and inserts them starting at startIndex into the destArray.
        /// This will correct this system's natural byte order to reflect what is required to match the
        /// shapefiles specification.
        /// </summary>
        /// <param name="destArray">A byte array where the values should be written</param>
        /// <param name="startIndex">The starting index where the values should be inserted</param>
        /// <param name="value">The integer value to convert</param>
        /// <param name="isLittleEndian">Specifies whether the value should be written as big or little endian</param>
        public static void WriteBytes(byte[] destArray, int startIndex, int value, bool isLittleEndian)
        {
            // Some systems run BigEndian by default, others run little endian.
            // The parameter specifies the byte order that should exist on the file.
            // The BitConverter tells us what our system will do by default.
            if (isLittleEndian != BitConverter.IsLittleEndian)
            {
                byte[] flipBytes = BitConverter.GetBytes(value);
                Array.Reverse(flipBytes);
                Array.Copy(flipBytes, 0, destArray, startIndex, 4);
            }
            else
            {
                byte[] flipBytes = BitConverter.GetBytes(value);
                Array.Copy(flipBytes, 0, destArray, startIndex, 4);
            }
        }

        /// <summary>
        /// Reads the entire index file in order to get a breakdown of how shapes are broken up.
        /// </summary>
        /// <param name="fileName">A string fileName of the .shx file to read.</param>
        /// <returns>A List of ShapeHeaders that give offsets and lengths so that reading can be optimized</returns>
        public List<ShapeHeader> ReadIndexFile(string fileName)
        {
            string shxFilename = fileName;
            string ext = Path.GetExtension(fileName);

            if (ext != ".shx")
            {
                shxFilename = Path.ChangeExtension(fileName, ".shx");
            }

            if (shxFilename == null)
            {
                throw new NullReferenceException(DataStrings.ArgumentNull_S.Replace("%S", fileName));
            }

            if (!File.Exists(shxFilename))
            {
                throw new FileNotFoundException(DataStrings.FileNotFound_S.Replace("%S", shxFilename));
            }

            var fileLen = new FileInfo(shxFilename).Length;
            if (fileLen == 100)
            {
                // the file is empty so we are done reading
                return Enumerable.Empty<ShapeHeader>().ToList();
            }

            // Use a the length of the file to dimension the byte array
            using (var fs = new FileStream(shxFilename, FileMode.Open, FileAccess.Read, FileShare.Read, 65536))
            {
                // Skip the header and begin reading from the first record
                fs.Seek(100, SeekOrigin.Begin);

                Header.ShxLength = (int)(fileLen / 2);
                var length = (int)(fileLen - 100);
                var numRecords = length / 8;

                // Each record consists of 2 Big-endian integers for a total of 8 bytes.
                // This will store the header elements that we read from the file.
                var result = new List<ShapeHeader>(numRecords);
                for (var i = 0; i < numRecords; i++)
                {
                    result.Add(new ShapeHeader { Offset = fs.ReadInt32(Endian.BigEndian), ContentLength = fs.ReadInt32(Endian.BigEndian) });
                }

                return result;
            }
        }

        /// <summary>
        /// Ensures that the attribute table will have information that matches the current table of attribute information.
        /// </summary>
        public void UpdateAttributes()
        {
            if (Attributes == null) return;

            string newFile = Path.ChangeExtension(Filename, "dbf");
            if (!AttributesPopulated && File.Exists(Attributes.Filename))
            {
                if (!newFile.Equals(Attributes.Filename))
                {
                    // only switch to the new file, if we aren't using it already
                    if (File.Exists(newFile)) File.Delete(newFile);
                    File.Copy(Attributes.Filename, newFile);
                    Attributes.Filename = newFile;
                }

                return;
            }

            InitAttributeTable();
            Attributes.SaveAs(newFile, true);
        }

        /// <summary>
        /// Exports the dbf file content to a stream.
        /// </summary>
        /// <returns>A stream that contains the dbf file content.</returns>
        protected Stream ExportDbfToStream()
        {
            InitAttributeTable();
            return Attributes?.ExportDbfToStream();
        }

        /// <summary>
        /// Creates the Attributes Table if it does not already exist.
        /// </summary>
        private void InitAttributeTable()
        {
            // The Attributes either don't exist or have been loaded and will now replace the ones in the file.
            if (Attributes == null || Attributes.Table != null && Attributes.Table.Columns.Count > 0) return;

            // Only add an FID field if there are no attributes at all.
            DataTable newTable = new DataTable();
            newTable.Columns.Add("FID");

            // Added by JamesP@esdm.co.uk - Index mode has no attributes and no features - so Features.count is Null and so was not adding any rows and failing
            int numRows = IndexMode ? ShapeIndices.Count : Features.Count;
            for (int i = 0; i < numRows; i++)
            {
                DataRow dr = newTable.NewRow();
                dr["FID"] = i;
                newTable.Rows.Add(dr);
            }

            Attributes.Table = newTable;
        }

        /// <summary>
        /// This uses the fileName of this shapefile to read the prj file of the same name
        /// and stores the result in the Projection class.
        /// </summary>
        public void ReadProjection()
        {
            var prjFile = Path.ChangeExtension(Filename, ".prj");
            Projection = File.Exists(prjFile) ? ProjectionInfo.Open(prjFile) : new ProjectionInfo();
        }

        /// <summary>
        /// Automatically uses the fileName of this shapefile to save the projection
        /// </summary>
        public void SaveProjection()
        {
            var prjFile = Path.ChangeExtension(Filename, ".prj");
            if (File.Exists(prjFile))
                File.Delete(prjFile);
            Projection?.SaveAs(prjFile);
        }

        /// <summary>
        /// Reads just the content requested in order to satisfy the paging ability of VirtualMode for the DataGridView
        /// </summary>
        /// <param name="startIndex">The integer lower page boundary</param>
        /// <param name="numRows">The integer number of attribute rows to return for the page</param>
        /// <param name="fieldNames">The list or array of fieldnames to return.</param>
        /// <returns>A DataTable populated with data rows with only the specified values.</returns>
        public override DataTable GetAttributes(int startIndex, int numRows, IEnumerable<string> fieldNames)
        {
            if (AttributesPopulated) return base.GetAttributes(startIndex, numRows, fieldNames);
            DataTable result = new DataTable();
            DataColumn[] columns = GetColumns();

            // Always add FID in this paging scenario.
            result.Columns.Add("FID", typeof(int));

            var fields = fieldNames as IList<string> ?? fieldNames.ToList();
            foreach (string name in fields)
            {
                foreach (var col in columns)
                {
                    if (string.Equals(col.ColumnName, name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        result.Columns.Add(col);
                        break;
                    }
                }
            }

            for (int i = 0; i < numRows; i++)
            {
                DataRow dr = result.NewRow();
                dr["FID"] = startIndex + i;
                result.Rows.Add(dr);
            }

            // Most use cases with an expression use only one or two fieldnames.  Tailor for better
            // performance in that case, at the cost of performance in the "read all " case.
            // The other overload without fieldnames specified is designed for that case.
            foreach (string field in fields)
            {
                if (field == "FID") continue;
                object[] values = _attributeTable.SupplyPageOfData(startIndex, numRows, field);
                for (int i = 0; i < numRows; i++)
                {
                    result.Rows[i][field] = values[i];
                }
            }

            return result;
        }

        /// <summary>
        /// The number of rows
        /// </summary>
        /// <returns></returns>
        public override int NumRows()
        {
            // overridden in sub-classes
            return _attributeTable.NumRecords;
        }

        /// <summary>
        /// This gets a copy of the actual internal list of columns.
        /// This should never be used to make changes to the column collection.
        /// </summary>
        public override DataColumn[] GetColumns()
        {
            return _attributeTable.Columns
                .Select(_ => (DataColumn)new Field(_.ColumnName, _.TypeCharacter, _.Length, _.DecimalCount))
                .ToArray();
        }

        /// <summary>
        /// Checks whether the shapefile can be saved with the given fileName.
        /// </summary>
        /// <param name="fileName">File name to save.</param>
        /// <param name="overwrite">Indicates whether the file may be overwritten.</param>
        protected void EnsureValidFileToSave(string fileName, bool overwrite)
        {
            string dir = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            else if (File.Exists(fileName))
            {
                if (fileName != Filename && !overwrite) throw new ArgumentOutOfRangeException(string.Format(DataStrings.FileExistsOverwritingNotAllowed, fileName));
                File.Delete(fileName);
                var shx = Path.ChangeExtension(fileName, ".shx");
                if (File.Exists(shx)) File.Delete(shx);
            }
        }

        /// <summary>
        /// Updates the Header with the shape type that corresponds to this shapefiles CoordinateType.
        /// </summary>
        protected virtual void SetHeaderShapeType()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the file to a new location.
        /// </summary>
        /// <param name="fileName">The fileName to save.</param>
        /// <param name="overwrite">Boolean that specifies whether or not to overwrite the existing file.</param>
        public override void SaveAs(string fileName, bool overwrite)
        {
            EnsureValidFileToSave(fileName, overwrite);
            Filename = fileName;
            HeaderSaveAs(fileName);

            StreamLengthPair streamLengthPair;
            using (var shpStream = new FileStream(Filename, FileMode.Append, FileAccess.Write, FileShare.None, 10000000))
            using (var shxStream = new FileStream(Header.ShxFilename, FileMode.Append, FileAccess.Write, FileShare.None, 10000000))
            {
                streamLengthPair = PopulateShpAndShxStreams(shpStream, shxStream, IndexMode);
            }

            WriteFileLength(Filename, streamLengthPair.ShpLength);
            WriteFileLength(Header.ShxFilename, streamLengthPair.ShxLength);
            UpdateAttributes();
            SaveProjection();
        }

        /// <summary>
        /// Saves the header to a new location.
        /// </summary>
        /// <param name="fileName">File to save.</param>
        protected void HeaderSaveAs(string fileName)
        {
            UpdateHeader();
            Header.SaveAs(fileName);
        }

        /// <summary>
        /// Updates the Header.
        /// </summary>
        protected void UpdateHeader()
        {
            SetHeaderShapeType();
            InvalidateEnvelope();
            Header.SetExtent(Extent);
            Header.ShxLength = IndexMode ? ShapeIndices.Count * 4 + 50 : Features.Count * 4 + 50;
        }

        /// <summary>
        /// Populates the given streams for the shp and shx file.
        /// </summary>
        /// <param name="shpStream">Stream that is used to write the shp file.</param>
        /// <param name="shxStream">Stream that is used to write the shx file.</param>
        /// <param name="indexed">Indicates whether the streams are populated in IndexMode.</param>
        /// <returns>The lengths of the streams in bytes.</returns>
        protected virtual StreamLengthPair PopulateShpAndShxStreams(Stream shpStream, Stream shxStream, bool indexed)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ShapefilePackage ExportShapefilePackage()
        {
            UpdateHeader();

            ShapefilePackage package = new ShapefilePackage
            {
                DbfFile = ExportDbfToStream(),
                ShpFile = Header.ExportShpHeaderToStream(),
                ShxFile = Header.ExportShxHeaderToStream()
            };

            StreamLengthPair streamLengthPair = PopulateShpAndShxStreams(package.ShpFile, package.ShxFile, IndexMode);

            // write file length 
            WriteFileLength(package.ShpFile, streamLengthPair.ShpLength);
            WriteFileLength(package.ShxFile, streamLengthPair.ShxLength);

            package.ShpFile.Seek(0, SeekOrigin.Begin);
            package.ShxFile.Seek(0, SeekOrigin.Begin);

            if (Projection != null)
            {
                package.PrjFile = new MemoryStream();
                StreamWriter projWriter = new StreamWriter(package.PrjFile);
                projWriter.WriteLine(Projection.ToEsriString());
                projWriter.Flush();
                package.PrjFile.Seek(0, SeekOrigin.Begin);
            }

            return package;
        }

        /// <summary>
        /// StreamLengthPair is used to get the length of the shp and shx file from PopulateShpAndShxStreams and update the file length inside the corresponding stream.
        /// </summary>
        protected internal class StreamLengthPair
        {
            internal int ShpLength { get; set; }
            internal int ShxLength { get; set; }
        }
    }
}