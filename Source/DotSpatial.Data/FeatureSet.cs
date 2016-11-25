// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using DotSpatial.NTSExtension;
using DotSpatial.Projections;
using DotSpatial.Serialization;
using GeoAPI.Geometries;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// Rather than being an abstract base class, this is a "router" class that will
    /// make decisions based on the file extension or whatever being used and decide the best
    /// IFeatureSet that matches the specifications.
    /// </summary>
    public class FeatureSet : DataSet, IFeatureSet
    {
        #region Events

        /// <summary>
        /// Occurs when a new feature is added to the list
        /// </summary>
        public event EventHandler<FeatureEventArgs> FeatureAdded;

        /// <summary>
        /// Occurs when a feature is removed from the list.
        /// </summary>
        public event EventHandler<FeatureEventArgs> FeatureRemoved;

        /// <summary>
        /// Occurs when the vertices are invalidated, encouraging a re-draw
        /// </summary>
        public event EventHandler VerticesInvalidated;

        #endregion

        #region Variables

        /// <summary>
        /// The _data table.
        /// </summary>
        private DataTable _dataTable;

        /// <summary>
        /// The _feature lookup.
        /// </summary>
        private readonly Dictionary<DataRow, IFeature> _featureLookup;

        /// <summary>
        /// The _features.
        /// </summary>
        private IFeatureList _features;

        /// <summary>
        /// The _m.
        /// </summary>
        private double[] _m;

        /// <summary>
        /// The _shape indices.
        /// </summary>
        private List<ShapeRange> _shapeIndices;

        /// <summary>
        /// The _vertices.
        /// </summary>
        private double[] _vertices;

        /// <summary>
        /// The _vertices are valid.
        /// </summary>
        private bool _verticesAreValid;

        /// <summary>
        /// The _z.
        /// </summary>
        private double[] _z;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSet"/> class.
        /// This doesn't do anything exactly because there is no file-specific information yet
        /// </summary>
        public FeatureSet()
        {
            IndexMode = false; // this is false unless we are loading it from a specific shapefile case.
            _featureLookup = new Dictionary<DataRow, IFeature>();
            _features = new FeatureList(this);
            _features.FeatureAdded += FeaturesFeatureAdded;
            _features.FeatureRemoved += FeaturesFeatureRemoved;
            _dataTable = new DataTable();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSet"/> class.
        /// Creates a new FeatureSet
        /// </summary>
        /// <param name="featureType">
        /// The Feature type like point, line or polygon
        /// </param>
        public FeatureSet(FeatureType featureType)
            : this()
        {
            FeatureType = featureType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSet"/> class.
        /// This creates a new featureset by checking each row of the table.  If the WKB feature
        /// type matches the specified featureTypes, then it will copy that.
        /// </summary>
        /// <param name="wkbTable">
        /// </param>
        /// <param name="wkbColumnIndex">
        /// </param>
        /// <param name="indexed">
        /// </param>
        /// <param name="type">
        /// </param>
        public FeatureSet(DataTable wkbTable, int wkbColumnIndex, bool indexed, FeatureType type)
            : this()
        {
            if (IndexMode)
            {
                // Assume this DataTable has WKB in column[0] and the rest of the columns are attributes.
                FeatureSetPack result = new FeatureSetPack();
                foreach (DataRow row in wkbTable.Rows)
                {
                    byte[] data = (byte[])row[0];
                    MemoryStream ms = new MemoryStream(data);
                    WKBFeatureReader.ReadFeature(ms, result);
                }

                // convert lists of arrays into a single vertex array for each shape type.
                result.StopEditing();

                // Make sure all the same columns exist in the same order
                result.Polygons.CopyTableSchema(wkbTable);

                // Assume that all the features happened to be polygons
                foreach (DataRow row in wkbTable.Rows)
                {
                    // Create a new row
                    DataRow dest = result.Polygons.DataTable.NewRow();
                    dest.ItemArray = row.ItemArray;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSet"/> class.
        /// Creates a new FeatureSet using a given list of IFeatures.
        /// This will copy the existing features, rather than removing
        /// them from their parent feature set.
        /// </summary>
        /// <param name="inFeatures">
        /// The list of IFeatures
        /// </param>
        public FeatureSet(IList<IFeature> inFeatures)
            : this()
        {
            _dataTable.RowDeleted += DataTableRowDeleted;

            if (inFeatures.Count > 0)
            {
                FeatureType = inFeatures[0].FeatureType;
            }

            _features = new FeatureList(this);
            if (inFeatures.Count > 0)
            {
                if (inFeatures[0].ParentFeatureSet != null)
                {
                    CopyTableSchema(inFeatures[0].ParentFeatureSet);
                }
                else
                {
                    if (inFeatures[0].DataRow != null)
                    {
                        CopyTableSchema(inFeatures[0].DataRow.Table);
                    }
                }

                _features.SuspendEvents();
                foreach (IFeature f in inFeatures)
                {
                    IFeature myFeature = f.Copy();
                    _features.Add(myFeature);
                    if (myFeature.DataRow != null)
                        _featureLookup.Add(myFeature.DataRow, myFeature);
                }

                _features.ResumeEvents();
            }
        }

        /// <summary>
        /// Generates a new feature, adds it to the features and returns the value.
        /// </summary>
        /// <param name="geometry">
        /// The geometry.
        /// </param>
        /// <returns>
        /// The feature that was added to this featureset
        /// </returns>
        public IFeature AddFeature(IGeometry geometry)
        {
            IFeature f = new Feature(geometry, this);
            return f;
        }

        /// <summary>
        /// The data table row deleted.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DataTableRowDeleted(object sender, DataRowChangeEventArgs e)
        {
            Features.Remove(_featureLookup[e.Row]);
            _featureLookup.Remove(e.Row);
        }

        /// <summary>
        /// The features feature added.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void FeaturesFeatureAdded(object sender, FeatureEventArgs e)
        {
            _verticesAreValid = false; // invalidate vertices
            if (e.Feature.DataRow == null)
            {
                return;
            }

            _featureLookup[e.Feature.DataRow] = e.Feature;
            if (FeatureAdded != null)
            {
                FeatureAdded(sender, e);
            }
        }

        /// <summary>
        /// The features feature removed.
        /// </summary>
        /// <param name="sender">
        /// The object sender.
        /// </param>
        /// <param name="e">
        /// The FeatureEventArgs.
        /// </param>
        private void FeaturesFeatureRemoved(object sender, FeatureEventArgs e)
        {
            _verticesAreValid = false;
            ShapeIndices.Remove(e.Feature.ShapeIndex);
            _featureLookup.Remove(e.Feature.DataRow);
            if (FeatureRemoved != null)
            {
                FeatureRemoved(sender, e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the FID values as a field called FID, but only if the FID field
        /// does not already exist.
        /// </summary>
        public void AddFid()
        {
            DataTable dt = DataTable;
            if (dt.Columns.Contains("FID"))
            {
                return;
            }

            dt.Columns.Add("FID", typeof(int));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["FID"] = i;
            }
        }

        /// <inheritdoc/>
        public void AddShape(Shape shape)
        {
            // This first section controls the indices which need to happen regardless
            // because drawing uses indices even if editors like working with features.
            int count = (_vertices != null) ? _vertices.Length / 2 : 0; // Original number of points
            int totalCount = shape.Range.NumPoints + count;
            int start = shape.Range.StartIndex;
            int num = shape.Range.NumPoints;

            double[] vertices = new double[totalCount * 2];
            if (_vertices != null)
            {
                Array.Copy(_vertices, 0, vertices, 0, _vertices.Length);
            }

            if (shape.Vertices != null)
            {
                Array.Copy(shape.Vertices, start * 2, vertices, count * 2, num * 2);
                if (_m != null || shape.M != null)
                {
                    double[] m = new double[totalCount];
                    if (_m != null)
                    {
                        Array.Copy(_m, 0, m, 0, _m.Length);
                    }

                    if (shape.M != null)
                    {
                        Array.Copy(shape.Vertices, start, m, count, num);
                    }

                    _m = m;
                }

                if (_z != null || shape.Z != null)
                {
                    double[] z = new double[totalCount];
                    if (_z != null)
                    {
                        Array.Copy(_z, 0, z, 0, _z.Length);
                    }

                    if (shape.Z != null)
                    {
                        Array.Copy(shape.Vertices, start, z, count, num);
                    }

                    _z = z;
                }
            }

            shape.Range.StartIndex = count;
            ShapeIndices.Add(shape.Range);
            Vertex = vertices;
            if (Extent == null)
            {
                Extent = new Extent();
            }

            Extent.ExpandToInclude(shape.Range.Extent);

            if (!IndexMode)
            {
                // Just add a new feature
                IFeature addedFeature = new Feature(shape);

                Features.Add(addedFeature);
                addedFeature.DataRow = AddAttributes(shape);

                if (_features.EventsSuspended && addedFeature.DataRow != null) // Changed by jany_: If feature gets added while _features events are suspended _features and _featureLookup get out of sync
                    _featureLookup[addedFeature.DataRow] = addedFeature;

                if (!shape.Range.Extent.IsEmpty())
                {
                    Extent.ExpandToInclude(addedFeature.Geometry.EnvelopeInternal.ToExtent());
                }
            }
        }

        /// <inheritdoc/>
        public void AddShapes(IEnumerable<Shape> shapes)
        {
            IEnumerable<Shape> enumShapes = shapes as IList<Shape> ?? shapes.ToList();

            if (!IndexMode)
            {
                _features.SuspendEvents();
                foreach (Shape shape in enumShapes)
                {
                    AddShape(shape);
                }

                _features.ResumeEvents();
                return;
            }

            bool vertsExist = _vertices != null;
            int count = vertsExist ? _vertices.Length / 2 : 0;
            int numPoints = count;
            bool hasZ = _z != null;
            bool hasM = _m != null;

            foreach (Shape shape in enumShapes)
            {
                numPoints += shape.Range.NumPoints;
                if (shape.M != null)
                {
                    hasM = true;
                }

                if (shape.Z != null)
                {
                    hasZ = true;
                }
            }

            double[] vertices = new double[numPoints * 2];
            if (vertsExist)
            {
                Array.Copy(_vertices, 0, vertices, 0, _vertices.Length);
            }

            if (hasM)
            {
                double[] m = new double[numPoints];
                if (_m != null)
                {
                    Array.Copy(_m, 0, m, 0, _m.Length);
                }

                _m = m;
            }

            if (hasZ)
            {
                double[] z = new double[numPoints];
                if (_z != null)
                {
                    Array.Copy(_z, 0, z, 0, _z.Length);
                }

                _z = z;
            }

            int offset = count;
            foreach (Shape shape in enumShapes)
            {
                int start = shape.Range.StartIndex;
                int num = shape.Range.NumPoints;
                if (shape.Vertices != null)
                {
                    if (shape.Vertices.Length - start >= num)
                    {
                        Array.Copy(shape.Vertices, start * 2, vertices, offset * 2, num * 2);
                    }
                }

                if (shape.M != null && _m != null && (shape.M.Length - start) >= num)
                {
                    Array.Copy(shape.M, start, _m, offset, num);
                }

                if (shape.Z != null && _z != null && (shape.Z.Length - start) >= num)
                {
                    Array.Copy(shape.Z, start, _z, offset, num);
                }

                ShapeRange newRange = CloneableEM.Copy(shape.Range);
                newRange.StartIndex = offset;
                offset += num;
                ShapeIndices.Add(newRange);
            }

            Vertex = vertices;
            UpdateExtent();
        }

        /// <inheritdoc/>
        IFeatureSet IFeatureSet.CopyFeatures(bool withAttributes)
        {
            return CopySubset("", withAttributes);
        }

        /// <inheritdoc/>
        IFeatureSet IFeatureSet.CopySubset(List<int> indices)
        {
            return CopySubset(indices, true);
        }

        /// <inheritdoc/>
        IFeatureSet IFeatureSet.CopySubset(List<int> indices, bool withAttributes)
        {
            return CopySubset(indices, withAttributes);
        }

        /// <inheritdoc/>
        IFeatureSet IFeatureSet.CopySubset(string filterExpression)
        {
            return CopySubset(filterExpression, true);
        }

        /// <inheritdoc/>
        IFeatureSet IFeatureSet.CopySubset(string filterExpression, bool withAttributes)
        {
            return CopySubset(filterExpression, withAttributes);
        }

        /// <inheritdoc/>
        public void CopyTableSchema(IFeatureSet source)
        {
            DataTable.Columns.Clear();
            DataTable dt = source.DataTable;
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc != null)
                {
                    DataColumn outCol = new DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping);
                    Field fld = new Field(outCol);
                    DataTable.Columns.Add(fld);
                }
            }
        }

        /// <inheritdoc/>
        public void CopyTableSchema(DataTable sourceTable)
        {
            DataTable.Columns.Clear();
            foreach (DataColumn dc in sourceTable.Columns)
            {
                if (dc != null)
                {
                    DataColumn outCol = new DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping);
                    Field fld = new Field(outCol);
                    DataTable.Columns.Add(fld);
                }
            }
        }

        /// <inheritdoc/>
        public IFeature FeatureFromRow(DataRow row)
        {
            return _featureLookup[row];
        }

        /// <inheritdoc/>
        public virtual void FillAttributes()
        {
            FillAttributes(ProgressHandler);
        }

        /// <inheritdoc/>
        public virtual void FillAttributes(IProgressHandler progressHandler)
        {
        }

        /// <inheritdoc/>
        public virtual IFeature GetFeature(int index)
        {
            if (IndexMode == false)
            {
                return Features[index];
            }

            if (FeatureType == FeatureType.Point)
            {
                return GetPoint(index);
            }

            if (FeatureType == FeatureType.Line)
            {
                return GetLine(index);
            }

            if (FeatureType == FeatureType.Polygon)
            {
                return GetPolygon(index);
            }

            if (FeatureType == FeatureType.MultiPoint)
            {
                return GetMultiPoint(index);
            }

            return null;
        }

        /// <inheritdoc/>
        public virtual Shape GetShape(int index, bool getAttributes)
        {
            if (!IndexMode)
            {
                IFeature f = Features[index];
                Shape shp = new Shape(f.Geometry, f.FeatureType);
                if (getAttributes)
                    shp.Attributes = f.DataRow.ItemArray;
                return shp;
            }

            Shape result = new Shape(FeatureType);

            // This will also deep copy the parts, attributes and vertices
            ShapeRange range = ShapeIndices[index];
            result.Range = CloneableEM.Copy(range);
            int start = range.StartIndex;
            int numPoints = range.NumPoints;

            if (_z != null && (_z.Length - start) >= numPoints)
            {
                result.Z = new double[numPoints];
                Array.Copy(_z, start, result.Z, 0, numPoints);
            }

            if (_m != null && (_m.Length - start) >= numPoints)
            {
                result.M = new double[numPoints];
                Array.Copy(_m, start, result.M, 0, numPoints);
            }

            double[] vertices = new double[numPoints * 2];
            Array.Copy(_vertices, start * 2, vertices, 0, numPoints * 2);
            result.Vertices = vertices;

            // There is presumed to be only a single shape in the output array.
            result.Range.StartIndex = 0;
            if (getAttributes)
            {
                if (AttributesPopulated)
                {
                    result.Attributes = DataTable.Rows[index].ItemArray;
                }
                else
                {
                    DataTable dt = GetAttributes(index, 1);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        result.Attributes = dt.Rows[0].ItemArray;
                    }
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public void InitializeVertices()
        {
            OnInitializeVertices();
        }

        /// <inheritdoc/>
        public void InvalidateVertices()
        {
            _verticesAreValid = false;
            OnVerticesInvalidated();
        }

        /// <inheritdoc/>
        public IFeatureSet Join(DataTable table, string localJoinField, string dataTableJoinField)
        {
            if (!table.Columns.Contains(dataTableJoinField))
            {
                throw new Exception("The foreign join field specified not found.");
            }

            var res = Open(Filename);
            if (!res.AttributesPopulated)
            {
                FillAttributes();
            }

            if (!res.DataTable.Columns.Contains(localJoinField))
            {
                throw new Exception("The local join field specified is not in this table.");
            }

            var copyColumns = new List<DataColumn>();
            foreach (DataColumn column in table.Columns)
            {
                if (res.DataTable.Columns.Contains(column.ColumnName))
                {
                    continue;
                }

                copyColumns.Add(column);
                res.DataTable.Columns.Add(new DataColumn(column.ColumnName, column.DataType, column.Expression, column.ColumnMapping));
            }

            foreach (DataRow row in res.DataTable.Rows)
            {
                string query;
                if (table.Columns[dataTableJoinField].DataType == typeof(string))
                {
                    // Replcase quote with double quote, if need
                    var localJoinStr = (string)row[localJoinField];
                    if (localJoinStr != null)
                    {
                        localJoinStr = localJoinStr.Replace("'", "''");
                    }
                    query = "[" + dataTableJoinField + "] = '" + localJoinStr + "'";
                }
                else
                {
                    query = "[" + dataTableJoinField + "] = " + row[localJoinField];
                }

                var result = table.Select(query);

                if (result.Length == 0)
                {
                    continue;
                }

                foreach (var column in copyColumns)
                {
                    row[column.ColumnName] = result[0][column.ColumnName];
                }
            }

            return res;
        }

        /// <inheritdoc/>
        public IFeatureSet Join(string xlsFilePath, string localJoinField, string xlsJoinField)
        {
            // This connection string will not likely work on 64 bit machines.
            var con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + xlsFilePath + "; Extended Properties=Excel 8.0");
            var da = new OleDbDataAdapter("select * from [Data$]", con);
            var dt = new DataTable();
            da.Fill(dt);

            return Join(dt, localJoinField, xlsJoinField);
        }

        /// <inheritdoc/>
        public void Save()
        {
            if (!AttributesPopulated) FillAttributes();
            SaveAs(Filename, true);
        }

        /// <inheritdoc/>
        public virtual void SaveAs(string fileName, bool overwrite)
        {
            if (FeatureType == FeatureType.Unspecified)
            {
                if (IndexMode)
                {
                    if (ShapeIndices.Count > 0)
                    {
                        FeatureType = ShapeIndices[0].FeatureType;
                    }
                }
                else if (Features.Count > 0)
                {
                    FeatureType = Features[0].FeatureType;
                }
            }

            IFeatureSet result = DataManager.DefaultDataManager.CreateVector(fileName, FeatureType, ProgressHandler);

            // Previously I prevented setting of the features list, but I think we can negate that decision.
            // No reason to prevent it that I can see.
            // Same for the ShapeIndices. I'd like to simply set them here and prevent the hassle of a
            // pure copy process.
            result.Vertex = Vertex;
            result.ShapeIndices = ShapeIndices;
            result.Extent = Extent;
            result.IndexMode = IndexMode; // added by JamesP@esdm.co.uk as this was not being passed into result
            result.CoordinateType = CoordinateType;
            if (!IndexMode)
            {
                result.Features = Features;
            }

            if (AttributesPopulated)
            {
                result.DataTable = DataTable;

                //added by Jiri Kadlec to prevent overwriting the DataTable in the Save() function
                result.AttributesPopulated = true;
            }

            result.ProgressHandler = ProgressHandler;
            result.Projection = Projection;
            result.Save();
            Filename = result.Filename;
        }

        /// <inheritdoc/>
        public virtual List<IFeature> Select(Extent region)
        {
            Extent ignoreMe;
            return Select(region, out ignoreMe);
        }

        /// <inheritdoc/>
        public virtual List<IFeature> Select(Extent region, out Extent affectedRegion)
        {
            var result = new List<IFeature>();
            affectedRegion = new Extent();

            if (IndexMode)
            {
                var aoi = new ShapeRange(region);
                var shapes = ShapeIndices;
                for (var shp = 0; shp < shapes.Count; shp++)
                {
                    if (!shapes[shp].Intersects(aoi)) continue;

                    var feature = GetFeature(shp);
                    affectedRegion.ExpandToInclude(feature.Geometry.EnvelopeInternal.ToExtent());
                    result.Add(feature);
                }
            }
            else
            {
                foreach (var feature in Features)
                {
                    if (!feature.Geometry.Intersects(region.ToEnvelope().ToPolygon()))
                        continue;

                    result.Add(feature);
                    affectedRegion.ExpandToInclude(feature.Geometry.EnvelopeInternal.ToExtent());
                }
            }

            return result;
        }

        /// <summary>
        /// Selects using a string filter expression to obtain the desired features.
        /// Field names should be in square brackets.  Alternately, if the field name of [FID]
        /// is used, then it will use the row index instead if no FID field is found.
        /// </summary>
        /// <param name="filterExpression">
        /// The features to return.
        /// </param>
        /// <returns>
        /// The list of desired features.
        /// </returns>
        public virtual List<IFeature> SelectByAttribute(string filterExpression)
        {
            DataTable dt = DataTable;
            bool isTemp = false;
            if (filterExpression != null)
            {
                if (filterExpression.Contains("[FID]") && DataTable.Columns.Contains("FID") == false)
                {
                    AddFid();
                    isTemp = true;
                }
            }

            DataRow[] rows = dt.Select(filterExpression);
            var result = rows.Select(dr => FeatureLookup[dr]).ToList();
            if (isTemp)
            {
                dt.Columns.Remove("FID");
            }

            return result;
        }

        /// <summary>
        /// This version is more tightly integrated to the DataTable and returns the row indices, rather
        /// than attempting to link the results to features themselves, which may not even exist.
        /// </summary>
        /// <param name="filterExpression">
        /// The filter expression
        /// </param>
        /// <returns>
        /// The list of indices
        /// </returns>
        public virtual List<int> SelectIndexByAttribute(string filterExpression)
        {
            var result = new List<int>();
            if (AttributesPopulated && DataTable != null)
            {
                var rows = DataTable.Select(filterExpression);
                result.AddRange(rows.Select(row => DataTable.Rows.IndexOf(row)));
            }
            else
            {
                const int rowsPerPage = 10000;
                var numPages = (int)Math.Ceiling((double)NumRows() / rowsPerPage);
                for (int page = 0; page < numPages; page++)
                {
                    var table = GetAttributes(page * rowsPerPage, rowsPerPage);
                    foreach (var row in table.Select(filterExpression))
                    {
                        result.Add(table.Rows.IndexOf(row) + page * rowsPerPage);
                    }
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public virtual List<int> SelectIndices(Extent region)
        {
            List<int> result = new List<int>();
            List<ShapeRange> shapes = ShapeIndices; // get from internal dataset if necessary
            if (shapes != null)
            {
                for (int shp = 0; shp < shapes.Count; shp++)
                {
                    if (shapes[shp].Extent.Intersects(region))
                    {
                        result.Add(shp);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves a subset using exclusively the features matching the specified values.
        /// </summary>
        /// <param name="indices">
        /// An integer list of indices to copy into the new FeatureSet.
        /// </param>
        /// <returns>
        /// A FeatureSet with the new items.
        /// </returns>
        public FeatureSet CopySubset(List<int> indices)
        {
            return CopySubset(indices, true);
        }

        /// <summary>
        /// Retrieves a subset using exclusively the features matching the specified values.
        /// </summary>
        /// <param name="indices">
        /// An integer list of indices to copy into the new FeatureSet.
        /// </param>
        /// <param name="withAttributes"></param>
        /// <returns>
        /// A FeatureSet with the new items.
        /// </returns>
        private FeatureSet CopySubset(List<int> indices, bool withAttributes)
        {
            List<IFeature> f = new List<IFeature>();
            foreach (int row in indices)
            {
                if (withAttributes)
                    f.Add(GetFeature(row));
                else
                {
                    Shape shp = GetShape(row, false);
                    f.Add(new Feature(shp.ToGeometry()));
                }
            }
            FeatureSet copy = new FeatureSet(f);
            copy.Projection = CloneableEM.Copy(Projection);
            copy.InvalidateEnvelope(); // the new set will likely have a different envelope bounds
            return copy;
        }

        /// <inheritdoc/>
        public void InvalidateEnvelope()
        {
            MyExtent = null;
        }

        /// <summary>
        /// This attempts to open the specified file as a valid IFeatureSet.  This will require that
        /// the default data manager can work with the file format at runtime.
        /// </summary>
        /// <param name="fileName">
        /// The string fileName for this featureset.
        /// </param>
        public static IFeatureSet Open(string fileName)
        {
            return DataManager.DefaultDataManager.OpenVector(fileName, true, DataManager.DefaultDataManager.ProgressHandler);
        }

        /// <summary>
        /// This will return the correct feature type by reading the fileName.
        /// </summary>
        /// <param name="fileName">
        /// A string specifying the file with the extension .shp to open.
        /// </param>
        /// <returns>
        /// A correct featureSet which is exclusively for reading the .shp data
        /// </returns>
        public static IFeatureSet OpenFile(string fileName)
        {
            return DataManager.DefaultDataManager.OpenFile(fileName) as IFeatureSet;
        }

        /// <summary>
        /// Generates a new FeatureSet, if possible, from the specified fileName.
        /// </summary>
        /// <param name="fileName">
        /// The string fileName to attempt to load into a new FeatureSet.
        /// </param>
        /// <param name="progressHandler">
        /// An IProgressHandler for progress messages
        /// </param>
        public static IFeatureSet OpenFile(string fileName, IProgressHandler progressHandler)
        {
            return DataManager.DefaultDataManager.OpenFile(fileName, true, progressHandler) as IFeatureSet;
        }

        /// <summary>
        /// Gets the line for the specified index
        /// </summary>
        protected IFeature GetLine(int index)
        {
            ShapeRange shape = ShapeIndices[index];
            List<ILineString> lines = new List<ILineString>();
            foreach (PartRange part in shape.Parts)
            {
                int i = part.StartIndex;
                List<Coordinate> coords = new List<Coordinate>();
                foreach (Vertex d in part)
                {
                    Coordinate c = new Coordinate(d.X, d.Y);
                    coords.Add(c);
                    if (M != null && M.Length > 0)
                    {
                        c.M = M[i];
                    }

                    if (Z != null && Z.Length > 0)
                    {
                        c.Z = Z[i];
                    }

                    i++;
                }

                lines.Add(new LineString(coords.ToArray()));
            }

            IGeometry geom;
            if (FeatureGeometryFactory == null)
            {
                FeatureGeometryFactory = GeometryFactory.Default;
            }

            if (shape.Parts.Count > 1)
            {
                geom = FeatureGeometryFactory.CreateMultiLineString(lines.ToArray());
            }
            else if (shape.Parts.Count == 1)
            {
                geom = FeatureGeometryFactory.CreateLineString(lines[0].Coordinates);
            }
            else
            {
                geom = FeatureGeometryFactory.CreateMultiLineString(new ILineString[] { });
            }

            var f = new Feature(geom)
                            {
                                ParentFeatureSet = this,
                                ShapeIndex = shape,
                            };

            // Attribute reading is only handled in the overridden case.
            return f;
        }

        /// <summary>
        /// Returns a single multipoint feature for the shape at the specified index
        /// </summary>
        protected IFeature GetMultiPoint(int index)
        {
            ShapeRange shape = ShapeIndices[index];
            List<Coordinate> coords = new List<Coordinate>();
            foreach (PartRange part in shape.Parts)
            {
                int i = part.StartIndex;
                foreach (Vertex vertex in part)
                {
                    Coordinate c = new Coordinate(vertex.X, vertex.Y);
                    coords.Add(c);
                    if (M != null && M.Length != 0)
                    {
                        c.M = M[i];
                    }

                    if (Z != null && Z.Length != 0)
                    {
                        c.Z = Z[i];
                    }

                    i++;
                }
            }

            if (FeatureGeometryFactory == null)
            {
                FeatureGeometryFactory = GeometryFactory.Default;
            }

            var mp = FeatureGeometryFactory.CreateMultiPoint(coords.ToArray());
            var f = new Feature(mp)
            {
                ParentFeatureSet = this,
                ShapeIndex = shape,
            };

            // Attribute reading is only handled in the overridden case.
            return f;
        }

        /// <summary>
        /// Gets the point for the shape at the specified index
        /// </summary>
        protected IFeature GetPoint(int index)
        {
            ShapeRange shape = ShapeIndices[index];
            Coordinate c = new Coordinate(Vertex[index * 2], Vertex[index * 2 + 1]);
            if (M != null && M.Length != 0)
            {
                c.M = M[index];
            }

            if (Z != null && Z.Length != 0)
            {
                c.Z = Z[index];
            }

            if (FeatureGeometryFactory == null)
                FeatureGeometryFactory = GeometryFactory.Default;

            IPoint p = FeatureGeometryFactory.CreatePoint(c);
            var f = new Feature(p)
            {
                ParentFeatureSet = this,
                ShapeIndex = shape,
            };

            // Attributes only retrieved in the overridden case
            return f;
        }

        /// <summary>
        /// If the FeatureType is polygon, this is the code for converting the vertex array into a feature.
        /// </summary>
        protected IFeature GetPolygon(int index)
        {
            if (FeatureGeometryFactory == null)
                FeatureGeometryFactory = GeometryFactory.Default;

            ShapeRange shape = ShapeIndices[index];
            List<ILinearRing> shells = new List<ILinearRing>();
            List<ILinearRing> holes = new List<ILinearRing>();
            foreach (PartRange part in shape.Parts)
            {
                List<Coordinate> coords = new List<Coordinate>();
                int i = part.StartIndex;
                foreach (Vertex d in part)
                {
                    Coordinate c = new Coordinate(d.X, d.Y);
                    if (M != null && M.Length > 0)
                    {
                        c.M = M[i];
                    }
                    if (Z != null && Z.Length > 0)
                    {
                        c.Z = Z[i];
                    }
                    i++;
                    coords.Add(c);
                }
                ILinearRing ring = FeatureGeometryFactory.CreateLinearRing(coords.ToArray());
                if (shape.Parts.Count == 1)
                {
                    shells.Add(ring);
                }
                else
                {
                    if (CGAlgorithms.IsCCW(ring.Coordinates))
                    {
                        holes.Add(ring);
                    }
                    else
                    {
                        shells.Add(ring);
                    }
                }
            }

            // Now we have a list of all shells and all holes
            List<ILinearRing>[] holesForShells = new List<ILinearRing>[shells.Count];
            for (int i = 0; i < shells.Count; i++)
            {
                holesForShells[i] = new List<ILinearRing>();
            }

            // Find holes
            foreach (ILinearRing t in holes)
            {
                ILinearRing currentHole = t;
                Envelope minEnv = null;
                Envelope currentHoleEnv = currentHole.EnvelopeInternal;
                Coordinate currentHoleFirstPt = currentHole.Coordinates[0];
                int addToShell = -1;

                for (int j = 0; j < shells.Count; j++)
                {
                    ILinearRing currentShell = shells[j];
                    Envelope currentShellEnv = currentShell.EnvelopeInternal;

                    // Check if this new containing ring is smaller than the current minimum ring
                    if (currentShellEnv.Contains(currentHoleEnv) && (CGAlgorithms.IsPointInRing(currentHoleFirstPt, currentShell.Coordinates) || PointInList(currentHoleFirstPt, currentShell.Coordinates)))
                    {
                        if (minEnv == null || minEnv.Contains(currentShellEnv))
                        {
                            minEnv = currentShellEnv;
                            addToShell = j; //remember the index of the shell this holes fits into, this might still change if we find a smaller shell that still contains the hole
                        }
                    }
                }
                if (addToShell > -1) holesForShells[addToShell].Add(t); //add the hole to the smallest shell it fits into
            }

            var polygons = new IPolygon[shells.Count];
            for (int i = 0; i < shells.Count; i++)
            {
                polygons[i] = FeatureGeometryFactory.CreatePolygon(shells[i], holesForShells[i].ToArray());
            }

            Feature feature = new Feature((polygons.Length == 1 ? polygons[0] : FeatureGeometryFactory.CreateMultiPolygon(polygons) as IGeometry))
            {
                ParentFeatureSet = this,
                ShapeIndex = shape
            };

            // Attributes handled in the overridden case
            return feature;
        }

        /// <summary>
        /// handles the attributes while adding a shape
        /// </summary>
        /// <param name="shape">
        /// </param>
        /// <returns>
        /// A data row, but only if attributes are populated
        /// </returns>
        private DataRow AddAttributes(Shape shape)
        {
            // Handle attributes if the array is not null.  Assumes compatible schema.
            if (shape.Attributes != null)
            {
                DataColumn[] columns = GetColumns();
                Dictionary<string, object> rowContent = new Dictionary<string, object>();
                object[] fixedContent = new object[columns.Length];

                if (shape.Attributes.Length != columns.Length)
                {
                    throw new ArgumentException("Attribute column count mismatch.");
                }

                for (int iField = 0; iField < columns.Length; iField++)
                {
                    object value = shape.Attributes[iField];
                    if (value != null)
                    {
                        if (columns[iField].DataType != value.GetType())
                        {
                            // this may throw an exception if the type casting fails
                            value = Convert.ChangeType(value, columns[iField].DataType);
                        }

                        fixedContent[iField] = value;
                        rowContent.Add(columns[iField].ColumnName, value);
                    }
                }

                if (AttributesPopulated)
                {
                    // just add a new DataRow
                    DataRow addedRow = _dataTable.NewRow();
                    addedRow.ItemArray = fixedContent;
                    return addedRow;
                }

                // Insert a new row in the source
                AddRow(rowContent);
            }

            return null;
        }

        /// <summary>
        /// Copies the subset of specified features to create a new featureset that is restricted to just the members specified.
        /// </summary>
        /// <param name="filterExpression">
        /// The string expression to test.
        /// </param>
        /// <param name="withAttributes"></param>
        /// <returns>
        /// A FeatureSet that has members that only match the specified members.
        /// </returns>
        private FeatureSet CopySubset(string filterExpression, bool withAttributes)
        {
            return CopySubset(SelectIndexByAttribute(filterExpression), withAttributes);
        }

        /// <summary>
        /// Test if a point is in a list of coordinates.
        /// </summary>
        /// <param name="testPoint">
        /// TestPoint the point to test for.
        /// </param>
        /// <param name="pointList">
        /// PointList the list of points to look through.
        /// </param>
        /// <returns>
        /// true if testPoint is a point in the pointList list.
        /// </returns>
        private static bool PointInList(Coordinate testPoint, IEnumerable<Coordinate> pointList)
        {
            return pointList.Any(p => p.Equals2D(testPoint));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether or not the attributes have all been loaded into the data table.
        /// </summary>
        public virtual bool AttributesPopulated
        {
            get
            {
                // This is true by default, and only overridden in cases where we can grab attributes from a file
                return true;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the coordinate type across the entire featureset.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CoordinateType CoordinateType { get; set; }

        /// <summary>
        /// DataTable is the System.Data.DataTable for all the attributes of this FeatureSet.
        /// This will call FillAttributes if it is accessed and that has not yet been called.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual DataTable DataTable
        {
            get
            {
                return _dataTable;
            }

            set
            {
                OnDataTableExcluded(_dataTable);
                _dataTable = value;
                OnDataTableIncluded(_dataTable);
            }
        }

        /// <summary>
        /// This is the envelope in Extent form.  This may be cached.
        /// </summary>
        public override Extent Extent
        {
            get
            {
                if (MyExtent == null || MyExtent.IsEmpty())
                {
                    UpdateExtent();
                }
                return MyExtent;
            }
            set
            {
                MyExtent = value;
            }
        }

        /// <summary>
        /// This is an optional GeometryFactory that can be set to control how the geometries on features are
        /// created. The "Feature" prefix allows us to access the static Default instance on GeometryFactory.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IGeometryFactory FeatureGeometryFactory { get; set; }

        /// <summary>
        /// Gets the feature lookup Table itself.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<DataRow, IFeature> FeatureLookup
        {
            get
            {
                return _featureLookup;
            }
        }

        /// <summary>
        /// Gets or sets an enumeration specifying whether this featureset contains Lines, Points, Polygons or an unspecified type.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FeatureType FeatureType { get; set; }

        /// <summary>
        /// A list of the features in this layer.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IFeatureList Features
        {
            get
            {
                if (_features == null || _features.Count == 0) //Changed by jany_: sometimes _features are empty when indexMode is false, so we have to load them then too
                {
                    // People working with features like this probably want to see changes from the features themselves.
                    IndexMode = true;
                    FeaturesFromVertices();
                    IndexMode = false;
                }
                return _features;
            }

            set
            {
                OnExcludeFeatures(_features);
                _features = value;
                OnIncludeFeatures(_features);
            }
        }

        /// <summary>
        /// If this is true, then the ShapeIndices and Vertex values are used, and features are created on demand.
        /// Otherwise the list of Features is used directly.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IndexMode { get; set; }

        /// <inheritdoc/>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double[] M
        {
            get
            {
                if (!_verticesAreValid)
                    OnInitializeVertices();
                return _m;
            }

            set
            {
                _m = value;
            }
        }

        /// <summary>
        /// Attempts to remove the specified shape.  If in memory, this will also remove the
        /// corresponding database row.  This has no affect on the underlying datasets.
        /// </summary>
        /// <param name="index">
        /// The integer index o the shape to remove.
        /// </param>
        /// <returns>
        /// Boolean, true if the remove was successful.
        /// </returns>
        public bool RemoveShapeAt(int index)
        {
            if (index < 0 || index >= _shapeIndices.Count)
                return false;
            if (IndexMode == false)
            {
                Features.RemoveAt(index);
                InitializeVertices();
                return true;
            }

            ShapeRange sr = _shapeIndices[index];

            // remove the x,y vertices
            double[] xyResult = new double[_vertices.Length - sr.NumPoints * 2];
            if (sr.StartIndex > 0)
            {
                Array.Copy(_vertices, 0, xyResult, 0, sr.StartIndex * 2);
            }
            int end = sr.StartIndex * 2 + sr.NumPoints * 2;
            if (index < _shapeIndices.Count - 1)
            {
                Array.Copy(_vertices, end, xyResult, sr.StartIndex * 2, _vertices.Length - end);
            }
            _vertices = xyResult;

            // remove the m values if necessary
            if (CoordinateType == CoordinateType.M)
            {
                double[] mResult = new double[_m.Length - sr.NumPoints];
                if (index > 0)
                {
                    Array.Copy(_m, 0, mResult, 0, sr.StartIndex);
                }
                end = sr.StartIndex + sr.NumPoints;
                if (index < _shapeIndices.Count - 1)
                {
                    Array.Copy(_m, end, mResult, sr.StartIndex, _m.Length - sr.NumPoints);
                }
                _m = mResult;
            }

            // remove the z values if necessary
            if (CoordinateType == CoordinateType.Z)
            {
                double[] zResult = new double[_m.Length - sr.NumPoints];
                if (index > 0)
                {
                    Array.Copy(_z, 0, zResult, 0, sr.StartIndex);
                }
                end = sr.StartIndex + sr.NumPoints;
                if (index < _shapeIndices.Count - 1)
                {
                    Array.Copy(_z, end, zResult, sr.StartIndex, _m.Length - sr.NumPoints);
                }
                _z = zResult;
            }

            int count = sr.NumPoints;

            // remove the ShapeRange for this index
            _shapeIndices.RemoveAt(index);

            // Update the offsets of all the shape indices above this one, if any, to
            // be the point offset without the shape.
            for (int i = index; i < _shapeIndices.Count; i++)
            {
                _shapeIndices[i].StartIndex -= count;
            }

            // Updating the vertex array means that the parts are now pointing
            // to the wrong array of vertices internally.  This doesn't affect
            // rendering, but will affect selection.
            foreach (ShapeRange shape in _shapeIndices)
            {
                foreach (PartRange part in shape.Parts)
                {
                    part.Vertices = _vertices;
                }
            }

            if (AttributesPopulated)
            {
                DataTable.Rows.RemoveAt(index);
            }

            return true;
        }

        /// <summary>
        /// Attempts to remove a range of shapes by index.  This is optimized to
        /// work better for large numbers.  For one or two, using RemoveShapeAt might
        /// be faster.
        /// </summary>
        /// <param name="indices">
        /// The enumerable set of indices to remove.
        /// </param>
        public void RemoveShapesAt(IEnumerable<int> indices)
        {
            IEnumerable<int> enumerable = indices as IList<int> ?? indices.ToList();
            List<int> remove = enumerable.ToList(); // moved by jany_ (2015-06-11) because in non index mode the features have to be removed from largest to smallest index
            remove.Sort();
            if (remove.Count == 0)
                return;

            if (IndexMode == false)
            {
                for (int i = remove.Count - 1; i >= 0; i--)
                {
                    if (remove[i] < 0 || remove[i] >= _shapeIndices.Count)
                        continue;
                    Features.RemoveAt(remove[i]);
                }
                InitializeVertices();
                return;
            }

            List<int> remaining = new List<int>();
            for (int i = 0; i < _shapeIndices.Count; i++)
            {
                if (remove.Count > 0 && remove[0] == i)
                {
                    remove.Remove(i);
                    continue;
                }
                remaining.Add(i);
            }

            List<double> vertex = new List<double>();
            List<double> z = new List<double>();
            List<double> m = new List<double>();
            int pointTotal = 0;
            ProgressMeter = new ProgressMeter(ProgressHandler, "Removing Vertices", remaining.Count);
            foreach (int index in remaining)
            {
                if (index < 0 || index >= _shapeIndices.Count)
                    continue;
                ShapeRange sr = _shapeIndices[index];
                double[] xyShape = new double[sr.NumPoints * 2];
                Array.Copy(_vertices, sr.StartIndex * 2, xyShape, 0, sr.NumPoints * 2);
                vertex.AddRange(xyShape);

                /////////////////////////////////////////////////////////////////
                // fix to address issue http://dotspatial.codeplex.com/workitem/174
                ////////////////////////////////////////////////////////////////
                //// remove the m values if necessary
                //if (CoordinateType == CoordinateType.M)
                //{
                //    double[] mShape = new double[sr.NumPoints];
                //    Array.Copy(_m, sr.StartIndex, mShape, 0, sr.NumPoints);
                //    m.AddRange(mShape);
                //}

                // remove the z values if necessary
                if (CoordinateType == CoordinateType.Z)
                {
                    double[] zShape = new double[sr.NumPoints];
                    Array.Copy(_z, sr.StartIndex, zShape, 0, sr.NumPoints);
                    z.AddRange(zShape);
                    /////////////////////////////////////////////////////////////////
                    // fix to address issue http://dotspatial.codeplex.com/workitem/174
                    ////////////////////////////////////////////////////////////////
                    double[] mShape = new double[sr.NumPoints];
                    Array.Copy(_m, sr.StartIndex, mShape, 0, sr.NumPoints);
                    m.AddRange(mShape);
                    /////////////////////////////////////////////////////////////////
                }
                sr.StartIndex = pointTotal;
                pointTotal += sr.NumPoints;
                ProgressMeter.Next();
            }
            ProgressMeter.Reset();

            _vertices = vertex.ToArray();
            _m = m.ToArray();
            _z = z.ToArray();
            remove = enumerable.ToList();
            remove.Sort();

            ProgressMeter = new ProgressMeter(ProgressHandler, "Removing indices", remove.Count);
            List<ShapeRange> result = new List<ShapeRange>();
            int myIndex = 0;
            foreach (ShapeRange range in _shapeIndices)
            {
                if (remove.Count > 0 && remove[0] == myIndex)
                {
                    remove.RemoveAt(0);
                }
                else
                {
                    result.Add(range);
                }
                ProgressMeter.Next();
                myIndex++;
            }
            _shapeIndices = result;
            ProgressMeter.Reset();

            remove = enumerable.ToList();
            remove.Sort();
            remove.Reverse();
            ProgressMeter = new ProgressMeter(ProgressHandler, "Removing Attribute Rows", remove.Count);
            foreach (int index in remove)
            {
                if (AttributesPopulated)
                {
                    DataTable.Rows.RemoveAt(index);
                }
                ProgressMeter.Next();
            }
            ProgressMeter.Reset();
            ProgressMeter = new ProgressMeter(ProgressHandler, "Reassigning part vertex pointers", _shapeIndices.Count);
            // Updating the vertex array means that the parts are now pointing
            // to the wrong array of vertices internally.  This doesn't affect
            // rendering, but will affect selection.
            foreach (ShapeRange shape in _shapeIndices)
            {
                foreach (PartRange part in shape.Parts)
                {
                    part.Vertices = _vertices;
                }
            }
        }

        /// <summary>
        /// Reprojects all of the in-ram vertices of this featureset.
        /// This will also update the projection to be the specified projection.
        /// </summary>
        /// <param name="targetProjection">
        /// The projection information to reproject the coordinates to.
        /// </param>
        public override void Reproject(ProjectionInfo targetProjection)
        {
            Projections.Reproject.ReprojectPoints(Vertex, Z, Projection, targetProjection, 0, Vertex.Length / 2);
            if (!IndexMode)
                UpdateCoordinates();

            foreach (ShapeRange shape in ShapeIndices)
            {
                foreach (PartRange part in shape.Parts)
                    part.Vertices = Vertex;
            }

            UpdateExtent();
            Projection = targetProjection;
        }

        /// <summary>
        /// These specifically allow the user to make sense of the Vertices array.  These are
        /// fast acting sealed classes and are not meant to be overridden or support clever
        /// new implementations.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ShapeRange> ShapeIndices
        {
            get
            {
                if (_shapeIndices == null) OnInitializeVertices();
                return _shapeIndices;
            }

            set
            {
                _shapeIndices = value;
            }
        }

        /// <summary>
        /// After changing coordinates, this will force the re-calculation of envelopes on a feature
        /// level or test the shapes in index mode to rebuild an Extent.
        /// </summary>
        public void UpdateExtent()
        {
            MyExtent = new Extent();
            if (!IndexMode)
            {
                if (Features == null || Features.Count <= 0)
                {
                    // jany_ (2015-07-17) return the empty extent because any other extent would result in to big extent when zooming to full map extent
                    return;
                }

                foreach (IFeature feature in Features)
                {
                    feature.Geometry.UpdateEnvelope();
                    MyExtent.ExpandToInclude(new Extent(feature.Geometry.EnvelopeInternal));
                }
            }
            else
            {
                if (_shapeIndices == null || _shapeIndices.Count == 0)
                {
                    // jany_ (2015-07-17) return the empty extent because any other extent would result in to big extent when zooming to full map extent
                    return;
                }

                foreach (ShapeRange range in _shapeIndices)
                {
                    range.CalculateExtents();
                    MyExtent.ExpandToInclude(range.Extent);
                }
            }
        }

        /// <inheritdoc/>
        public double[] Vertex
        {
            get
            {
                if (!_verticesAreValid)
                    OnInitializeVertices();
                return _vertices;
            }

            set
            {
                _vertices = value;
                if (_shapeIndices != null)
                {
                    foreach (ShapeRange shape in _shapeIndices)
                    {
                        shape.SetVertices(_vertices);
                    }
                }

                _verticesAreValid = true;
            }
        }

        /// <summary>
        /// Gets a Boolean that indicates whether or not the InvalidateVertices has been called
        /// more recently than the cached vertex array has been built.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool VerticesAreValid
        {
            get
            {
                return _verticesAreValid;
            }
        }

        /// <inheritdoc/>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double[] Z
        {
            get
            {
                if (!_verticesAreValid)
                    OnInitializeVertices();
                return _z;
            }

            set
            {
                _z = value;
            }
        }

        /// <summary>
        /// Calculates the features from the shape indices and vertex array.
        /// </summary>
        protected void FeaturesFromVertices()
        {
            if (_features == null)
            {
                _features = new FeatureList(this) { IncludeAttributes = false };
            }
            else
            {
                // need to preserve event handler already attached to this feature list
                _features.Clear();
                _features.IncludeAttributes = false;
                _featureLookup.Clear();
            }

            _features.SuspendEvents();
            for (int shp = 0; shp < ShapeIndices.Count; shp++)
            {
                var f = GetFeature(shp);
                _features.Add(f);
                if (AttributesPopulated)
                {
                    // Don't force population if we haven't populated yet, but
                    // definitely assign the DataRow if it already exists.
                    _features[shp].DataRow = DataTable.Rows[shp];
                    _featureLookup.Add(_features[shp].DataRow, _features[shp]); //Added by jany_: sync the _featureLookup
                }
            }

            _features.ResumeEvents();
            _features.IncludeAttributes = true;

            // from this point on, any features that get added will also add a DataRow to the DataTable.
        }

        /// <summary>
        /// Allows the un-wiring of event handlers related to the data Table.
        /// </summary>
        /// <param name="dataTable">
        /// </param>
        protected virtual void OnDataTableExcluded(DataTable dataTable)
        {
            if (dataTable != null)
            {
                dataTable.RowDeleted -= DataTableRowDeleted;
            }
        }

        /// <summary>
        /// Allows the wiring of event handlers related to the data Table.
        /// </summary>
        /// <param name="dataTable">
        /// </param>
        protected virtual void OnDataTableIncluded(DataTable dataTable)
        {
            if (dataTable != null)
            {
                dataTable.RowDeleted += DataTableRowDeleted;
            }
        }

        /// <summary>
        /// Occurs when removing the feature list, allowing events to be disconnected
        /// </summary>
        /// <param name="features">
        /// </param>
        protected virtual void OnExcludeFeatures(IFeatureList features)
        {
            if (_features == null)
            {
                return;
            }

            _features.FeatureAdded -= FeaturesFeatureAdded;
            _features.FeatureRemoved -= FeaturesFeatureRemoved;
        }

        /// <summary>
        /// Occurs when setting the feature list, allowing events to be connected
        /// </summary>
        /// <param name="features">
        /// </param>
        protected virtual void OnIncludeFeatures(IFeatureList features)
        {
            if (_features == null)
            {
                return;
            }

            _features.FeatureAdded += FeaturesFeatureAdded;
            _features.FeatureRemoved += FeaturesFeatureRemoved;
        }

        #endregion

        #region IFeatureSet Members

        /// <inheritdoc/>
        public virtual void AddRow(Dictionary<string, object> values)
        {
        }

        /// <inheritdoc/>
        public virtual void AddRow(DataRow values)
        {
        }

        /// <inheritdoc/>
        public virtual void Edit(int index, Dictionary<string, object> values)
        {
        }

        /// <inheritdoc/>
        public virtual void Edit(int index, DataRow values)
        {
        }

        /// <inheritdoc />
        public virtual DataTable GetAttributes(int startIndex, int numRows)
        {
            return GetAttributes(startIndex, numRows, GetColumns().Select(d => d.ColumnName));
        }

        /// <summary>
        /// Reads just the content requested in order to satisfy the paging ability of VirtualMode for the DataGridView
        /// </summary>
        /// <param name="startIndex">
        /// The integer lower page boundary.
        /// </param>
        /// <param name="numRows">
        /// The integer number of attribute rows to return for the page.
        /// </param>
        /// <param name="fieldNames">
        /// The list or array of fieldnames to return.
        /// </param>
        /// <returns>
        /// A DataTable populated with data rows with only the specified values.
        /// </returns>
        public virtual DataTable GetAttributes(int startIndex, int numRows, IEnumerable<string> fieldNames)
        {
            // overridden in subclasses.
            var result = new DataTable();
            DataColumn[] columns = GetColumns();

            // Always add FID in this paging scenario.  This is for the in-ram case.  A more appropriate
            // implementation exists
            if (columns.All(c => c.ColumnName != "FID"))
            {
                result.Columns.Add("FID", typeof(int));
            }

            var fn = new HashSet<string>(fieldNames);
            foreach (var col in columns)
            {
                if (fn.Contains(col.ColumnName))
                {
                    result.Columns.Add(col);
                }
            }

            for (int row = startIndex, i = 0; i < numRows && row < _dataTable.Rows.Count; row++, i++)
            {
                DataRow myRow = result.NewRow();
                myRow["FID"] = row;
                foreach (var name in fn.Where(d => d != "FID"))
                {
                    myRow[name] = _dataTable.Rows[row][name];
                }

                result.Rows.Add(myRow);
            }

            return result;
        }

        /// <inheritdoc/>
        public DataColumn GetColumn(string name)
        {
            DataColumn[] columns = GetColumns();
            return columns.FirstOrDefault(field => field.ColumnName == name);
        }

        /// <inheritdoc/>
        public virtual DataColumn[] GetColumns()
        {
            return (from DataColumn column in DataTable.Columns
                    select new DataColumn(column.ColumnName, column.DataType)).ToArray();
        }

        /// <inheritdoc/>
        public virtual int[] GetCounts(string[] expressions, ICancelProgressHandler progressHandler, int maxSampleSize)
        {
            int[] counts = null;

            if (expressions != null && expressions.Length > 0)
            {
                counts = new int[expressions.Length];
                for (int i = 0; i < expressions.Length; i++)
                {
                    if (expressions[i] != null)
                    {
                        if (expressions[i].Contains("=[NULL]"))
                        {
                            expressions[i] = expressions[i].Replace("=[NULL]", " is NULL");
                        }
                        else
                            if (expressions[i].Contains("= '[NULL]'"))
                            {
                                expressions[i] = expressions[i].Replace("= '[NULL]'", " is NULL");
                            }
                    }
                    counts[i] = DataTable.Select(expressions[i]).Length;
                }
            }

            return counts;
        }

        /// <inheritdoc/>
        public virtual int NumRows()
        {
            // overridden in sub-classes to prevent relying on an in-memory data table.
            return DataTable.Rows.Count;
        }

        /// <inheritdoc/>
        public virtual void SetAttributes(int startIndex, DataTable pageValues)
        {
            // overridden in sub-classes, but default implementation is for the in-ram only case.
            int row = startIndex;
            if (_dataTable == null)
            {
                _dataTable = new DataTable();
            }

            List<string> names = new List<string>();
            foreach (DataColumn c in pageValues.Columns)
            {
                _dataTable.Columns.Add(new DataColumn(c.ColumnName, c.DataType));
                names.Add(c.ColumnName);
            }

            foreach (DataRow dataRow in pageValues.Rows)
            {
                foreach (string name in names)
                {
                    _dataTable.Rows[row][name] = dataRow[name];
                }

                row++;
            }
        }

        #endregion

        /// <summary>
        /// Occurs when the vertices are being re-calculated.
        /// </summary>
        protected virtual void OnInitializeVertices()
        {
            if (_features == null)
                return;

            int count = _features.Sum(f => f.Geometry.NumPoints);
            _vertices = new double[count * 2];
            int i = 0;
            foreach (IFeature f in _features)
            {
                // this should be all the coordinates, for all parts of the geometry.
                IList<Coordinate> coords = f.Geometry.Coordinates;

                if (coords == null) continue;

                foreach (Coordinate c in coords)
                {
                    _vertices[i * 2] = c.X;
                    _vertices[i * 2 + 1] = c.Y;

                    // essentially add a reference pointer to the internal array of values
                    i++;
                }
            }

            // not sure, but I bet arrays a smidge faster at indexed access than lists
            _shapeIndices = new List<ShapeRange>();
            int vIndex = 0;
            foreach (IFeature f in _features)
            {
                ShapeRange shx = new ShapeRange(FeatureType) { Extent = new Extent(f.Geometry.EnvelopeInternal), StartIndex = vIndex };
                _shapeIndices.Add(shx);
                f.ShapeIndex = shx;

                // for simplicity in looping, there is always at least one part.
                // That way, the shape range can be ignored and the parts loop used instead.
                int shapeStart = vIndex;
                for (int part = 0; part < f.Geometry.NumGeometries; part++)
                {
                    PartRange prtx = new PartRange(_vertices, shapeStart, vIndex - shapeStart, FeatureType);
                    IPolygon bp = f.Geometry.GetGeometryN(part) as IPolygon;
                    if (bp != null)
                    {
                        // Account for the Shell
                        prtx.NumVertices = bp.Shell.NumPoints;
                        vIndex += bp.Shell.NumPoints;

                        // The part range should be adjusted to no longer include the holes
                        foreach (var hole in bp.Holes)
                        {
                            PartRange holex = new PartRange(_vertices, shapeStart, vIndex - shapeStart, FeatureType)
                            {
                                NumVertices = hole.NumPoints
                            };
                            shx.Parts.Add(holex);
                            vIndex += hole.NumPoints;
                        }
                    }
                    else
                    {
                        int numPoints = f.Geometry.GetGeometryN(part).NumPoints;

                        // This is not a polygon, so just add the number of points.
                        vIndex += numPoints;
                        prtx.NumVertices = numPoints;
                    }
                    shx.Parts.Add(prtx);
                }
                shx.NumParts = shx.Parts.Count;
                shx.NumPoints = vIndex - shx.StartIndex; //Changed by jany_: has to be initialized to correctly paint multipoints 
            }
            _verticesAreValid = true;
        }

        /// <summary>
        /// Fires the VerticesInvalidated event
        /// </summary>
        protected virtual void OnVerticesInvalidated()
        {
            if (VerticesInvalidated != null)
            {
                VerticesInvalidated(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// This forces the cached vertices array to be copied back to the individual X and Y values
        /// of the coordinates themselves.
        /// </summary>
        private void UpdateCoordinates()
        {
            int i = 0;
            foreach (IFeature f in _features)
            {
                // this should be all the coordinates, for all parts of the geometry.
                IList<Coordinate> coords = f.Geometry.Coordinates;

                if (coords == null)
                    continue;

                foreach (Coordinate c in coords)
                {
                    c.X = _vertices[i * 2];
                    c.Y = _vertices[i * 2 + 1];
                    i++;
                }
            }
        }

        /// <summary>
        /// Disposes the unmanaged memory objects.
        /// </summary>
        /// <param name="disposeManagedResources">If this is true, managed resources are set to null.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                _features = null;
                Filename = null;
                _m = null;
                _shapeIndices = null;
                _vertices = null;
                _z = null;
            }
            if (_dataTable != null)
                _dataTable.Dispose();

            base.Dispose(disposeManagedResources);
        }
    }
}