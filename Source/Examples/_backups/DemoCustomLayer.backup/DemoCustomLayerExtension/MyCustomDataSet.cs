using System;
using DotSpatial.Data;
using DotSpatial.Projections;

namespace DemoCustomLayer.DemoCustomLayerExtension
{
    public class MyCustomDataSet : IDataSet
    {
        #region IDataSet Members

        public void Close()
        {
            //throw new NotImplementedException();
        }

        public Extent Extent
        {
            get
            {
                return new Extent(-20000000,-10000000, 20000000, 10000000);
            }
            set
            {
                //throw new NotImplementedException();
            }
        }

        public string Filename
        {
            get
            {
                return null;
            }

            set
            {
               // throw new NotImplementedException();
            }
        }

        public string FilePath
        {
            get
            {
                return null;
            }

            set
            {
              // throw new NotImplementedException();
            }
        }

        /// <summary>
        /// This method simulates loading the array of points from the LAS file.
        /// Right now it is generating random points that are within the view extent.
        /// </summary>
        /// <param name="boundingBox">the view extent</param>
        /// <returns>array of the points in [x y x y ... order]</returns>
        public double[] GetPointArray(Extent boundingBox)
        {
            double[] pointArray = new double[1000];
            Random rnd = new Random();
            double xMin = boundingBox.MinX;
            double yMin = boundingBox.MinY;

            for (int i = 0; i < 1000; i++)
            {
                double randomX = xMin + rnd.NextDouble() * boundingBox.Width;
                double randomY = yMin + rnd.NextDouble() * boundingBox.Height;
                pointArray[i] = randomX;
                i = i + 1;
                pointArray[i] = randomY;
            }
            return pointArray;
        }

        public bool IsDisposed
        {
            get { return false; }
        }

        public string Name
        {
            get
            {
                return "MyCustomDataSet";
            }
            set
            {
            }
        }

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

        public SpaceTimeSupport SpaceTimeSupport
        {
            get
            {
                return SpaceTimeSupport.Spatial;
            }
            set
            {
                //throw new NotImplementedException();
            }
        }

        public string TypeName
        {
            get
            {
                return "MyCustomDataSet";
            }
            set
            {
                //throw new NotImplementedException();
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region IDisposeLock Members

        public bool IsDisposeLocked
        {
            get { throw new NotImplementedException(); }
        }

        public void LockDispose()
        {
            //throw new NotImplementedException();
        }

        public void UnlockDispose()
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region IReproject Members

        public bool CanReproject
        {
            get { return false; }
        }

        public ProjectionInfo Projection
        {
            get
            {
                return KnownCoordinateSystems.Projected.World.WebMercator;
            }
            set
            {
                //throw new NotImplementedException();
            }
        }

        public string ProjectionString
        {
            get
            {
                return Projection.ToString();
            }
            set
            {
                //throw new NotImplementedException();
            }
        }

        public void Reproject(ProjectionInfo targetProjection)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
