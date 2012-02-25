// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
// The Original Code is DotSpatial.dll
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using DotSpatial.Projections;
using DotSpatial.Serialization;
using DotSpatial.Topology;
using DotSpatial.Topology.Algorithm;

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
        private Dictionary<DataRow, IFeature> _featureLookup;

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
                    WkbFeatureReader.ReadFeature(ms, result);
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
            _dataTable = new DataTable();
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
        public IFeature AddFeature(IBasicGeometry geometry)
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

            if (_featureLookup.ContainsKey(e.Feature.DataRow))
            {
                _featureLookup[e.Feature.DataRow] = e.Feature;
            }
            else
            {
                _featureLookup.Add(e.Feature.DataRow, e.Feature);
            }

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
        /// does not already exist
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
            IFeature addedFeature;

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
                addedFeature = new Feature(shape);

                Features.Add(addedFeature);
                addedFeature.DataRow = AddAttributes(shape);
                if (!shape.Range.Extent.IsEmpty())
                {
                    Extent.ExpandToInclude(new Extent(addedFeature.Envelope));
                }
            }
        }

        /// <inheritdoc/>
        public void AddShapes(IEnumerable<Shape> shapes)
        {
            if (!IndexMode)
            {
                _features.SuspendEvents();
                foreach (Shape shape in shapes)
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
            foreach (Shape shape in shapes)
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
            foreach (Shape shape in shapes)
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

                ShapeRange newRange = shape.Range.Copy();
                newRange.StartIndex = offset;
                offset += num;
                ShapeIndices.Add(newRange);
            }

            Vertex = vertices;
            UpdateExtent();
        }

        /// <inheritdoc/>
        public void CopyFeatures(IFeatureSet source, bool copyAttributes)
        {
            ProgressMeter = new ProgressMeter(ProgressHandler, "Copying Features", ShapeIndices.Count);
            Vertex = source.Vertex.Copy();
            _shapeIndices = new List<ShapeRange>();
            foreach (ShapeRange range in source.ShapeIndices)
            {
                _shapeIndices.Add(range.Copy());
            }

            if (copyAttributes)
            {
                foreach (DataColumn dc in source.GetColumns())
                {
                    if (dc != null)
                    {
                        DataColumn outCol = new DataColumn(dc.ColumnName, dc.DataType, dc.Expression, dc.ColumnMapping);
                        Field fld = new Field(outCol);
                        DataTable.Columns.Add(fld);
                    }
                }
            }

            if (source.AttributesPopulated)
            {
                // Handle data table content directly
                if (!IndexMode)
                {
                    // If not in index mode, just handle this using features
                    Features.SuspendEvents();
                    int i = 0;
                    foreach (IFeature f in source.Features)
                    {
                        IFeature copy = AddFeature(f.BasicGeometry);
                        copy.ShapeIndex = ShapeIndices[i];
                        if (copyAttributes)
                        {
                            copy.DataRow.ItemArray = f.DataRow.ItemArray.Copy();
                        }

                        i++;
                    }

                    Features.ResumeEvents();
                }
                else
                {
                    // We need to copy the attributes, but just copy a datarow
                    if (copyAttributes)
                    {
                        foreach (DataRow row in source.DataTable.Rows)
                        {
                            DataRow result = DataTable.NewRow();
                            result.ItemArray = row.ItemArray.Copy();
                            DataTable.Rows.Add(result);
                        }
                    }
                }
            }
            else
            {
                AttributesPopulated = false;

                // Handle data table content directly
                if (!IndexMode)
                {
                    // If not in index mode, just handle this using features
                    Features.SuspendEvents();
                    int i = 0;
                    foreach (IFeature f in source.Features)
                    {
                        IFeature result = AddFeature(f.BasicGeometry);
                        result.ShapeIndex = ShapeIndices[i];
                        i++;
                    }

                    Features.ResumeEvents();
                }

                if (copyAttributes)
                {
                    // We need to copy the attributes, but use the page system
                    int maxRow = NumRows();
                    const int pageSize = 10000;
                    int numPages = (int)Math.Ceiling(maxRow / (double)pageSize);
                    for (int i = 0; i < numPages; i++)
                    {
                        int numRows = pageSize;
                        if (i == numPages - 1)
                        {
                            numRows = numPages - (pageSize * i);
                        }

                        DataTable dt = source.GetAttributes(i * pageSize, numRows);
                        SetAttributes(i * pageSize, dt);
                    }
                }
            }
        }

        /// <summary>
        /// The copy subset.
        /// </summary>
        /// <param name="indices">
        /// The indices.
        /// </param>
        /// <returns>
        /// </returns>
        IFeatureSet IFeatureSet.CopySubset(List<int> indices)
        {
            return CopySubset(indices);
        }

        /// <summary>
        /// The copy subset.
        /// </summary>
        /// <param name="filterExpression">
        /// The filter expression.
        /// </param>
        /// <returns>
        /// </returns>
        IFeatureSet IFeatureSet.CopySubset(string filterExpression)
        {
            return CopySubset(filterExpression);
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
        public List<int> Find(string filterExpression)
        {
            DataRow[] hits = _dataTable.Select(filterExpression);
            return hits.Select(dr => _dataTable.Rows.IndexOf(dr)).ToList();
        }

        /// <summary>
        /// Gets the list of string names available as columns from the specified excel file.
        /// </summary>
        /// <param name="xlsFilePath">
        /// </param>
        /// <returns>
        /// </returns>
        /// Warning. This method should be moved outside of FeatureSet.
        public List<string> GetColumnNames(string xlsFilePath)
        {
            OleDbConnection con =
                new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + xlsFilePath +
                                    "; Extended Properties=Excel 8.0");
            // GIS Group of Maryland Environmental Service recommended this query string.
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [Data$]", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return (from DataColumn column in dt.Columns
                    select column.ColumnName).ToList();
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
            if (IndexMode == false)
            {
                return new Shape(Features[index]);
            }

            Shape result = new Shape(FeatureType);

            // This will also deep copy the parts, attributes and vertices
            ShapeRange range = ShapeIndices[index];
            result.Range = range.Copy();
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
            if (AttributesPopulated)
            {
                if (getAttributes)
                {
                    result.Attributes = DataTable.Rows[index].ItemArray;
                }
            }
            else
            {
                DataTable dt = GetAttributes(index, 1);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result.Attributes = dt.Rows[0].ItemArray;
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

        /// <summary>
        /// For attributes that are small enough to be loaded into a data table, this
        /// will join attributes from a foreign table (from an excel file).  This method
        /// won't create new rows in this table, so only matching members are brought in,
        /// but no rows are removed either, so not all rows will receive data.
        /// </summary>
        /// <param name="xlsFilePath">
        /// The complete path of the file to join
        /// </param>
        /// <param name="localJoinField">
        /// The field name to join on in this table
        /// </param>
        /// <param name="xlsJoinField">
        /// The field in the foreign table.
        /// </param>
        /// <returns>
        /// A modified featureset with the changes.
        /// </returns>
        public IFeatureSet Join(string xlsFilePath, string localJoinField, string xlsJoinField)
        {
            IFeatureSet res = Open(Filename);
            if (!res.AttributesPopulated)
            {
                FillAttributes();
            }

            if (!res.DataTable.Columns.Contains(localJoinField))
            {
                throw new Exception("The local join field specified is not in this table.");
            }

            // This connection string will not likely work on 64 bit machines.
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source="
                                                      + xlsFilePath + "; Extended Properties=Excel 8.0");
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [Data$]", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (!dt.Columns.Contains(xlsJoinField))
            {
                throw new Exception("The foreign join field specified is not in the xls data source specified.");
            }

            List<DataColumn> copyColumns = new List<DataColumn>();
            foreach (DataColumn column in dt.Columns)
            {
                if (res.DataTable.Columns.Contains(column.ColumnName))
                {
                    continue;
                }

                copyColumns.Add(column);
                res.DataTable.Columns.Add(
                    new DataColumn(column.ColumnName, column.DataType, column.Expression, column.ColumnMapping));
            }

            foreach (DataRow row in res.DataTable.Rows)
            {
                string query;
                if (dt.Columns[xlsJoinField].DataType == typeof(string))
                {
                    query = "[" + xlsJoinField + "] = '" + row[localJoinField] + "'";
                }
                else
                {
                    query = "[" + xlsJoinField + "] = " + row[localJoinField];
                }

                DataRow[] result = dt.Select(query);

                if (result.Length == 0)
                {
                    continue;
                }

                foreach (DataColumn column in copyColumns)
                {
                    row[column.ColumnName] = result[0][column.ColumnName];
                }
            }

            return res;
        }

        /// <inheritdoc/>
        public void Save()
        {
            if (!AttributesPopulated)
            {
                FillAttributes();
            }

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
                else
                {
                    if (Features.Count > 0)
                    {
                        FeatureType = Features[0].FeatureType;
                    }
                }
            }

            IFeatureSet result = DataManager.DefaultDataManager.CreateVector(fileName, FeatureType, ProgressHandler);

            // Previously I prevented setting of the features list, but I think we can negate that decision.
            // No reason to prevent it that I can see.
            // Same for the ShapeIndices.  I'd like to simply set them here and prevent the hassle of a
            // pure copy process.
            result.Vertex = Vertex;
            result.ShapeIndices = ShapeIndices;
            result.Extent = Extent;
            result.IndexMode = IndexMode; // added by JamesP@esdm.co.uk as this was not being passed into result
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
            List<IFeature> result = new List<IFeature>();
            if (IndexMode)
            {
                ShapeRange aoi = new ShapeRange(region);
                Extent affected = new Extent();
                List<ShapeRange> shapes = ShapeIndices;
                if (shapes != null)
                {
                    //ProgressMeter = new ProgressMeter(ProgressHandler, "Selecting shapes", shapes.Count);
                    for (int shp = 0; shp < shapes.Count; shp++)
                    {
                        //ProgressMeter.Next();
                        if (!shapes[shp].Intersects(aoi))
                        {
                            continue;
                        }

                        IFeature f = GetFeature(shp);
                        affected.ExpandToInclude(shapes[shp].Extent);
                        result.Add(f);
                    }
                    //ProgressMeter.Reset();
                }

                affectedRegion = affected;
                return result;
            }

            affectedRegion = new Extent();

            bool useProgress = (Features.Count > 10000);
            //ProgressMeter = new ProgressMeter(ProgressHandler, "Selecting Features", Features.Count);
            foreach (IFeature feature in Features)
            {
                //if (useProgress)
                //    ProgressMeter.Next();
                if (!region.Intersects(feature.Envelope))
                {
                    continue;
                }
                if (!feature.Intersects(region.ToEnvelope()))
                {
                    continue;
                }
                result.Add(feature);
                affectedRegion.ExpandToInclude(feature.Envelope.ToExtent());
            }
            //if (useProgress)
            //    ProgressMeter.Reset();

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
            List<IFeature> result = new List<IFeature>();
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
            if (FeatureLookup != null)
            {
                result.AddRange(rows.Select(dr => FeatureLookup[dr]));
            }

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
            List<int> result = new List<int>();
            if (AttributesPopulated && DataTable != null)
            {
                DataRow[] rows = DataTable.Select(filterExpression);
                result.AddRange(rows.Select(row => DataTable.Rows.IndexOf(row)));
            }
            else
            {
                // page in sets of 10, 000 rows
                int numPages = (int)Math.Ceiling((double)NumRows() / 10000);
                for (int page = 0; page < numPages; page++)
                {
                    DataTable table = GetAttributes(page * 10000, 10000);
                    DataRow[] rows = table.Select(filterExpression);

                    foreach (DataRow row in rows)
                    {
                        result.Add(table.Rows.IndexOf(row) + page * 10000);
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
        /// An integer list of indices to copy into the new FeatureSet
        /// </param>
        /// <returns>
        /// A FeatureSet with the new items.
        /// </returns>
        public FeatureSet CopySubset(List<int> indices)
        {
            FeatureSet copy = MemberwiseClone() as FeatureSet;
            if (copy == null)
            {
                return null;
            }

            copy.Features = new FeatureList(copy);
            foreach (int row in indices)
            {
                copy.Features.Add(Features[row]);
            }

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
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        protected IFeature GetLine(int index)
        {
            ShapeRange shape = ShapeIndices[index];
            List<IBasicLineString> lines = new List<IBasicLineString>();
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

                lines.Add(new LineString(coords));
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
            else
            {
                geom = FeatureGeometryFactory.CreateLineString(lines[0].Coordinates);
            }

            Feature f = new Feature(geom)
                            {
                                ParentFeatureSet = this,
                                ShapeIndex = shape,
                                RecordNumber = shape.RecordNumber,
                                ContentLength = shape.ContentLength,
                                ShapeType = shape.ShapeType
                            };

            // Attribute reading is only handled in the overridden case.
            return f;
        }

        /// <summary>
        /// Returns a single multipoint feature for the shape at the specified index
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
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

            IMultiPoint mp = FeatureGeometryFactory.CreateMultiPoint(coords);
            return new Feature(mp);
        }

        /// <summary>
        /// Gets the point for the shape at the specified index
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        protected IFeature GetPoint(int index)
        {
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
            {
                FeatureGeometryFactory = GeometryFactory.Default;
            }

            IPoint p = FeatureGeometryFactory.CreatePoint(c);
            Feature f = new Feature(p) { ParentFeatureSet = this, RecordNumber = index };

            // Attributes only retrieved in the overridden case
            return f;
        }

        /// <summary>
        /// If the FeatureType is polygon, this is the code for converting the vertex array
        /// into a feature.
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        protected IFeature GetPolygon(int index)
        {
            if (FeatureGeometryFactory == null)
            {
                FeatureGeometryFactory = GeometryFactory.Default;
            }

            Feature feature = new Feature
                                  {
                                      Envelope = ShapeIndices[index].Extent.ToEnvelope()
                                  };
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
                ILinearRing ring = FeatureGeometryFactory.CreateLinearRing(coords);
                if (shape.Parts.Count == 1)
                {
                    shells.Add(ring);
                }
                else
                {
                    if (CgAlgorithms.IsCounterClockwise(ring.Coordinates))
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
                ILinearRing testRing = t;
                ILinearRing minShell = null;
                IEnvelope minEnv = null;
                IEnvelope testEnv = testRing.EnvelopeInternal;
                Coordinate testPt = testRing.Coordinates[0];
                ILinearRing tryRing;
                for (int j = 0; j < shells.Count; j++)
                {
                    tryRing = shells[j];
                    IEnvelope tryEnv = tryRing.EnvelopeInternal;
                    if (minShell != null)
                    {
                        minEnv = minShell.EnvelopeInternal;
                    }

                    bool isContained = false;

                    if (tryEnv.Contains(testEnv)
                        && (CgAlgorithms.IsPointInRing(testPt, tryRing.Coordinates)
                            || PointInList(testPt, tryRing.Coordinates)))
                    {
                        isContained = true;
                    }

                    // Check if this new containing ring is smaller than the current minimum ring
                    if (isContained)
                    {
                        if (minShell == null || minEnv.Contains(tryEnv))
                        {
                            minShell = tryRing;
                        }

                        holesForShells[j].Add(t);
                    }
                }
            }

            IPolygon[] polygons = new Polygon[shells.Count];
            for (int i = 0; i < shells.Count; i++)
            {
                polygons[i] = FeatureGeometryFactory.CreatePolygon(shells[i], holesForShells[i].ToArray());
            }

            if (polygons.Length == 1)
            {
                feature.BasicGeometry = polygons[0];
            }
            else
            {
                // It's a multi part
                feature.BasicGeometry = FeatureGeometryFactory.CreateMultiPolygon(polygons);
            }

            // feature.FID = feature.RecordNumber; FID is now dynamic
            feature.ParentFeatureSet = this;
            feature.ShapeIndex = shape;
            feature.RecordNumber = shape.RecordNumber;
            feature.ContentLength = shape.ContentLength;
            feature.ShapeType = shape.ShapeType;

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
                DataRow addedRow;
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
                    addedRow = _dataTable.NewRow();
                    addedRow.ItemArray = fixedContent;
                    return addedRow;
                }

                // Insert a new row in the source
                AddRow(rowContent);
            }

            return null;
        }

        /// <summary>
        /// Copies the subset of specified features to create a new featureset that is restricted to
        /// just the members specified.
        /// </summary>
        /// <param name="filterExpression">
        /// The string expression to test.
        /// </param>
        /// <returns>
        /// A FeatureSet that has members that only match the specified members.
        /// </returns>
        private FeatureSet CopySubset(string filterExpression)
        {
            return CopySubset(Find(filterExpression));
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
        /// Gets or sets the surrent file path. This is the relative path relative to
        /// the current project folder. For feature sets coming from a database
        /// or a web service, the FilePath property is NULL.
        /// </summary>
        /// <value>
        /// The relative file path.
        /// </value>
        /// <remarks>This property is used when saving source file information to a DSPX project.</remarks>
        [Serialize("FilePath")]
        public virtual string FilePath
        {
            get
            {
                //do not construct FilePath for FeatureSets without a Filename
                if (String.IsNullOrEmpty(Filename)) return null;

                return RelativePathTo(Filename);
            }

            set
            {
                Filename = AbsolutePathTo(value);
            }
        }

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
        public CoordinateType CoordinateType { get; set; }

        /// <summary>
        /// DataTable is the System.Data.DataTable for all the attributes of this FeatureSet.
        /// This will call FillAttributes if it is accessed and that has not yet been called.
        /// </summary>
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
                if (MyExtent == null)
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
        /// created.  The "Feature" prefix allows us to access the static Default instance on GeometryFactory.
        /// </summary>
        public IGeometryFactory FeatureGeometryFactory { get; set; }

        /// <summary>
        /// Gets the feature lookup Table itself.
        /// </summary>
        public Dictionary<DataRow, IFeature> FeatureLookup
        {
            get
            {
                return _featureLookup;
            }
        }

        /// <summary>
        /// Gets or sets an enumeration specifying whether this
        /// featureset contains Lines, Points, Polygons or an
        /// unspecified type.
        /// </summary>
        public FeatureType FeatureType { get; set; }

        /// <summary>
        /// A list of the features in this layer
        /// </summary>
        public virtual IFeatureList Features
        {
            get
            {
                if (IndexMode && (_features == null || _features.Count == 0))
                {
                    // People working with features like this probably want to see changes from the features themselves.
                    FeaturesFromVertices();
                    IndexMode = false;
                }

                return _features;
            }

            set
            {
                OnIncludeFeatures(_features);
                OnExcludeFeatures(_features);
                _features = value;
            }
        }

        /// <summary>
        /// Gets or sets the file name of a file based feature set. The file name should be the
        /// absolute path including the file extension. For feature sets coming from a database
        /// or a web service, the Filename property is NULL.
        /// </summary>
        public virtual string Filename { get; set; }

        /// <summary>
        /// If this is true, then the ShapeIndices and Vertex values are used,
        /// and features are created on demand.  Otherwise the list of Features
        /// is used directly.
        /// </summary>
        public bool IndexMode { get; set; }

        /// <inheritdoc/>
        public double[] M
        {
            get
            {
                if (_verticesAreValid == false)
                {
                    OnInitializeVertices();
                }

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
        /// To impact those, use the
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
            if (IndexMode == false)
            {
                foreach (int index in indices)
                {
                    if (index < 0 || index >= _shapeIndices.Count)
                        continue;
                    Features.RemoveAt(index);
                }
                InitializeVertices();
                return;
            }

            List<int> remove = indices.ToList();
            remove.Sort();
            if (remove.Count == 0)
                return;
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
            remove = indices.ToList();
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

            remove = indices.ToList();
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
            {
                UpdateCoordinates();
            }

            foreach (ShapeRange shape in ShapeIndices)
            {
                foreach (PartRange part in shape.Parts)
                {
                    part.Vertices = Vertex;
                }
            }

            UpdateExtent();
            Projection = targetProjection;
        }

        /// <summary>
        /// These specifically allow the user to make sense of the Vertices array.  These are
        /// fast acting sealed classes and are not meant to be overridden or support clever
        /// new implementations.
        /// </summary>
        public List<ShapeRange> ShapeIndices
        {
            get
            {
                if (_shapeIndices == null)
                {
                    OnInitializeVertices();
                }

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
                    MyExtent = new Extent(-180, -90, 180, 90);
                    return;
                }

                foreach (IFeature feature in Features)
                {
                    feature.UpdateEnvelope();
                    MyExtent.ExpandToInclude(new Extent(feature.Envelope));
                }
            }
            else
            {
                if (_shapeIndices == null || _shapeIndices.Count == 0)
                {
                    MyExtent = new Extent(-180, -90, 180, 90);
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
                if (_verticesAreValid == false)
                {
                    OnInitializeVertices();
                }

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
        public bool VerticesAreValid
        {
            get
            {
                return _verticesAreValid;
            }
        }

        /// <inheritdoc/>
        public double[] Z
        {
            get
            {
                if (_verticesAreValid == false)
                {
                    OnInitializeVertices();
                }

                return _z;
            }

            set
            {
                _z = value;
            }
        }

        private static string AbsolutePathTo(string toPath)
        {
            if (String.IsNullOrEmpty(toPath))
                throw new ArgumentNullException("toPath");

            return Path.GetFullPath(toPath);
        }

        /// <summary>
        /// Creates a relative path from one file
        /// or folder to another.
        /// </summary>
        /// <param name="toPath">
        /// Contains the path that defines the
        /// endpoint of the relative path.
        /// </param>
        /// <returns>
        /// The relative path from the start
        /// directory to the end path.
        /// </returns>
        /// <exception cref="ArgumentNullException">Occurs when the toPath is NULL</exception>
        //http://weblogs.asp.net/pwelter34/archive/2006/02/08/create-a-relative-path-code-snippet.aspx
        public static string RelativePathTo(string toPath)
        {
            string fromDirectory = Directory.GetCurrentDirectory();

            if (toPath == null)
                throw new ArgumentNullException("toPath");

            bool isRooted = Path.IsPathRooted(fromDirectory)
                            && Path.IsPathRooted(toPath);

            if (isRooted)
            {
                bool isDifferentRoot = string.Compare(
                    Path.GetPathRoot(fromDirectory),
                    Path.GetPathRoot(toPath), true) != 0;

                if (isDifferentRoot)
                    return toPath;
            }

            StringCollection relativePath = new StringCollection();
            string[] fromDirectories = fromDirectory.Split(
                Path.DirectorySeparatorChar);

            string[] toDirectories = toPath.Split(
                Path.DirectorySeparatorChar);

            int length = Math.Min(
                fromDirectories.Length,
                toDirectories.Length);

            int lastCommonRoot = -1;

            // find common root
            for (int x = 0; x < length; x++)
            {
                if (string.Compare(fromDirectories[x],
                                   toDirectories[x], true) != 0)
                    break;

                lastCommonRoot = x;
            }
            if (lastCommonRoot == -1)
                return toPath;

            // add relative folders in from path
            for (int x = lastCommonRoot + 1; x < fromDirectories.Length; x++)
                if (fromDirectories[x].Length > 0)
                    relativePath.Add("..");

            // add to folders to path
            for (int x = lastCommonRoot + 1; x < toDirectories.Length; x++)
                relativePath.Add(toDirectories[x]);

            // create relative path
            string[] relativeParts = new string[relativePath.Count];
            relativePath.CopyTo(relativeParts, 0);

            string newPath = string.Join(
                Path.DirectorySeparatorChar.ToString(),
                relativeParts);

            return newPath;
        }

        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string MakeRelativePath(string fromPath, string toPath)
        {
            if (String.IsNullOrEmpty(fromPath))
                throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath))
                throw new ArgumentNullException("toPath");

            if (Path.GetDirectoryName(toPath) == String.Empty)
                // it looks like we only have a file name.
                return toPath;

            if (!fromPath.EndsWith(@"\"))
                fromPath += @"\";

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);

            return relativeUri.ToString();
        }

        /// <summary>
        /// Calculates the features from the shape indices and vertex array
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
            }

            _features.SuspendEvents();
            for (int shp = 0; shp < ShapeIndices.Count; shp++)
            {
                _features.Add(GetFeature(shp));
                if (AttributesPopulated)
                {
                    // Don't force population if we haven't populated yet, but
                    // definitely assign the DataRow if it already exists.
                    _features[shp].DataRow = DataTable.Rows[shp];
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
            // overridden in sub-classes
            DataTable result = new DataTable();
            DataColumn[] columns = GetColumns();

            // Always add FID in this paging scenario.
            bool hasFid = false;
            foreach (DataColumn col in columns)
            {
                if (col.ColumnName == "FID")
                {
                    hasFid = true;
                }
            }

            if (!hasFid)
            {
                result.Columns.Add("FID", typeof(int));
            }

            int i = 0;
            for (int row = startIndex; i < numRows; row++)
            {
                DataRow myRow = result.NewRow();
                myRow["FID"] = row;
                foreach (DataColumn col in columns)
                {
                    if (col.ColumnName == "FID")
                    {
                        continue; // prefer our own FID here for indexing.
                    }

                    myRow[col.ColumnName] = _dataTable.Rows[row][col.ColumnName];
                }

                result.Rows.Add(myRow);
                i++;
            }

            return result;
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
            DataTable result = new DataTable();
            DataColumn[] columns = GetColumns();

            // Always add FID in this paging scenario.  This is for the in-ram case.  A more appropriate
            // implementation exists
            result.Columns.Add("FID", typeof(int));
            foreach (string name in fieldNames)
            {
                foreach (var col in columns)
                {
                    if (col.ColumnName == name)
                    {
                        result.Columns.Add(col);
                        break;
                    }
                }
            }

            int i = 0;
            for (int row = startIndex; i < numRows; row++)
            {
                DataRow myRow = result.NewRow();
                myRow["FID"] = row;
                foreach (string name in fieldNames)
                {
                    if (name == "FID")
                    {
                        continue;
                    }

                    myRow[name] = _dataTable.Rows[row][name];
                }

                result.Rows.Add(myRow);
                i++;
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

            return;
        }

        #endregion

        /// <summary>
        /// Occurs when the vertices are being re-calculated.
        /// </summary>
        protected virtual void OnInitializeVertices()
        {
            int count = _features.Sum(f => f.NumPoints);
            _vertices = new double[count * 2];
            int i = 0;
            foreach (IFeature f in _features)
            {
                // this should be all the coordinates, for all parts of the geometry.
                IList<Coordinate> coords = f.Coordinates;

                if (coords == null)
                {
                    continue;
                }

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
                ShapeRange shx = new ShapeRange(FeatureType) { Extent = new Extent(f.Envelope) };
                _shapeIndices.Add(shx);
                f.ShapeIndex = shx;

                // for simplicity in looping, there is always at least one part.
                // That way, the shape range can be ignored and the parts loop used instead.
                shx.Parts = new List<PartRange>();
                int shapeStart = vIndex;
                for (int part = 0; part < f.NumGeometries; part++)
                {
                    PartRange prtx = new PartRange(_vertices, shapeStart, vIndex - shapeStart, FeatureType);
                    IBasicPolygon bp = f.GetBasicGeometryN(part) as IBasicPolygon;
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
                        int numPoints = f.GetBasicGeometryN(part).NumPoints;

                        // This is not a polygon, so just add the number of points.
                        vIndex += numPoints;
                        prtx.NumVertices = numPoints;
                    }

                    shx.Parts.Add(prtx);
                }
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
                VerticesInvalidated(this, new EventArgs());
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
                IList<Coordinate> coords = f.Coordinates;

                if (coords == null)
                {
                    continue;
                }

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
                _featureLookup = null;
                _features = null;
                Filename = null;
                _m = null;
                _shapeIndices = null;
                _vertices = null;
                _z = null;
            }
            if (_dataTable != null)
                _dataTable.Dispose();
            GC.Collect();
            base.Dispose(disposeManagedResources);
        }
    }
}