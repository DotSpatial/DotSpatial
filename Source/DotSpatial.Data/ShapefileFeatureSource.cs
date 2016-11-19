﻿// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
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
using DotSpatial.NTSExtension;
using GeoAPI.Geometries;
using NetTopologySuite.Index;

namespace DotSpatial.Data
{
    ///<summary>
    /// Provides common functionality for all ShapefileFeatureSource classes (Point, Line, Polygon)
    ///</summary>
    public abstract class ShapefileFeatureSource
    {
        private string _fileName;

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
        /// Gets or sets the absolute Filename of this ShapefileFeatureSource. If a relative path gets assigned it is changed to the absolute path including the file extension.
        /// </summary>
        public string Filename
        {
            get { return _fileName; }
            set { _fileName = Path.GetFullPath(value); }
        }

        /// <summary>
        /// The integer maximum number of records to return in a single page of results.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the spatial index if any
        /// </summary>
        public ISpatialIndex<int> SpatialIndex
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

        /// <summary>
        /// Updates the file with an additional feature.
        /// </summary>
        /// <param name="feature"></param>
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
                        header.Zmin = feature.Geometry.EnvelopeInternal.Minimum.Z;
                        header.Zmax = feature.Geometry.EnvelopeInternal.Maximum.Z;
                        header.ShapeType = ShapeTypeZ;
                    }
                    header.Mmin = feature.Geometry.EnvelopeInternal.Minimum.M;
                    header.Mmax = feature.Geometry.EnvelopeInternal.Maximum.M;
                }
                header.ShxLength = 4 + 50;
                header.SaveAs(Filename);
            }

            AppendBasicGeometry(header, feature.Geometry, numFeatures);

            feature.RecordNumber = numFeatures;

            if (null != Quadtree)
                Quadtree.Insert(feature.Geometry.EnvelopeInternal, numFeatures - 1);
        }


        /// <summary>
        /// Removes the feature with the specified index
        /// </summary>
        /// <param name="index">Removes the feature from the specified index location</param>
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

                Envelope featureEnv = ifs.GetFeature(0).Geometry.EnvelopeInternal;
                if (featureEnv.MinX <= hdr.Xmin || featureEnv.MaxX >= hdr.Xmax || featureEnv.MaxY >= hdr.Ymax || featureEnv.MinY <= hdr.Ymin)
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


        /// <summary>
        /// The default implementation calls the add method repeatedly.
        /// </summary>
        /// <param name="features">The set of features to add</param>
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
                            header.Zmin = feature.Geometry.EnvelopeInternal.Minimum.Z;
                            header.Zmax = feature.Geometry.EnvelopeInternal.Maximum.Z;
                            header.ShapeType = ShapeTypeZ;
                        }
                        header.Mmin = feature.Geometry.EnvelopeInternal.Minimum.M;
                        header.Mmax = feature.Geometry.EnvelopeInternal.Maximum.M;
                    }
                    header.ShxLength = 4 + 50;
                    header.SaveAs(Filename);
                    filenameExists = true;
                }

                AppendBasicGeometry(header, feature.Geometry, numFeatures);

                feature.RecordNumber = numFeatures;

                if (null != Quadtree)
                    Quadtree.Insert(feature.Geometry.EnvelopeInternal, numFeatures - 1);
            }
        }


        /// <summary>
        /// Select function passed with a filter expression.
        /// </summary>
        /// <param name="filterExpression">The string filter expression based on attributes</param>
        /// <param name="envelope">The envelope for vector filtering.</param>
        /// <param name="startIndex">The integer start index where values should be checked.  This will be updated to the </param>
        /// last index that was checked before the return, so that paging the query can begin with that index in the next cycle.
        /// Be sure to add one before the next call.
        /// <param name="maxCount">The integer maximum number of IFeature values that should be returned.</param>
        /// <returns>A dictionary with FID keys and IFeature values.</returns>
        public abstract IFeatureSet Select(string filterExpression, Envelope envelope, ref int startIndex, int maxCount);


        /// <summary>
        /// Conditionally modify attributes as searched in a single pass via client supplied callback.
        /// </summary>
        /// <param name="envelope">The envelope for vector filtering.</param>
        /// <param name="chunkSize">Number of shapes to request from the ShapeSource in each chunk</param>
        /// <param name="rowCallback">Callback on each feature</param>
        public abstract void SearchAndModifyAttributes(Envelope envelope, int chunkSize, FeatureSourceRowEditEvent rowCallback);


        /// <summary>
        /// Edits the values of the specified row in the attribute table.  Since not all data formats
        /// can handle the dynamic change of attributes, updating vectors can be done with Remove and Add.
        /// </summary>
        /// <param name="fid">The feature offest</param>
        /// <param name="attributeValues">The row of new attribute values.</param>
        public void EditAttributes(int fid, DataRow attributeValues)
        {
            AttributeTable at = GetAttributeTable(Filename);
            at.Edit(fid, attributeValues);
        }


        /// <summary>
        /// Edits the values of the specified rows in the attribute table.
        /// </summary>
        /// <param name="indexDataRowPairs"></param>
        public void EditAttributes(IEnumerable<KeyValuePair<int, DataRow>> indexDataRowPairs)
        {
            AttributeTable at = GetAttributeTable(Filename);
            at.Edit(indexDataRowPairs);
        }


        /// <summary>
        /// Adding and removing shapes may make the bounds for the entire shapefile invalid.
        /// This triggers a check that ensures that the collective extents contain all the shapes.
        /// </summary>
        public abstract void UpdateExtents();

        ///<summary>
        /// The geographic extent of the feature source
        ///</summary>
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
                    throw new ClassNotSupportedException(string.Format("Cannot create ShapefileFeatureSource for {0} shape type", header.ShapeType)); // TODO jany_ why use classNotSupportedException if message not a class name?  resulting Errormessage makes no sense
            }
        }

        #endregion

        /// <summary>
        /// Update the header to include the feature extent
        /// </summary>
        /// <param name="header"></param>
        /// <param name="feature"></param>
        protected void UpdateHeader(ShapefileHeader header, IGeometry feature)
        {
            UpdateHeader(header, feature, true);
        }


        /// <summary>
        /// Update the header to include the feature extent
        /// </summary>
        /// <param name="header"></param>
        /// <param name="feature"></param>
        /// <param name="writeHeaderFiles"> </param>
        protected void UpdateHeader(ShapefileHeader header, IGeometry feature, bool writeHeaderFiles)
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
                newExt.InitM(header.Mmin, header.Mmax);
                newExt.InitZ(header.Zmin, header.Zmax);
                newExt.ExpandToInclude(feature.EnvelopeInternal);
            }
            header.Xmin = newExt.Minimum.X;
            header.Ymin = newExt.Minimum.Y;
            header.Zmin = newExt.Minimum.Z;
            header.Xmax = newExt.Maximum.X;
            header.Ymax = newExt.Maximum.Y;
            header.Zmax = newExt.Maximum.Z;
            header.Mmin = newExt.Minimum.M; //TODO added by jany_ on nts integration ... is this correct?
            header.Mmax = newExt.Maximum.M;
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
        protected IFeatureSet Select(IShapeSource sr, string filterExpression, Envelope envelope, ref int startIndex, int maxCount)
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
                    var f = new Feature(pair.Value) { RecordNumber = pair.Key + 1, DataRow = td.Rows[0] };
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
        protected void SearchAndModifyAttributes(IShapeSource sr, Envelope envelope, int chunkSize, FeatureSourceRowEditEvent featureEditCallback)
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
        protected abstract void AppendBasicGeometry(ShapefileHeader header, IGeometry feature, int numFeatures);

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