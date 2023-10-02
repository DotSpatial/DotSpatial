// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;
using DotSpatial.Projections;

namespace DemoCustomLayer.DemoCustomLayerExtension
{
    /// <summary>
    /// Shows how to create a custom dataset.
    /// </summary>
    public class MyCustomDataSet : IDataSet
    {
        #region IDataSet Members

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Extent Extent
        {
            get => new(-20000000, -10000000, 20000000, 10000000);
            set
            {
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Filename
        {
            get => null;

            set
            {
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string FilePath
        {
            get => null;

            set
            {
            }
        }

        /// <summary>
        /// This method simulates loading the array of points from the LAS file.
        /// Right now it is generating random points that are within the view extent.
        /// </summary>
        /// <param name="boundingBox">the view extent</param>
        /// <returns>array of the points in [x y x y ... order]</returns>
        public static double[] GetPointArray(Extent boundingBox)
        {
            double[] pointArray = new double[1000];
            Random rnd = new();
            double xMin = boundingBox.MinX;
            double yMin = boundingBox.MinY;

            for (int i = 0; i < 1000; i++)
            {
                double randomX = xMin + rnd.NextDouble() * boundingBox.Width;
                double randomY = yMin + rnd.NextDouble() * boundingBox.Height;
                pointArray[i] = randomX;
                i++;
                pointArray[i] = randomY;
            }

            return pointArray;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsDisposed => false;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Name
        {
            get => "MyCustomDataSet";
            set
            {
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get => null;
            set
            {
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public static string TypeName
        {
            get => "MyCustomDataSet";
            set
            {
            }
        }

        #endregion

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
        }

        #region IDisposeLock Members

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsDisposeLocked => throw new NotImplementedException();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void LockDispose()
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void UnlockDispose()
        {
        }

        #endregion

        #region IReproject Members

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool CanReproject => false;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ProjectionInfo Projection
        {
            get => KnownCoordinateSystems.Projected.World.WebMercator;
            set
            {
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ProjectionString
        {
            get => Projection.ToString();
            set
            {
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Reproject(ProjectionInfo targetProjection)
        {
        }

        #endregion
    }
}
