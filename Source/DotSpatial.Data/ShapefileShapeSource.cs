// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        #region Fields

        private readonly ISpatialIndex<int> _spatialIndex;
        private string _filename;

        /// <summary>
        /// Cached contents of shape index file
        /// </summary>
        private ShapefileIndexFile _shx;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileShapeSource"/> class with the specified
        /// shapefile as the source.
        /// </summary>
        /// <param name="fileName">Name of the source file.</param>
        protected ShapefileShapeSource(string fileName)
        {
            Filename = fileName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileShapeSource"/> class with the specified
        /// shapefile as the source and supplied spatial and shape indices.
        /// </summary>
        /// <param name="fileName">Name of the source file.</param>
        /// <param name="spatialIndex">The spatial index.</param>
        /// <param name="shx">The shapefile index file.</param>
        protected ShapefileShapeSource(string fileName, ISpatialIndex<int> spatialIndex, ShapefileIndexFile shx)
        {
            Filename = fileName;
            _spatialIndex = spatialIndex;
            _shx = shx;
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public abstract FeatureType FeatureType { get; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string Filename
        {
            get
            {
                return _filename;
            }

            set
            {
                _filename = Path.GetFullPath(value);
            }
        }

        /// <summary>
        /// Gets the shape type (without M or Z) supported by this shape source.
        /// </summary>
        protected abstract ShapeType ShapeType { get; }

        /// <summary>
        /// Gets the shape type (with M, and no Z) supported by this shape source.
        /// </summary>
        protected abstract ShapeType ShapeTypeM { get; }

        /// <summary>
        /// Gets the shape type (with M and Z) supported by this shape source.
        /// </summary>
        protected abstract ShapeType ShapeTypeZ { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void EndGetShapesSession()
        {
            _shx = null;
        }

        /// <inheritdoc />
        public int GetShapeCount()
        {
            string file = Path.ChangeExtension(Filename, ".shx");
            var fi = new FileInfo(file);
            long len = (fi.Length - 100) / 8;
            return Convert.ToInt32(len);
        }

        /// <inheritdoc />
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
            if (header.ShapeType != ShapeType && header.ShapeType != ShapeTypeM && header.ShapeType != ShapeTypeZ)
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
            if (_spatialIndex != null && envelope != null)
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
                        if (myShape != null)
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
                    if (myShape != null)
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

        /// <inheritdoc />
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
            if (header.ShapeType != ShapeType && header.ShapeType != ShapeTypeM && header.ShapeType != ShapeTypeZ)
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
                    if (index < numShapes) result[j] = GetShapeAtIndex(fs, shx, header, index, null);
                }

                return result;
            }
            finally
            {
                fs.Close();
            }
        }

        /// <summary>
        /// Cache Index File in memory so we don't have to read it in every call to GetShapes.
        /// </summary>
        /// <returns>The cached index file.</returns>
        protected ShapefileIndexFile CacheShapeIndexFile()
        {
            if (_shx == null)
            {
                _shx = new ShapefileIndexFile();
                _shx.Open(Filename);
            }

            return _shx;
        }

        /// <summary>
        /// Get the shape at the specified index. Must be implemented by all shape sources derived from ShapefileShapeSource.
        /// </summary>
        /// <param name="fs">The feature set.</param>
        /// <param name="shx">The shapefile index file.</param>
        /// <param name="header">The shapefile header.</param>
        /// <param name="shp">The shape index.</param>
        /// <param name="envelope">The envelope.</param>
        /// <returns>A shape.</returns>
        protected abstract Shape GetShapeAtIndex(FileStream fs, ShapefileIndexFile shx, ShapefileHeader header, int shp, Envelope envelope);

        #endregion
    }
}