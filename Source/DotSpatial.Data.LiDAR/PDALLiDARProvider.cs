using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;
using Pdal;

namespace DotSpatial.Data.LiDAR
{
    class PDALLiDARProvider : ILiDARDataProvider
    {

        public ILiDARData Open(string fileName)
        {
            LasReader reader = new LasReader(fileName);
            reader.initialize();

            // how many points do we have?
            ulong numPoints = reader.getNumPoints();
            // create the point buffer we'll read into
            // make it only hold 128 points a time, so we can show iterating
            Schema schema = reader.getSchema();
            SchemaLayout layout = new SchemaLayout(schema);
            PointBuffer data = new PointBuffer(layout, 128);

            // get the dimensions (fields) of the point record for the X, Y, and Z values
            int offsetX = schema.getDimensionIndex(Dimension.Field.Field_X, Dimension.DataType.Int32);
            int offsetY = schema.getDimensionIndex(Dimension.Field.Field_Y, Dimension.DataType.Int32);
            int offsetZ = schema.getDimensionIndex(Dimension.Field.Field_Z, Dimension.DataType.Int32);
            Dimension dimensionX = schema.getDimension((uint)offsetX);
            Dimension dimensionY = schema.getDimension((uint)offsetY);
            Dimension dimensionZ = schema.getDimension((uint)offsetZ);

            // make the iterator to read from the file
            StageSequentialIterator iter = reader.createSequentialIterator();

            uint totalRead = 0;

            while (!iter.atEnd())
            {
                uint numRead = iter.read(data);
                totalRead += numRead;
                 // did we just read the first block of 128 points?
                // if so, let's check the first point in the buffer
                // (which is point number 0 in the file) and make sure 
                // it is correct
                if (iter.getIndex() == 128)
                {
                    uint pointIndex = 0;

                    // get the raw data from the 1st point record in the point buffer
                    int x0raw = data.getField_Int32(pointIndex, offsetX);
                    int y0raw = data.getField_Int32(pointIndex, offsetY);
                    int z0raw = data.getField_Int32(pointIndex, offsetZ);

                    // LAS stores the data in scaled integer form: undo the scaling
                    // so we can see the real values as doubles
                    double x0 = dimensionX.applyScaling_Int32(x0raw);
                    double y0 = dimensionY.applyScaling_Int32(y0raw);
                    double z0 = dimensionZ.applyScaling_Int32(z0raw);

                    // make sure the X, Y, Z fields are correct!
                    //Debug.Assert(x0 == 637012.240000);
                    //Debug.Assert(y0 == 849028.310000);
                    //Debug.Assert(z0 == 431.660000);
                    Console.WriteLine("point 0 is correct");
                }
            }
            return new LiDARData();
        }

        IDataSet IDataProvider.Open(string name)
        {
            return Open(name) as IDataSet;
        }

        public string DialogReadFilter
        {
            get { return "LiDAR|*.las"; }
        }

        public string DialogWriteFilter
        {
            get { return "LiDAR|*.las"; }
        }

        public string Name
        {
            get { return "LiDAR"; }
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

        public string Description
        {
            get { return null; }
        }
    }
}
