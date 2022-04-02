// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Projections;
using Pdal;

namespace DotSpatial.Plugins.LiDAR
{
    /// <summary>
    /// LiDarDataSet.
    /// </summary>
    public class LiDarDataSet : IDataSet
    {
        #region Fields

        private readonly Extent _setupExtent;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LiDarDataSet"/> class.
        /// </summary>
        /// <param name="filename">Name of the file that is used in this dataset.</param>
        public LiDarDataSet(string filename)
        {
            // here read the maxX, maxY from the las file header
            // and change the Extent property
            LasReader reader = new(filename);
            reader.initialize();
            ulong pointNum = reader.getNumPoints();
            int arrayLength = (int)pointNum;

            Schema schema = reader.getSchema();
            PointBuffer data = new(schema, (uint)pointNum);

            // get the dimensions (fields) of the point record for the X, Y, and Z values
            int offsetX = schema.getDimensionIndex(DimensionId.Id.X_i32);
            int offsetY = schema.getDimensionIndex(DimensionId.Id.Y_i32);
            int offsetZ = schema.getDimensionIndex(DimensionId.Id.Z_i32);
            Dimension dimensionX = schema.getDimension((uint)offsetX);
            Dimension dimensionY = schema.getDimension((uint)offsetY);
            schema.getDimension((uint)offsetZ);

            // make the iterator to read from the file
            StageSequentialIterator iter = reader.createSequentialIterator();
            iter.read(data);

            int xraw = data.getField_Int32(0, offsetX);
            int yraw = data.getField_Int32(0, offsetY);

            // LAS stores the data in scaled integer form: undo the scaling
            // so we can see the real values as doubles
            double maxX, maxY;
            var minX = maxX = dimensionX.applyScaling_Int32(xraw);
            var minY = maxY = dimensionY.applyScaling_Int32(yraw);

            for (int i = 1; i < arrayLength; i++)
            {
                xraw = data.getField_Int32((uint)i, offsetX);
                yraw = data.getField_Int32((uint)i, offsetY);

                // LAS stores the data in scaled integer form: undo the scaling
                // so we can see the real values as doubles
                double x = dimensionX.applyScaling_Int32(xraw);
                double y = dimensionY.applyScaling_Int32(yraw);

                if (x < minX) minX = x;
                if (x > maxX) maxX = x;
                if (y < minY) minY = y;
                if (y > maxY) maxY = y;
            }

            _setupExtent = new Extent(minX, minY, maxX, maxY);
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public bool CanReproject
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc />
        public Extent Extent
        {
            get
            {
                return _setupExtent;
            }

            set
            {
            }
        }

        /// <inheritdoc />
        public string Filename { get; set; }

        /// <inheritdoc />
        public string FilePath { get; set; }

        /// <inheritdoc />
        public bool IsDisposed => false;

        /// <inheritdoc />
        public bool IsDisposeLocked
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc />
        public string Name
        {
            get
            {
                return "LiDARDataSet";
            }

            set
            {
            }
        }

        /// <inheritdoc />
        public IProgressHandler ProgressHandler
        {
            get
            {
                return null;
            }

            set
            {
            }
        }

        /// <inheritdoc />
        public ProjectionInfo Projection
        {
            get
            {
                return KnownCoordinateSystems.Projected.World.WebMercator;
            }

            set
            {
                // throw new NotImplementedException();
            }
        }

        /// <inheritdoc />
        public string ProjectionString
        {
            get
            {
                return Projection.ToString();
            }

            set
            {
                // throw new NotImplementedException();
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Close()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        /// <summary>
        /// This method simulates loading the array of points from the LAS file.
        /// Right now it is generating random points that are within the
        /// view extent.
        /// </summary>
        /// <param name="boundingBox">the view extent.</param>
        /// <returns>array of the points in [x y x y ... order].</returns>
        public double[] GetPointArray(Extent boundingBox)
        {
            LasReader reader = new("C:\\Tile_1.las");
            reader.initialize();
            ulong numPoints = reader.getNumPoints();
            int arrayLength = (int)numPoints;

            double[] pointArray = new double[numPoints];

            // create the point buffer we'll read into
            // make it only hold 128 points a time, so we can show iterating
            Schema schema = reader.getSchema();
            PointBuffer data = new(schema, (uint)numPoints);

            // get the dimensions (fields) of the point record for the X, Y, and Z values
            int offsetX = schema.getDimensionIndex(DimensionId.Id.X_i32);
            int offsetY = schema.getDimensionIndex(DimensionId.Id.Y_i32);
            int offsetZ = schema.getDimensionIndex(DimensionId.Id.Z_i32);
            Dimension dimensionX = schema.getDimension((uint)offsetX);
            Dimension dimensionY = schema.getDimension((uint)offsetY);
            Dimension dimensionZ = schema.getDimension((uint)offsetZ);

            // make the iterator to read from the file
            StageSequentialIterator iter = reader.createSequentialIterator();
            iter.read(data);
            for (int i = 0; i < arrayLength; i++)
            {
                int xraw = data.getField_Int32((uint)i, offsetX);
                int yraw = data.getField_Int32((uint)i, offsetY);
                int zraw = data.getField_Int32((uint)i, offsetZ);

                // LAS stores the data in scaled integer form: undo the scaling
                // so we can see the real values as doubles
                double x = dimensionX.applyScaling_Int32(xraw);
                double y = dimensionY.applyScaling_Int32(yraw);
                dimensionZ.applyScaling_Int32(zraw);

                pointArray[i] = x;
                i++;
                pointArray[i] = y;
            }

            return pointArray;
        }

        /// <inheritdoc />
        public void LockDispose()
        {
            // throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Reproject(ProjectionInfo targetProjection)
        {
            // throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void UnlockDispose()
        {
            throw new NotImplementedException();
        }

        private void ReadFromLasFile()
        {
            using var ofd = new OpenFileDialog { Filter = @"LAS|*.las" };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            string filename = ofd.FileName;
            LasReader reader = new(filename);
            reader.initialize();
            ulong numPoints = reader.getNumPoints();
        }

        #endregion
    }
}