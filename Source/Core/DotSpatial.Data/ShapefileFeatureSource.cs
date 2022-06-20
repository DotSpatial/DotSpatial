// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Data;
using System.IO;
using DotSpatial.NTSExtension;
using NetTopologySuite.Geometries;
using NetTopologySuite.Index;

namespace DotSpatial.Data
{
    /// <summary>
    /// Provides common functionality for all ShapefileFeatureSource classes (Point, Line, Polygon).
    /// </summary>
    public abstract class ShapefileFeatureSource
    {
        #region Fields

        private readonly bool _trackDeletedRows;
        private List<int> _deletedRows;
        private string _fileName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileFeatureSource"/> class from the specified file.
        /// </summary>
        /// <param name="fileName">The fileName to work with.</param>
        protected ShapefileFeatureSource(string fileName)
            : this(fileName, false, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileFeatureSource"/> class from the specified file and builds spatial index if requested.
        /// </summary>
        /// <param name="fileName">The fileName to work with.</param>
        /// <param name="useSpatialIndexing">Indicates whether the spatial index should be build.</param>
        /// <param name="trackDeletedRows">Indicates whether deleted records should be tracked.</param>
        protected ShapefileFeatureSource(string fileName, bool useSpatialIndexing, bool trackDeletedRows)
        {
            Filename = fileName;
            if (useSpatialIndexing)
            {
                InitializeQuadtree();
            }

            _trackDeletedRows = trackDeletedRows;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the AttributeTable  for this feature source including DeletedRows.
        /// </summary>
        public AttributeTable AttributeTable => GetAttributeTable(Filename);

        /// <summary>
        /// Gets the geographic extent of the feature source.
        /// </summary>
        public Extent Extent
        {
            get
            {
                var sh = new ShapefileHeader(Filename);
                if (sh.ShxLength == 50) return new Extent(double.NaN, 0, 0, 0);

                return sh.ToExtent();
            }
        }

        /// <summary>
        /// Gets the feature type supported by this feature source.
        /// </summary>
        public abstract FeatureType FeatureType { get; }

        /// <summary>
        /// Gets or sets the absolute Filename of this ShapefileFeatureSource. If a relative path gets assigned it is changed to the absolute path including the file extension.
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
        /// Gets or sets the integer maximum number of records to return in a single page of results.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the shape type (without M or Z) supported by this feature source.
        /// </summary>
        public abstract ShapeType ShapeType { get; }

        /// <summary>
        /// Gets the shape type (with M, and no Z) supported by this feature source.
        /// </summary>
        public abstract ShapeType ShapeTypeM { get; }

        /// <summary>
        /// Gets the shape type (with M and Z) supported by this feature source.
        /// </summary>
        public abstract ShapeType ShapeTypeZ { get; }

        /// <summary>
        /// Gets the spatial index if any.
        /// </summary>
        public ISpatialIndex<int> SpatialIndex => Quadtree;

        /// <summary>
        /// Gets or sets the spatial index for faster searches.
        /// </summary>
        protected ShapefileFeatureSourceQuadtree Quadtree { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a ShapefileFeatureSource of the appropriate type.
        /// </summary>
        /// <param name="filename">Name of the file that gets opened.</param>
        /// <returns>A ShapefileFeatureSource loaded from the specified file.</returns>
        public static ShapefileFeatureSource Open(string filename)
        {
            return Open(filename, false, false);
        }

        /// <summary>
        /// Create a ShapefileFeatureSource of the appropriate type spatial indexing.
        /// </summary>
        /// <param name="filename">Name of the file that gets opened.</param>
        /// <param name="useSpatialIndexing">Indicates whether a spatial index should be used.</param>
        /// <param name="trackDeletedRows">Indicates whether deleted rows should be tracked.</param>
        /// <returns>A ShapefileFeatureSource loaded from the specified file.</returns>
        public static ShapefileFeatureSource Open(string filename, bool useSpatialIndexing, bool trackDeletedRows)
        {
            var header = new ShapefileHeader(filename);

            return header.ShapeType switch
            {
                ShapeType.Polygon or ShapeType.PolygonM or ShapeType.PolygonZ => new PolygonShapefileFeatureSource(filename, useSpatialIndexing, trackDeletedRows),
                ShapeType.PolyLine or ShapeType.PolyLineM or ShapeType.PolyLineZ => new LineShapefileFeatureSource(filename, useSpatialIndexing, trackDeletedRows),
                ShapeType.Point or ShapeType.PointM or ShapeType.PointZ => new PointShapefileFeatureSource(filename, useSpatialIndexing, trackDeletedRows),
                _ => throw new ClassNotSupportedException($"Cannot create ShapefileFeatureSource for {header.ShapeType} shape type"),// TODO jany_ why use classNotSupportedException if message not a class name?  resulting Errormessage makes no sense
            };
        }

        /// <summary>
        /// Updates the file with an additional feature.
        /// </summary>
        /// <param name="feature">Feature that gets added.</param>
        public void Add(IFeature feature)
        {
            if (feature.FeatureType != FeatureType)
            {
                throw new FeatureTypeMismatchException();
            }

            string dir = Path.GetDirectoryName(Filename);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // We must add the dbf entry before changing the shx because if we already have one deleted record, the AttributeTable thinks we have none
            AttributeTable dbf = GetAttributeTable(Filename);
            dbf.AddRow(feature.DataRow);

            int numFeatures = 0;
            var header = new ShapefileHeader();
            if (File.Exists(Filename))
            {
                header.Open(Filename);
                UpdateHeader(header, feature.Geometry);
                numFeatures = (header.ShxLength - 50) / 4;
            }
            else
            {
                header.Xmin = feature.Geometry.EnvelopeInternal.MinX;
                header.Xmax = feature.Geometry.EnvelopeInternal.MaxX;
                header.Ymin = feature.Geometry.EnvelopeInternal.MinY;
                header.Ymax = feature.Geometry.EnvelopeInternal.MaxY;
                if (double.IsNaN(feature.Geometry.Coordinates[0].M))
                {
                    header.ShapeType = ShapeType;
                }
                else
                {
                    if (double.IsNaN(feature.Geometry.Coordinates[0].Z))
                    {
                        header.ShapeType = ShapeTypeM;
                    }
                    else
                    {
                        header.Zmin = feature.Geometry.MinZ();
                        header.Zmax = feature.Geometry.MaxZ();
                        header.ShapeType = ShapeTypeZ;
                    }

                    header.Mmin = feature.Geometry.MinM();
                    header.Mmax = feature.Geometry.MaxM();
                }

                header.ShxLength = 4 + 50;
                header.SaveAs(Filename);
            }

            AppendGeometry(header, feature.Geometry, numFeatures);

            Quadtree?.Insert(feature.Geometry.EnvelopeInternal, numFeatures - 1);
        }

        /// <summary>
        /// Adds the given features to the file.
        /// </summary>
        /// <param name="features">The set of features to add.</param>
        public void AddRange(IEnumerable<IFeature> features)
        {
            // Make sure the Output Directory exists
            string dir = Path.GetDirectoryName(Filename);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // Get the Attribute Table
            AttributeTable dbf = GetAttributeTable(Filename);

            // Open the Header if the Filename exists
            bool filenameExists = File.Exists(Filename);
            var header = new ShapefileHeader();
            if (filenameExists)
            {
                header.Open(Filename);
            }

            foreach (var feature in features)
            {
                if (feature.FeatureType != FeatureType)
                {
                    throw new FeatureTypeMismatchException();
                }

                // We must add the dbf entry before changing the shx because if we already have one deleted record, the AttributeTable thinks we have none
                dbf.AddRow(feature.DataRow);

                int numFeatures = 0;
                if (filenameExists)
                {
                    UpdateHeader(header, feature.Geometry, true);
                    numFeatures = (header.ShxLength - 50) / 4;
                }
                else
                {
                    header.Xmin = feature.Geometry.EnvelopeInternal.MinX;
                    header.Xmax = feature.Geometry.EnvelopeInternal.MaxX;
                    header.Ymin = feature.Geometry.EnvelopeInternal.MinY;
                    header.Ymax = feature.Geometry.EnvelopeInternal.MaxY;
                    if (double.IsNaN(feature.Geometry.Coordinates[0].M))
                    {
                        header.ShapeType = ShapeType;
                    }
                    else
                    {
                        if (double.IsNaN(feature.Geometry.Coordinates[0].Z))
                        {
                            header.ShapeType = ShapeTypeM;
                        }
                        else
                        {
                            header.Zmin = feature.Geometry.MinZ();
                            header.Zmax = feature.Geometry.MaxZ();
                            header.ShapeType = ShapeTypeZ;
                        }

                        header.Mmin = feature.Geometry.MinM();
                        header.Mmax = feature.Geometry.MaxM();
                    }

                    header.ShxLength = 4 + 50;
                    header.SaveAs(Filename);
                    filenameExists = true;
                }

                AppendGeometry(header, feature.Geometry, numFeatures);

                Quadtree?.Insert(feature.Geometry.EnvelopeInternal, numFeatures - 1);
            }
        }

        /// <summary>
        /// Create an appropriate ShapeSource for this FeatureSource.
        /// </summary>
        /// <returns>The created IShapeSource.</returns>
        public abstract IShapeSource CreateShapeSource();

        /// <summary>
        /// This deletes the files with any extension and the same base fileName.
        /// </summary>
        public void Delete()
        {
            if (File.Exists(Filename)) File.Delete(Filename);
            string dir = Path.GetDirectoryName(Filename);
            if (dir == null) return;

            string baseName = Path.GetFileNameWithoutExtension(Filename);
            string[] files = Directory.GetFiles(dir, baseName + ".*");
            foreach (string file in files)
            {
                if (File.Exists(file)) File.Delete(file);
            }
        }

        /// <summary>
        /// Edits the values of the specified row in the attribute table. Since not all data formats
        /// can handle the dynamic change of attributes, updating vectors can be done with Remove and Add.
        /// </summary>
        /// <param name="fid">The feature offest.</param>
        /// <param name="attributeValues">The row of new attribute values.</param>
        public void EditAttributes(int fid, DataRow attributeValues)
        {
            AttributeTable at = GetAttributeTable(Filename);
            at.Edit(fid, attributeValues);
        }

        /// <summary>
        /// Edits the values of the specified rows in the attribute table.
        /// </summary>
        /// <param name="indexDataRowPairs">IEnumerable of rows that should be edited and their corresponding indices.</param>
        public void EditAttributes(IEnumerable<KeyValuePair<int, DataRow>> indexDataRowPairs)
        {
            AttributeTable at = GetAttributeTable(Filename);
            at.Edit(indexDataRowPairs);
        }

        /// <summary>
        /// Removes the feature with the specified index.
        /// </summary>
        /// <param name="index">Removes the feature from the specified index location.</param>
        public void RemoveAt(int index)
        {
            // Get shape range so we can update header smartly
            int startIndex = index;
            IFeatureSet ifs = Select(null, null, ref startIndex, 1);
            if (ifs.NumRows() > 0)
            {
                var shx = new ShapefileIndexFile();
                shx.Open(Filename);
                shx.Shapes.RemoveAt(index);
                shx.Save();

                AttributeTable dbf = GetAttributeTable(Filename);
                dbf.RemoveRowAt(index);
                if (_trackDeletedRows) _deletedRows = dbf.DeletedRows;

                // Update extent in header if feature being deleted is NOT completely contained
                var hdr = new ShapefileHeader(Filename);

                Envelope featureEnv = ifs.GetFeature(0).Geometry.EnvelopeInternal;
                if (featureEnv.MinX <= hdr.Xmin || featureEnv.MaxX >= hdr.Xmax || featureEnv.MaxY >= hdr.Ymax || featureEnv.MinY <= hdr.Ymin)
                {
                    UpdateExtents();
                }

                // Update the Quadtree
                Quadtree?.Remove(featureEnv, index);
            }
        }

        /// <summary>
        /// Conditionally modify attributes as searched in a single pass via client supplied callback.
        /// </summary>
        /// <param name="envelope">The envelope for vector filtering.</param>
        /// <param name="chunkSize">Number of shapes to request from the ShapeSource in each chunk.</param>
        /// <param name="rowCallback">Callback on each feature.</param>
        public abstract void SearchAndModifyAttributes(Envelope envelope, int chunkSize, FeatureSourceRowEditEvent rowCallback);

        /// <summary>
        /// Select function passed with a filter expression.
        /// </summary>
        /// <param name="filterExpression">The string filter expression based on attributes.</param>
        /// <param name="envelope">The envelope for vector filtering.</param>
        /// <param name="startIndex">The integer start index where values should be checked. This will be updated to the. </param>
        /// last index that was checked before the return, so that paging the query can begin with that index in the next cycle.
        /// Be sure to add one before the next call.
        /// <param name="maxCount">The integer maximum number of IFeature values that should be returned.</param>
        /// <returns>A dictionary with FID keys and IFeature values.</returns>
        public abstract IFeatureSet Select(string filterExpression, Envelope envelope, ref int startIndex, int maxCount);

        /// <summary>
        /// Adding and removing shapes may make the bounds for the entire shapefile invalid.
        /// This triggers a check that ensures that the collective extents contain all the shapes.
        /// </summary>
        public abstract void UpdateExtents();

        /// <summary>
        /// Append the geometry to the shapefile.
        /// </summary>
        /// <param name="header">ShapefileHeader of this file.</param>
        /// <param name="geometry">Geometry that gets appended.</param>
        /// <param name="numFeatures">Number of the features in the shapefile including the one getting appended.</param>
        protected abstract void AppendGeometry(ShapefileHeader header, Geometry geometry, int numFeatures);

        /// <summary>
        /// Conditionally modify attributes as searched in a single pass via client supplied callback.
        /// </summary>
        /// <param name="sr">ShapeSource for this feature source.</param>
        /// <param name="envelope">The envelope for vector filtering.</param>
        /// <param name="chunkSize">Number of shapes to request from the ShapeSource in each chunk.</param>
        /// <param name="featureEditCallback">Callback on each feature.</param>
        protected void SearchAndModifyAttributes(IShapeSource sr, Envelope envelope, int chunkSize, FeatureSourceRowEditEvent featureEditCallback)
        {
            var samp = new ShapefileFeatureSourceSearchAndModifyAttributeParameters(featureEditCallback);
            AttributeTable at = null;
            int startIndex = 0;
            do
            {
                samp.CurrentSearchAndModifyAttributeShapes = sr.GetShapes(ref startIndex, chunkSize, envelope);
                if (samp.CurrentSearchAndModifyAttributeShapes.Count > 0)
                {
                    if (at == null) at = GetAttributeTable(Filename);

                    at.Edit(samp.CurrentSearchAndModifyAttributeShapes.Keys, samp.OnRowEditEvent);
                }
            }
            while (samp.CurrentSearchAndModifyAttributeShapes.Count == chunkSize);
        }

        /// <summary>
        /// Select the features in an IShapeSource.
        /// </summary>
        /// <param name="shapeSource">ShapeSource that contains the shapes.</param>
        /// <param name="filterExpression">Expression that is used to filter the attributes.</param>
        /// <param name="envelope">The geographic extents that can be used to limit the shapes. If this is null, then no envelope is used.</param>
        /// <param name="startIndex">The integer offset of the first shape to test. When this returns, the offset is set to the integer index of the last shape tested, regardless of whether or not it was returned.</param>
        /// <param name="maxCount">The integer count of the maximum number of shapes to return here. </param>
        /// <returns>An IFeatureSet containing the selected features.</returns>
        protected IFeatureSet Select(IShapeSource shapeSource, string filterExpression, Envelope envelope, ref int startIndex, int maxCount)
        {
            var shapes = shapeSource.GetShapes(ref startIndex, maxCount, envelope);
            AttributeTable at = GetAttributeTable(Filename);
            var result = new FeatureSet(FeatureType.Polygon);
            bool schemaDefined = false;
            foreach (var pair in shapes)
            {
                DataTable td = at.SupplyPageOfData(pair.Key, 1);
                if (td.Select(filterExpression).Length > 0)
                {
                    if (!schemaDefined)
                    {
                        schemaDefined = true;
                        result.CopyTableSchema(td);
                    }

                    IFeature f = new Feature(pair.Value)
                    {
                        DataRow = td.Rows[0]
                    };
                    result.Features.Add(f);
                    f.UpdateEnvelope();
                }
            }

            return result;
        }

        /// <summary>
        /// Update header extents from the geometries in a IShapeSource.
        /// </summary>
        /// <param name="shapeSource">IShapeSource that is used to update the header extents.</param>
        protected void UpdateExtents(IShapeSource shapeSource)
        {
            var sr = new ShapeReader(shapeSource);
            Extent env = null;

            foreach (var page in sr)
            {
                foreach (var shape in page)
                {
                    Extent shapeExtent = shape.Value.Range.Extent;
                    if (env == null)
                    {
                        env = (Extent)shapeExtent.Clone();
                    }
                    else
                    {
                        env.ExpandToInclude(shapeExtent);
                    }
                }
            }

            // Extents for an empty file are unspecified according to the specification, so do nothing
            if (env == null) return;

            var sh = new ShapefileHeader(Filename);
            sh.SetExtent(env);
            sh.Save();
        }

        /// <summary>
        /// Updates the header to include the feature extent.
        /// </summary>
        /// <param name="header">Header that gets updated.</param>
        /// <param name="feature">Feature, whose extent gets include into the header extent. </param>
        protected void UpdateHeader(ShapefileHeader header, Geometry feature)
        {
            UpdateHeader(header, feature, true);
        }

        /// <summary>
        /// Updates the header to include the feature extent.
        /// </summary>
        /// <param name="header">Header that gets updated.</param>
        /// <param name="feature">Feature, whose extent gets include into the header extent. </param>
        /// <param name="writeHeaderFiles">Indicates whether the headers should be written to file.</param>
        protected void UpdateHeader(ShapefileHeader header, Geometry feature, bool writeHeaderFiles)
        {
            // Update the envelope
            Envelope newExt;

            // First, check to see if there are no features (ShxLength == 50)
            if (header.ShxLength <= 50)
            {
                // This is the lone feature, so just set the extent to the feature extent
                newExt = feature.EnvelopeInternal;
            }
            else
            {
                // Other features, so include new feature
                newExt = new Envelope(header.Xmin, header.Xmax, header.Ymin, header.Ymax);
                newExt.ExpandToInclude(feature.EnvelopeInternal);
            }

            header.Xmin = newExt.MinX;
            header.Ymin = newExt.MinY;
            header.Xmax = newExt.MaxX;
            header.Ymax = newExt.MaxY;

            if (header.ShapeType == ShapeTypeM)
            {
                if (feature.MinM() < header.Mmin)
                {
                    header.Mmin = feature.MinM();
                }
            }

            if (header.ShapeType == ShapeTypeM || header.ShapeType == ShapeTypeZ)
            {
                if (feature.MinZ() < header.Zmin)
                {
                    header.Zmin = feature.MinZ();
                }
            }

            header.ShxLength += 4;
            if (writeHeaderFiles)
            {
                WriteHeader(header, Filename);
                WriteHeader(header, header.ShxFilename);
            }
        }

        /// <summary>
        /// Writes the header to the given file.
        /// </summary>
        /// <param name="header">Header that gets written.</param>
        /// <param name="fileName">File whose header should be written.</param>
        private static void WriteHeader(ShapefileHeader header, string fileName)
        {
            string dir = Path.GetDirectoryName(fileName);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Write, FileShare.None, 100);
            fs.WriteBe(header.FileCode); ////  Byte 0          File Code       9994        Integer     Big

            var bt = new byte[20];
            fs.Write(bt, 0, 20); ////  Bytes 4 - 20 are unused

            // This is overwritten later
            fs.WriteBe(0); //  Byte 24         File Length     File Length Integer     Big
            fs.WriteLe(header.Version); //  Byte 28         Version         1000        Integer     Little
            fs.WriteLe((int)header.ShapeType); //  Byte 32         Shape Type      Shape Type  Integer     Little
            fs.WriteLe(header.Xmin); //  Byte 36         Bounding Box    Xmin        Double      Little
            fs.WriteLe(header.Ymin); //  Byte 44         Bounding Box    Ymin        Double      Little
            fs.WriteLe(header.Xmax); //  Byte 52         Bounding Box    Xmax        Double      Little
            fs.WriteLe(header.Ymax); //  Byte 60         Bounding Box    Ymax        Double      Little
            fs.WriteLe(header.Zmin); //  Byte 68         Bounding Box    Zmin        Double      Little
            fs.WriteLe(header.Zmax); //  Byte 76         Bounding Box    Zmax        Double      Little
            fs.WriteLe(header.Mmin); //  Byte 84         Bounding Box    Mmin        Double      Little
            fs.WriteLe(header.Mmax); //  Byte 92         Bounding Box    Mmax        Double      Little
            fs.Close();
        }

        private AttributeTable GetAttributeTable(string fileName)
        {
            var at = new AttributeTable();
            at.Open(fileName, _deletedRows);
            if (_trackDeletedRows) _deletedRows = at.DeletedRows;
            return at;
        }

        private void InitializeQuadtree()
        {
            var qt = new ShapefileFeatureSourceQuadtree();
            var sfs = CreateShapeSource();
            int nIndex = 0;
            int returnedCount;
            do
            {
                Dictionary<int, Shape> shapes = sfs.GetShapes(ref nIndex, 1000, null);
                foreach (KeyValuePair<int, Shape> shape in shapes)
                {
                    qt.Insert(shape.Value.Range.Extent.ToEnvelope(), shape.Key);
                }

                returnedCount = shapes.Count;
            }
            while (returnedCount == 1000);

            Quadtree = qt;
        }

        #endregion
    }
}