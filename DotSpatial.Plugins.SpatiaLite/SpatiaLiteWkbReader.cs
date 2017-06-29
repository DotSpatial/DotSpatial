using System.IO;
using DotSpatial.Topology;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Plugins.SpatiaLite
{
    /// <summary>
    /// Helper class for reading binary data from the SpatiaLite database
    /// </summary>
    public class SpatiaLiteWkbReader : WkbReader
    {
        /// <summary>
        /// Convert a byte array to a DotSpatial.Topology geometry object
        /// </summary>
        /// <param name="data">the data from the BLOB column</param>
        /// <returns>the geometry object</returns>
        public override IGeometry Read(byte[] data)
        {
            //specialized Read() method for SpatiaLite
            using (Stream stream = new MemoryStream(data))
            {
                //read first byte
                BinaryReader reader = null;
                var startByte = stream.ReadByte(); //must be "0"
                var byteOrder = (ByteOrder)stream.ReadByte();

                try
                {
                    reader = (byteOrder == ByteOrder.BigEndian) ? new BeBinaryReader(stream) : new BinaryReader(stream);

                    int srid = reader.ReadInt32();
                    double mbrMinX = reader.ReadDouble();
                    double mbrMinY = reader.ReadDouble();
                    double mbrMaxX = reader.ReadDouble();
                    double mbrMaxY = reader.ReadDouble();
                    byte mbrEnd = reader.ReadByte();

                    return Read(reader);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
        }
    }
}