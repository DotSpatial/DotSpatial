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
// | Kyle Ellison    |12/15/2010| Added method to get multiple shapes by index values, and consolidated code.
// |-----------------|----------|----------------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using GeoAPI.Geometries;
using NetTopologySuite.Index;
namespace DotSpatial.Data
{
    /// <summary>
    /// Common functionality for retrieving specific shapes.
    /// </summary>
    public abstract class ShapefileShapeSource : IShapeSource
    {
        private string _filename;
        private readonly ISpatialIndex<int> _spatialIndex;

        /// <summary>
        /// Cached contents of shape index file
        /// </summary>
        private ShapefileIndexFile _shx;

        /// <summary>
        /// Creates a new instance of the ShapefileShapeSource with the specified
        /// shapefile as the source.
        /// </summary>
        /// <param name="fileName"></param>
        protected ShapefileShapeSource(string fileName)
        {
            Filename = fileName;
        }

        /// <summary>
        /// Creates a new instance of the ShapefileShapeSource with the specified
        /// shapefile as the source and supplied spatial and shape indices.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="spatialIndex"></param>
        /// <param name="shx"></param>
        protected ShapefileShapeSource(string fileName, ISpatialIndex<int> spatialIndex, ShapefileIndexFile shx)
        {
            Filename = fileName;
            _spatialIndex = spatialIndex;
            _shx = shx;
        }

        /// <inheritdocs/>
        public string Filename
        {
            get { return _filename; }
            set { _filename = Path.GetFullPath(value); }
        }

        /// <summary>
        /// Get the shape type (without M or Z) supported by this shape source
        /// </summary>
        protected abstract ShapeType ShapeType { get; }

        /// <summary>
        /// Get the shape type (with M, and no Z) supported by this shape source
        /// </summary>
        protected abstract ShapeType ShapeTypeM { get; }

        /// <summary>
        /// Get the shape type (with M and Z) supported by this shape source
        /// </summary>
        protected abstract ShapeType ShapeTypeZ { get; }

        #region IShapeSource Members

        /// <inheritdocs/>
        public int GetShapeCount()
        {
            string file = Path.ChangeExtension(Filename, ".shx");
            var fi = new FileInfo(file);
            long len = (fi.Length - 100) / 8;
            return Convert.ToInt32(len);
        }

        /// <inheritdocs/>
        public abstract FeatureType FeatureType { get; }

        /// <inheritdocs/>
        public Dictionary<int, Shape> GetShapes(ref int startIndex, int count, Envelope envelope)
        {
            Dictionary<int, Shape> result = new Dictionary<int, Shape>();
            ShapefileIndexFile shx = CacheShapeIndexFile();

            // Check to ensure the fileName is not null
            if (Filename == null)
            {
                throw new NullReferenceException(Filename);
            }

            if (!File.Exists(Filename))
            {
                throw new FileNotFoundException(Filename);
            }

            // Get the basic header information.
            ShapefileHeader header = new ShapefileHeader(Filename);
            Extent ext = new Extent(new[] { header.Xmin, header.Ymin, header.Xmax, header.Ymax });
            if (envelope != null)
            {
                if (!ext.Intersects(envelope)) return result;
            }

            // Check to ensure that the fileName is the correct shape type
            if (header.ShapeType != ShapeType &&
                 header.ShapeType != ShapeTypeM &&
                 header.ShapeType != ShapeTypeZ)
            {
                throw new ArgumentException("Wrong feature type.");
            }

            FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (fs.Length == 100)
            {
                // The shapefile is empty so we can simply return here
                fs.Close();
                return result;
            }

            int shapesTested = 0;
            int shapesReturned = 0;

            // Use spatial index if we have one
            if (null != _spatialIndex && null != envelope)
            {
                IList<int> spatialQueryResults = _spatialIndex.Query(envelope);

                // Sort the results from low to high index
                var sqra = new int[spatialQueryResults.Count];
                spatialQueryResults.CopyTo(sqra, 0);
                Array.Sort(sqra);

                foreach (int shp in sqra)
                {
                    if (shp >= startIndex)
                    {
                        Shape myShape = GetShapeAtIndex(fs, shx, header, shp, envelope);
                        shapesTested++;
                        if (null != myShape)
                        {
                            shapesReturned++;
                            result.Add(shp, myShape);
                            if (shapesReturned >= count) break;
                        }
                    }
                }
            }
            else
            {
                int numShapes = shx.Shapes.Count;
                for (int shp = startIndex; shp < numShapes; shp++)
                {
                    Shape myShape = GetShapeAtIndex(fs, shx, header, shp, envelope);
                    shapesTested++;
                    if (null != myShape)
                    {
                        shapesReturned++;
                        result.Add(shp, myShape);
                        if (shapesReturned >= count) break;
                    }
                }
            }
            startIndex += shapesTested;
            fs.Close();
            return result;
        }

        /// <inheritdocs/>
        public Shape[] GetShapes(int[] indices)
        {
            Shape[] result = new Shape[indices.Length];

            ShapefileIndexFile shx = CacheShapeIndexFile();

            // Check to ensure the fileName is not null
            if (Filename == null)
            {
                throw new NullReferenceException(Filename);
            }

            if (File.Exists(Filename) == false)
            {
                throw new FileNotFoundException(Filename);
            }

            // Get the basic header information.
            ShapefileHeader header = new ShapefileHeader(Filename);

            // Check to ensure that the fileName is the correct shape type
            if (header.ShapeType != ShapeType &&
                 header.ShapeType != ShapeTypeM &&
                 header.ShapeType != ShapeTypeZ)
            {
                throw new ArgumentException("Wrong feature type.");
            }

            FileStream fs = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
                if (fs.Length == 100)
                {
                    // The shapefile is empty so we can simply return here
                    return result;
                }
                int numShapes = shx.Shapes.Count;
                for (int j = 0; j < indices.Length; j++)
                {
                    int index = indices[j];
                    if (index < numShapes)
                        result[j] = GetShapeAtIndex(fs, shx, header, index, null);
                }
                return result;
            }
            finally
            {
                fs.Close();
            }
        }

        /// <inheritdocs/>
        public void EndGetShapesSession()
        {
            _shx = null;
        }

        #endregion

        /// <summary>
        /// Cache Index File in memory so we don't have to read it in every call to GetShapes
        /// </summary>
        /// <returns></returns>
        protected ShapefileIndexFile CacheShapeIndexFile()
        {
            if (null == _shx)
            {
                _shx = new ShapefileIndexFile();
                _shx.Open(Filename);
            }
            return _shx;
        }

        /// <summary>
        /// Get the shape at the specified index.  Must be implemented by all shape sources derived from ShapefileShapeSource
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="shx"></param>
        /// <param name="header"></param>
        /// <param name="shp"></param>
        /// <param name="envelope"></param>
        /// <returns></returns>
        protected abstract Shape GetShapeAtIndex(FileStream fs, ShapefileIndexFile shx, ShapefileHeader header, int shp, Envelope envelope);
    }
}