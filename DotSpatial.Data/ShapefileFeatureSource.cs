// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial
//
// The Initial Developer of this Original Code is Kyle Ellison. Created 12/02/2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|----------|---------------------------------------------------------------------
// |      Name       |  Date    |                        Comments
// |-----------------|----------|----------------------------------------------------------------------
// | Kyle Ellison    |12/08/2010| Added ability to edit multiple rows in one call for performance
// | Kyle Ellison    |12/09/2010| Added Extent property
// | Kyle Ellison    |12/22/2010| Made FeatureType and ShapeType properties public, added static Open method,
// |                 |          | and return 'Empty' Extent if file is empty.
// |-----------------|----------|----------------------------------------------------------------------
// ********************************************************************************************************

using System.Collections.Generic;
using System.Data;
using System.IO;
using DotSpatial.Topology;
using DotSpatial.Topology.Index;

namespace DotSpatial.Data
{
    ///<summary>
    /// Provides common functionality for all ShapefileFeatureSource classes (Point, Line, Polygon)
    ///</summary>
    public abstract class ShapefileFeatureSource : IFeatureSource
    {
        #region Constructors

        /// <summary>
        /// Sets the fileName and creates a new ShapefileFeatureSource for the specified file.
        /// </summary>
        /// <param name="fileName">The fileName to work with.</param>
        protected ShapefileFeatureSource(string fileName)
            : this(fileName, false, false)
        {
        }

        /// <summary>
        /// Sets the fileName and creates a new PolygonshapefileFeatureSource for the specified file (and builds spatial index if requested)
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="useSpatialIndexing"></param>
        /// <param name="trackDeletedRows"></param>
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

        #region Public Properties

        /// <summary>
        /// Gets or sets the Filename of this polygon shapefile.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// The integer maximum number of records to return in a single page of results.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the spatial index if any
        /// </summary>
        public ISpatialIndex SpatialIndex
        {
            get { return Quadtree; }
        }

        #endregion

        #region Public Methods

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

        #endregion

        private readonly bool _trackDeletedRows;

        /// <summary>
        /// Spatial index for faster searches
        /// </summary>
        protected ShapefileFeatureSourceQuadtree Quadtree;

        private List<int> _deletedRows;

        /// <summary>
        /// Get the feature type supported by this feature source
        /// </summary>
        public abstract FeatureType FeatureType { get; }

        /// <summary>
        /// Get the shape type (without M or Z) supported by this feature source
        /// </summary>
        public abstract ShapeType ShapeType { get; }

        /// <summary>
        /// Get the shape type (with M, and no Z) supported by this feature source
        /// </summary>
        public abstract ShapeType ShapeTypeM { get; }

        /// <summary>
        /// Get the shape type (with M and Z) supported by this feature source
        /// </summary>
        public abstract ShapeType ShapeTypeZ { get; }

        #region Implementation of IFeatureSource

        /// <inheritdocs/>
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
                UpdateHeader(header, feature);
                numFeatures = (header.ShxLength - 50) / 4;
            }
            else
            {
                header.Xmin = feature.Envelope.Minimum.X;
                header.Xmax = feature.Envelope.Maximum.X;
                header.Ymin = feature.Envelope.Minimum.Y;
                header.Ymax = feature.Envelope.Maximum.Y;
                if (double.IsNaN(feature.Coordinates[0].M))
                {
                    header.ShapeType = ShapeType;
                }
                else
                {
                    if (double.IsNaN(feature.Coordinates[0].Z))
                    {
                        header.ShapeType = ShapeTypeM;
                    }
                    else
                    {
                        header.Zmin = feature.Envelope.Minimum.Z;
                        header.Zmax = feature.Envelope.Maximum.Z;
                        header.ShapeType = ShapeTypeZ;
                    }
                    header.Mmin = feature.Envelope.Minimum.M;
                    header.Mmax = feature.Envelope.Maximum.M;
                }
                header.ShxLength = 4 + 50;
                header.SaveAs(Filename);
            }

            AppendBasicGeometry(header, feature, numFeatures);

            feature.RecordNumber = numFeatures;

