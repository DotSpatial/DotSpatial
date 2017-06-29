using System;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Projections;
using Pdal;

namespace DotSpatial.Plugins.LiDAR
{
    public class LiDARDataSet : IDataSet
    {
        private Extent setupExtent;

        public LiDARDataSet(string filename)
        {
            //here read the maxX, maxY from the las file header
            //and change the Extent property
            LasReader Reader = new LasReader(filename);
            Reader.initialize();
            ulong PointNum = Reader.getNumPoints();
            int ArrayLength = (int)PointNum;
            //Range_double range = Reader.getBounds();
            //Reader.getBounds
            //Bounds_double ll = Reader.getBounds();
            Schema schema = Reader.getSchema();
            PointBuffer data = new PointBuffer(schema, (uint)PointNum);

            // get the dimensions (fields) of the point record for the X, Y, and Z values
            int offsetX = schema.getDimensionIndex(DimensionId.Id.X_i32);
            int offsetY = schema.getDimensionIndex(DimensionId.Id.Y_i32);
            int offsetZ = schema.getDimensionIndex(DimensionId.Id.Z_i32);
            Dimension dimensionX = schema.getDimension((uint)offsetX);
            Dimension dimensionY = schema.getDimension((uint)offsetY);
            Dimension dimensionZ = schema.getDimension((uint)offsetZ);

            // make the iterator to read from the file
            StageSequentialIterator iter = Reader.createSequentialIterator();
            uint numRead = iter.read(data);

            int xraw = data.getField_Int32(0, offsetX);
            int yraw = data.getField_Int32(0, offsetY);

            // LAS stores the data in scaled integer form: undo the scaling
            // so we can see the real values as doubles
            double MinX, MaxX, MinY, MaxY;
            MinX = MaxX = dimensionX.applyScaling_Int32(xraw);
            MinY = MaxY = dimensionY.applyScaling_Int32(yraw);

            for (int i = 1; i < ArrayLength; i++)
            {
                xraw = data.getField_Int32((uint)i, offsetX);
                yraw = data.getField_Int32((uint)i, offsetY);

                // LAS stores the data in scaled integer form: undo the scaling
                // so we can see the real values as doubles
                double x = dimensionX.applyScaling_Int32(xraw);
                double y = dimensionY.applyScaling_Int32(yraw);

                if (x < MinX) MinX = x;
                if (x > MaxX) MaxX = x;
                if (y < MinY) MinY = y;
                if (y > MaxY) MaxY = y;
            }
            setupExtent = new Extent(MinX, MinY, MaxX, MaxY);
        }

        #region IDataSet Members

        public void Close()
        {
            throw new NotImplementedException();
        }

        public Extent Extent
        {
            get
            {
                return setupExtent;
            }
            set
            {
            }
        }

        public bool IsDisposed
        {
            get { return false; }
        }

        public string Name
        {
            get
            {
                return "LiDARDataSet";
            }
            set
            { }
        }

        public IProgressHandler ProgressHandler
        {
            get
            {
                return null;
            }
            set
            { }
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
                return "LiDARDataSet";
            }
            set
            {
                // throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

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
            throw new NotImplementedException();
        }

        public bool CanReproject
        {
            get { throw new NotImplementedException(); }
        }

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

        private void ReadFromLasFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "LAS|*.las";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            String Filename = ofd.FileName;
            LasReader Reader = new LasReader(Filename);
            Reader.initialize();
            ulong numPoints = Reader.getNumPoints();
        }

        /// <summary>
        /// This method simulates loading the array of points from the LAS file.
        /// Right now it is generating random points that are within the
        /// view extent.
        /// </summary>
        /// <param name="boundingBox">the view extent</param>
        /// <returns>array of the points in [x y x y ... order]</returns>
        public double[] GetPointArray(Extent boundingBox)
        {
            LasReader Reader = new LasReader("C:\\Tile_1.las");
            Reader.initialize();
            ulong numPoints = Reader.getNumPoints();
            int ArrayLength = (int)numPoints;

            double[] pointArray = new double[numPoints];

            Random rnd = new Random();
            double xMin = boundingBox.MinX;
            double yMin = boundingBox.MinY;

            // create the point buffer we'll read into
            // make it only hold 128 points a time, so we can show iterating
            Schema schema = Reader.getSchema();
            PointBuffer data = new PointBuffer(schema, (uint)numPoints);

            // get the dimensions (fields) of the point record for the X, Y, and Z values
            int offsetX = schema.getDimensionIndex(DimensionId.Id.X_i32);
            int offsetY = schema.getDimensionIndex(DimensionId.Id.Y_i32);
            int offsetZ = schema.getDimensionIndex(DimensionId.Id.Z_i32);
            Dimension dimensionX = schema.getDimension((uint)offsetX);
            Dimension dimensionY = schema.getDimension((uint)offsetY);
            Dimension dimensionZ = schema.getDimension((uint)offsetZ);

            // make the iterator to read from the file
            StageSequentialIterator iter = Reader.createSequentialIterator();
            uint numRead = iter.read(data);
            for (int i = 0; i < ArrayLength; i++)
            {
                int xraw = data.getField_Int32((uint)i, offsetX);
                int yraw = data.getField_Int32((uint)i, offsetY);
                int zraw = data.getField_Int32((uint)i, offsetZ);

                // LAS stores the data in scaled integer form: undo the scaling
                // so we can see the real values as doubles
                double x = dimensionX.applyScaling_Int32(xraw);
                double y = dimensionY.applyScaling_Int32(yraw);
                double z = dimensionZ.applyScaling_Int32(zraw);

                //double randomX = xMin + rnd.NextDouble() * boundingBox.Width;
                //double randomY = yMin + rnd.NextDouble() * boundingBox.Height;
                pointArray[i] = x;
                i = i + 1;
                pointArray[i] = y;
            }
            return pointArray;
        }
    }
}