            if (null != Quadtree)
                Quadtree.Insert(feature.Envelope, numFeatures - 1);
        }

        /// <inheritdocs/>
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
                if (_trackDeletedRows)
                    _deletedRows = dbf.DeletedRows;

                // Update extent in header if feature being deleted is NOT completely contained
                var hdr = new ShapefileHeader(Filename);

                IEnvelope featureEnv = ifs.GetFeature(0).Envelope;
                if (featureEnv.Left() <= hdr.Xmin || featureEnv.Right() >= hdr.Xmax || featureEnv.Top() >= hdr.Ymax || featureEnv.Bottom() <= hdr.Ymin)
                {
                    UpdateExtents();
                }

                // Update the Quadtree
                if (null != Quadtree)
                {
                    Quadtree.Remove(featureEnv, index);
                }
            }
        }

        /// <inheritdocs/>
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
                    UpdateHeader(header, feature, true);
                    numFeatures = (header.ShxLength - 50) / 4;
                }
                else
                {
                    header.Xmin = feature.Envelope.Minimum.X;
                    header.Xmax = feature.Envelope.Maximum.X;
                    header.Ymin = feature.Envelope.Minimum.Y;
                    header.Ymax = feature.Envelope.Maximum.Y;
                    if (double.IsNaN(feature.Coordinates[0].M))
                    {
                        header.ShapeType = ShapeType;
                    }
                    else
                    {
                        if (double.IsNaN(feature.Coordinates[0].Z))
                        {
                            header.ShapeType = ShapeTypeM;
                        }
                        else
                        {
                            header.Zmin = feature.Envelope.Minimum.Z;
                            header.Zmax = feature.Envelope.Maximum.Z;
                            header.ShapeType = ShapeTypeZ;
                        }
                        header.Mmin = feature.Envelope.Minimum.M;
                        header.Mmax = feature.Envelope.Maximum.M;
                    }
                    header.ShxLength = 4 + 50;
                    header.SaveAs(Filename);
                    filenameExists = true;
                }

                AppendBasicGeometry(header, feature, numFeatures);

                feature.RecordNumber = numFeatures;

                if (null != Quadtree)
                    Quadtree.Insert(feature.Envelope, numFeatures - 1);
            }
        }

        /// <inheritdocs/>
        public abstract IFeatureSet Select(string filterExpression, IEnvelope envelope, ref int startIndex, int maxCount);

        /// <inheritdocs/>
        public abstract void SearchAndModifyAttributes(IEnvelope envelope, int chunkSize, FeatureSourceRowEditEvent rowCallback);

        /// <inheritdocs/>
        public void EditAttributes(int fid, DataRow attributeValues)
        {
            AttributeTable at = GetAttributeTable(Filename);
            at.Edit(fid, attributeValues);
        }

        /// <inheritdocs/>
        public void EditAttributes(IEnumerable<KeyValuePair<int, DataRow>> indexDataRowPairs)
        {
            AttributeTable at = GetAttributeTable(Filename);
            at.Edit(indexDataRowPairs);
        }

        /// <inheritdocs/>
        public abstract void UpdateExtents();

        /// <inheritdocs/>
        public Extent Extent
        {
            get
            {
                var sh = new ShapefileHeader(Filename);
                if (sh.ShxLength == 50)
                    return new Extent(double.NaN, 0, 0, 0);
                return sh.ToExtent();
            }
        }

        /// <summary>
        /// Gets the AttributeTable  for this feature source including DeletedRows
        /// </summary>
        public AttributeTable AttributeTable
        {
            get { return GetAttributeTable(Filename); }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Create a ShapefileFeatureSource of the appropriate type
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static ShapefileFeatureSource Open(string filename)
        {
            return Open(filename, false, false);
        }

        /// <summary>
        /// Create a ShapefileFeatureSource of the appropriate type spatial indexing
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="useSpatialIndexing"></param>
        /// <param name="trackDeletedRows"></param>
        /// <returns></returns>
        public static ShapefileFeatureSource Open(string filename, bool useSpatialIndexing, bool trackDeletedRows)
        {
            var header = new ShapefileHeader(filename);

            switch (header.ShapeType)
            {
                case ShapeType.Polygon:
                case ShapeType.PolygonM:
                case ShapeType.PolygonZ:
                    return new PolygonShapefileFeatureSource(filename, useSpatialIndexing, trackDeletedRows);
                case ShapeType.PolyLine:
                case ShapeType.PolyLineM:
                case ShapeType.PolyLineZ:
                    return new LineShapefileFeatureSource(filename, useSpatialIndexing, trackDeletedRows);
                case ShapeType.Point:
                case ShapeType.PointM:
                case ShapeType.PointZ:
                    return new PointShapefileFeatureSource(filename, useSpatialIndexing, trackDeletedRows);
                default:
                    throw new ClassNotSupportedException(string.Format("Cannot create ShapefileFeatureSource for {0} shape type", header.ShapeType));
            }
        }

        #endregion

        /// <summary>
        /// Update the header to include the feature extent
        /// </summary>
        /// <param name="header"></param>
        /// <param name="feature"></param>
        protected void UpdateHeader(ShapefileHeader header, IBasicGeometry feature)
        {
            UpdateHeader(header, feature, true);
        }


        /// <summary>
        /// Update the header to include the feature extent
        /// </summary>
        /// <param name="header"></param>
        /// <param name="feature"></param>
        /// <param name="writeHeaderFiles"> </param>
        protected void UpdateHeader(ShapefileHeader header, IBasicGeometry feature, bool writeHeaderFiles)
        {
            // Update the envelope
            IEnvelope newExt;
            // First, check to see if there are no features (ShxLength == 50)
            if (header.ShxLength <= 50)
            {
                // This is the lone feature, so just set the extent to the feature extent
                newExt = feature.Envelope;
            }
            else
            {
                // Other features, so include new feature
                newExt = new Envelope(header.Xmin, header.Xmax, header.Ymin, header.Ymax, header.Zmin, header.Zmax);
                newExt.ExpandToInclude(feature.Envelope);
            }
            header.Xmin = newExt.Minimum.X;
            header.Ymin = newExt.Minimum.Y;
            header.Zmin = newExt.Minimum.Z;
            header.Xmax = newExt.Maximum.X;
            header.Ymax = newExt.Maximum.Y;
            header.Zmax = newExt.Maximum.Z;
            header.ShxLength = header.ShxLength + 4;
            if (writeHeaderFiles)
            {
                WriteHeader(header, Filename);
                WriteHeader(header, header.ShxFilename);
            }
        }

        /// <summary>
        /// Update header extents from the geometries in a IShapeSource
        /// </summary>
        /// <param name="src"></param>
        protected void UpdateExtents(IShapeSource src)
        {
            var sr = new ShapeReader(src);
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
            if (null == env)
                return;

            var sh = new ShapefileHeader(Filename);
            sh.SetExtent(env);
            sh.Save();
        }

        /// <summary>
        /// Select the features in an IShapeSource
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="filterExpression"></param>
        /// <param name="envelope"></param>
        /// <param name="startIndex"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        protected IFeatureSet Select(IShapeSource sr, string filterExpression, IEnvelope envelope, ref int startIndex, int maxCount)
        {
            var shapes = sr.GetShapes(ref startIndex, maxCount, envelope);
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
                    var f = new Feature(pair.Value) {RecordNumber = pair.Key + 1, DataRow = td.Rows[0]};
                    result.Features.Add(f);
                    f.UpdateEnvelope();
                }
            }
            return result;
        }

        /// <summary>
        /// Conditionally modify attributes as searched in a single pass via client supplied callback.
        /// </summary>
        /// <param name="sr">ShapeSource for this feature source</param>
        /// <param name="envelope">The envelope for vector filtering.</param>
        /// <param name="chunkSize">Number of shapes to request from the ShapeSource in each chunk</param>
        /// <param name="featureEditCallback">Callback on each feature</param>
        protected void SearchAndModifyAttributes(IShapeSource sr, IEnvelope envelope, int chunkSize, FeatureSourceRowEditEvent featureEditCallback)
        {
            var samp = new ShapefileFeatureSourceSearchAndModifyAttributeParameters(featureEditCallback);
            AttributeTable at = null;
            int startIndex = 0;
            do
            {
                samp.CurrentSearchAndModifyAttributeshapes = sr.GetShapes(ref startIndex, chunkSize, envelope);
                if (samp.CurrentSearchAndModifyAttributeshapes.Count > 0)
                {
                    if (null == at)
                        at = GetAttributeTable(Filename);

                    at.Edit(samp.CurrentSearchAndModifyAttributeshapes.Keys, samp.OnRowEditEvent);
                }
            } while (samp.CurrentSearchAndModifyAttributeshapes.Count == chunkSize);
        }

        /// <inheritdocs/>
        private static void WriteHeader(ShapefileHeader header, string fileName)
        {
            string dir = Path.GetDirectoryName(fileName);
            if (dir != null)
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }

            var bbWriter = new FileStream(fileName, FileMode.Open, FileAccess.Write, FileShare.None, 100);

            bbWriter.WriteBe(header.FileCode);                     //  Byte 0          File Code       9994        Integer     Big

            var bt = new byte[20];
            bbWriter.Write(bt, 0, 20);                                   //  Bytes 4 - 20 are unused

            // This is overwritten later
            bbWriter.WriteBe(0);                //  Byte 24         File Length     File Length Integer     Big

            bbWriter.WriteLe(header.Version);                             //  Byte 28         Version         1000        Integer     Little

            bbWriter.WriteLe((int)header.ShapeType);                      //  Byte 32         Shape Type      Shape Type  Integer     Little

            bbWriter.WriteLe(header.Xmin);                                //  Byte 36         Bounding Box    Xmin        Double      Little

            bbWriter.WriteLe(header.Ymin);                                //  Byte 44         Bounding Box    Ymin        Double      Little

            bbWriter.WriteLe(header.Xmax);                                //  Byte 52         Bounding Box    Xmax        Double      Little

            bbWriter.WriteLe(header.Ymax);                                //  Byte 60         Bounding Box    Ymax        Double      Little

            bbWriter.WriteLe(header.Zmin);                                //  Byte 68         Bounding Box    Zmin        Double      Little

            bbWriter.WriteLe(header.Zmax);                                //  Byte 76         Bounding Box    Zmax        Double      Little

            bbWriter.WriteLe(header.Mmin);                                //  Byte 84         Bounding Box    Mmin        Double      Little

            bbWriter.WriteLe(header.Mmax);                                //  Byte 92         Bounding Box    Mmax        Double      Little

            // ------------ WRITE TO SHP FILE -------------------------

            bbWriter.Close();
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
            } while (1000 == returnedCount);
            Quadtree = qt;
        }

        private AttributeTable GetAttributeTable(string fileName)
        {
            var at = new AttributeTable();
            at.Open(fileName, _deletedRows);
            if (_trackDeletedRows)
                _deletedRows = at.DeletedRows;
            return at;
        }

        /// <summary>
        /// Append the geometry to the shapefile
        /// </summary>
        /// <param name="header"></param>
        /// <param name="feature"></param>
        /// <param name="numFeatures"></param>
        protected abstract void AppendBasicGeometry(ShapefileHeader header, IBasicGeometry feature, int numFeatures);

        /// <summary>
        /// Create an appropriate ShapeSource for this FeatureSource
        /// </summary>
        /// <returns></returns>
        public abstract IShapeSource CreateShapeSource();
    }

    internal class ShapefileFeatureSourceSearchAndModifyAttributeParameters
    {
        public ShapefileFeatureSourceSearchAndModifyAttributeParameters(FeatureSourceRowEditEvent featureEditCallback)
        {
            FeatureEditCallback = featureEditCallback;
        }

        public Dictionary<int, Shape> CurrentSearchAndModifyAttributeshapes { get; set; }

        public FeatureSourceRowEditEvent FeatureEditCallback { get; set; }

        public bool OnRowEditEvent(RowEditEventArgs e)
        {
            var shape = CurrentSearchAndModifyAttributeshapes[e.RowNumber];
            return FeatureEditCallback(new FeatureSourceRowEditEventArgs(e, shape));
        }
    }
}